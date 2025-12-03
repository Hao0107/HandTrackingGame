using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using System.Collections.Generic;

public class MyHandTrackingController : MonoBehaviour
{
    [Header("Link Object")]
    public SphereDragger sphereDragger;
    public HandLandmarkerRunner handLandmarkerRunner;
    public GameManager gameManager;

    [Header("Controll settings")]
    public float grabThreshold = 0.1f;
    public int controlLandmarkId = 8; // Đầu ngón trỏ
    public bool flipXAxis = false;

    [Header("Smooth")]
    [Range(1f, 50f)]
    public float smoothSpeed = 12f;

    // --- state variable ---
    private float _currentSmoothedX = 0.5f;
    private string _currentControllingHand = "None"; // "Left", "Right", hoặc "None"

    // --- Thread-Safe variable---
    private bool _leftHandDetected_Thread = false;
    private bool _leftHandGrabbing_Thread = false;
    private float _leftHandX_Thread = 0.5f;

    private bool _rightHandDetected_Thread = false;
    private bool _rightHandGrabbing_Thread = false;
    private float _rightHandX_Thread = 0.5f;

    private bool _dataUpdated_Thread = false;

    // --- Main Thread variable ---
    private bool _leftDetected = false;
    private bool _leftGrabbing = false;
    private float _leftX = 0f;

    private bool _rightDetected = false;
    private bool _rightGrabbing = false;
    private float _rightX = 0f;

    private bool _wasGrabbingLastFrame_MainThread = false;
    private bool _waitForRelease = true;

    // data lock variable
    private readonly object _dataLock = new object();

    void Start()
    {
        handLandmarkerRunner.OnLandmarkerResult.AddListener(OnHandLandmarks);
    }

    void OnDestroy()
    {
        if (handLandmarkerRunner != null)
            handLandmarkerRunner.OnLandmarkerResult.RemoveListener(OnHandLandmarks);
    }

