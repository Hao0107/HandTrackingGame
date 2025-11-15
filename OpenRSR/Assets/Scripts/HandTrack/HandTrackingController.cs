using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using System.Collections.Generic;

public class MyHandTrackingController : MonoBehaviour
{
    [Header("Đối tượng cần liên kết")]
    public SphereDragger sphereDragger;
    public HandLandmarkerRunner handLandmarkerRunner;
    public GameManager gameManager;

    [Header("Cài đặt điều khiển")]
    public float grabThreshold = 0.1f;
    public int controlLandmarkId = 8;
    public bool flipXAxis = false;

    // --- Biến cho Luồng chính (Update) ---
    private bool _isGrabbingNow = false;
    private float _normalizedX = 0f;
    private bool _isHandDetectedInCallback = false;
    private bool _wasGrabbingLastFrame_MainThread = false;

    // --- Biến "nháp" an toàn cho Luồng phụ (Thread-Safe) ---
    private bool _isGrabbing_Thread = false;
    private float _normalizedX_Thread = 0f;
    private bool _isHandDetected_Thread = false;

    // "Chìa khóa" để khóa dữ liệu
    private readonly object _dataLock = new object();


    void Start()
    {
        if (sphereDragger == null)
            Debug.LogError("MyHandTrackingController: Chưa gán SphereDragger!");
        if (handLandmarkerRunner == null)
            Debug.LogError("MyHandTrackingController: Chưa gán HandLandmarkerRunner!");
        if (gameManager == null)
            Debug.LogError("MyHandTrackingController: Chưa gán GameManager!");

        handLandmarkerRunner.OnLandmarkerResult.AddListener(OnHandLandmarks);
    }

    void OnDestroy()
    {
        if (handLandmarkerRunner != null)
            handLandmarkerRunner.OnLandmarkerResult.RemoveListener(OnHandLandmarks);
    }

    /// <summary>
    /// HÀM NÀY CHẠY TRÊN LUỒNG PHỤ (WORKER THREAD).
    /// Chỉ tính toán và ghi vào biến "nháp".
    /// </summary>
    private void OnHandLandmarks(HandLandmarkerResult result)
    {
        bool tempIsGrabbing = false;
        float tempNormalizedX = 0f;
        bool tempIsHandDetected = false;

        // 1. Tính toán (làm bên ngoài lock để không lãng phí thời gian)
        if (result.handLandmarks != null && result.handLandmarks.Count > 0)
        {
            var firstHandContainer = result.handLandmarks[0];
            if (firstHandContainer.landmarks != null && firstHandContainer.landmarks.Count > 0)
            {
                IReadOnlyList<NormalizedLandmark> landmarks = firstHandContainer.landmarks;

                var controlLandmark = landmarks[controlLandmarkId];
                tempNormalizedX = controlLandmark.x;
                if (flipXAxis) tempNormalizedX = 1.0f - tempNormalizedX;

                var thumbTip = landmarks[4];
                var indexTip = landmarks[8];
                float distance = Vector2.Distance(new Vector2(thumbTip.x, thumbTip.y), new Vector2(indexTip.x, indexTip.y));
                tempIsGrabbing = distance < grabThreshold;
                tempIsHandDetected = true;
            }
        }

        // 2. Khóa và ghi dữ liệu vào biến "nháp"
        lock (_dataLock)
        {
            _isGrabbing_Thread = tempIsGrabbing;
            _normalizedX_Thread = tempNormalizedX;
            _isHandDetected_Thread = tempIsHandDetected;
        }
    }


    /// <summary>
    /// HÀM NÀY CHẠY TRÊN LUỒNG CHÍNH (MAIN THREAD).
    /// Đọc dữ liệu an toàn và gọi API của Unity.
    /// </summary>
    void Update()
    {
        // 1. Khóa và copy dữ liệu từ "nháp" sang "chính"
        lock (_dataLock)
        {
            _isGrabbingNow = _isGrabbing_Thread;
            _normalizedX = _normalizedX_Thread;
            _isHandDetectedInCallback = _isHandDetected_Thread;

            // Reset cờ nháp ngay lập tức
            _isHandDetected_Thread = false;
        }

        // 2. Xử lý logic game (chỉ dùng biến "chính")
        if (_isHandDetectedInCallback)
        {
            // Tay đang được phát hiện
            sphereDragger.UpdateHandPosition(_normalizedX);
            sphereDragger.SetDragging(_isGrabbingNow);

            // Kiểm tra sự kiện "vừa nắm" (Grab Down)
            if (_isGrabbingNow && !_wasGrabbingLastFrame_MainThread)
            {
                if (gameManager.GFreeze.gamePaused)
                {
                    gameManager.GFreeze.gamePaused = false;
                    Time.timeScale = 1f;
                }
            }
            _wasGrabbingLastFrame_MainThread = _isGrabbingNow;
        }
        else
        {
            // Không có tay
            sphereDragger.SetDragging(false);
            _wasGrabbingLastFrame_MainThread = false;
        }
    }
}