    private void OnHandLandmarks(HandLandmarkerResult result)
    {
        // temp variable
        bool t_leftDet = false, t_leftGrab = false; float t_leftX = 0.5f;
        bool t_rightDet = false, t_rightGrab = false; float t_rightX = 0.5f;

        if (result.handLandmarks != null && result.handedness != null)
        {
            // Duyệt qua tất cả các tay được phát hiện
            for (int i = 0; i < result.handLandmarks.Count; i++)
            {
                if (i >= result.handedness.Count) break;

                var landmarks = result.handLandmarks[i].landmarks;
                var handedness = result.handedness[i].categories[0].categoryName; // handedness cho biet "Left" or "Right"

                // Tính toán tọa độ X thô và trạng thái nắm tay
                float rawX = landmarks[controlLandmarkId].x;
                if (flipXAxis) rawX = 1.0f - rawX;

                var thumbTip = landmarks[4];
                var indexTip = landmarks[8];
                float dist = Vector2.Distance(new Vector2(thumbTip.x, thumbTip.y), new Vector2(indexTip.x, indexTip.y));
                bool isGrabbing = dist < grabThreshold;

                // Gán vào biến tạm dựa trên tay trái/phải
                if (handedness == "Left")
                {
                    t_leftDet = true;
                    t_leftGrab = isGrabbing;
                    t_leftX = rawX;
                }
                else if (handedness == "Right")
                {
                    t_rightDet = true;
                    t_rightGrab = isGrabbing;
                    t_rightX = rawX;
                }

                //Debug.LogWarning($"[RAW LANDMARK] X: {rawX:F4} | Grab: {isGrabbing} | Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            }
        }

        lock (_dataLock)
        {
            _leftHandDetected_Thread = t_leftDet;
            _leftHandGrabbing_Thread = t_leftGrab;
            _leftHandX_Thread = t_leftX;

            _rightHandDetected_Thread = t_rightDet;
            _rightHandGrabbing_Thread = t_rightGrab;
            _rightHandX_Thread = t_rightX;

            _dataUpdated_Thread = true;
        }
    }

    void Update()
    {
        bool hasNewData = false;

        // 1. Copy dữ liệu từ Thread
        lock (_dataLock)
        {
            if (_dataUpdated_Thread)
            {
                _leftDetected = _leftHandDetected_Thread;
                _leftGrabbing = _leftHandGrabbing_Thread;
                _leftX = _leftHandX_Thread;

                _rightDetected = _rightHandDetected_Thread;
                _rightGrabbing = _rightHandGrabbing_Thread;
                _rightX = _rightHandX_Thread;

                hasNewData = true;
                _dataUpdated_Thread = false; // Reset cờ
            }
        }

        if (!hasNewData && Time.frameCount % 5 != 0) return; // Tối ưu: không cần tính toán lại nếu không có data mới (trừ khi cần lerp)

        if (gameManager == null || gameManager.GFreeze == null) return;

        if (gameManager.isInMainMenu)
        {
            _waitForRelease = true;
            if (sphereDragger != null) sphereDragger.SetDragging(false);
            return;
        }

        // Ngăn không cho điều khiển khi game over, trong trình chỉnh sửa cấp độ, hoặc khi bảng cài đặt mở, hoac o man hinh chinh
        if (gameManager.isGameOver)
        {
            bool isLeftGrabbing = _leftDetected && _leftGrabbing;
            bool isRightGrabbing = _rightDetected && _rightGrabbing;

            if (isLeftGrabbing && isRightGrabbing)
            {
                Debug.Log("detect 2 hand grabbing --> retry");

                _waitForRelease = true;

                gameManager.RestartGame();
            }
            return;
        }
        ;
        if (gameManager.levelEdit != null && gameManager.levelEdit.isInEditor) return;
        if (gameManager.settingsPanel != null && gameManager.settingsPanel.activeSelf) return;

        bool isGlobalGrabbing = false;
        bool leftGrab = _leftDetected && _leftGrabbing;
        bool rightGrab = _rightDetected && _rightGrabbing;

        if (_waitForRelease)
        {
            if (!leftGrab && !rightGrab)
            {
                _waitForRelease = false;
            }
            else
            {
                return;
            }
        }
        // End ngăn không cho điều khiển

        // 2. LOGIC CHỌN TAY ĐIỀU KHIỂN (CORE LOGIC)

        // Nếu chưa có ai điều khiển
        if (_currentControllingHand == "None")
        {
            if (_rightDetected && _rightGrabbing)
            {
                _currentControllingHand = "Right";
                _currentSmoothedX = _rightX;
            }
            else if (_leftDetected && _leftGrabbing)
            {
                _currentControllingHand = "Left";
                _currentSmoothedX = _leftX;
            }
        }
        // Nếu Tay Phải đang điều khiển
        else if (_currentControllingHand == "Right")
        {
            if (!_rightDetected || !_rightGrabbing) // Mất tay hoặc thả tay
            {
                _currentControllingHand = "None"; // Mất quyền
                // Kiểm tra ngay xem tay trái có đang đợi không để chuyển quyền liền mạch
                if (_leftDetected && _leftGrabbing)
                {
                    _currentControllingHand = "Left";
                    // Không set _currentSmoothedX để nó Lerp từ vị trí cũ sang tay trái cho mượt
                }
            }
        }
        // Nếu Tay Trái đang điều khiển
        else if (_currentControllingHand == "Left")
        {
            if (!_leftDetected || !_leftGrabbing) // Mất tay hoặc thả tay
            {
                _currentControllingHand = "None";
                if (_rightDetected && _rightGrabbing)
                {
                    _currentControllingHand = "Right";
                }
            }
        }

        // 3. THỰC HIỆN ĐIỀU KHIỂN
        isGlobalGrabbing = (_currentControllingHand != "None");
        float targetX = _currentSmoothedX;

        if (_currentControllingHand == "Right") targetX = _rightX;
        else if (_currentControllingHand == "Left") targetX = _leftX;

        // Làm mượt chuyển động
        _currentSmoothedX = Mathf.Lerp(_currentSmoothedX, targetX, Time.deltaTime * smoothSpeed);

        //Debug.Log($"[MAIN THREAD] Target X (Raw): {targetX:F4} | Smoothed X: {_currentSmoothedX:F4}");

        // Cập nhật SphereDragger
        if (sphereDragger != null)
        {
            sphereDragger.UpdateHandPosition(_currentSmoothedX);
            sphereDragger.SetDragging(isGlobalGrabbing);
        }

        // 4. Logic Start Game (Chỉ cần bất kỳ tay nào nắm lần đầu)
        if (isGlobalGrabbing && !_wasGrabbingLastFrame_MainThread)
        {
            if (gameManager != null && gameManager.GFreeze != null && gameManager.GFreeze.gamePaused)
            {
                gameManager.GFreeze.gamePaused = false;
                Time.timeScale = 1f;
            }
        }
        _wasGrabbingLastFrame_MainThread = isGlobalGrabbing;
    }
}