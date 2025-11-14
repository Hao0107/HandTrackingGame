using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using TMPro;

using Mediapipe.Unity;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers; // need for Landmark/Category
using Mediapipe.Unity.Sample.HandLandmarkDetection; // need for HandLandmarkerRunner

public class MediaPipeHandler : MonoBehaviour
{
    [Header("Plugin Dependencies")]
    public HandLandmarkerRunner handLandmarkerRunner;

    [Header("Game Logic Dependencies")]
    public TargetManager targetManager;
    public GameObject cursorObject;
    public GameObject leftCursorObject;
    public TextMeshProUGUI NumOfHandsTMP;

    [Header("2D Cursor Settings (Calibration)")]
    public float Horizontal_Scale = 20f;
    public float Vertical_Scale = 10f;
    public float Horizontal_Offset = 0f;
    public float Vertical_Offset = 0f;

    public bool Flip_X_Axis = true;
    public bool Flip_Y_Axis = true;

    public float Z_Fixed_Position = 4f;

    private const int INDEX_TIP_INDEX = 8;

    private readonly object _dataLock = new object();
    private HandLandmarkerResult? _latestResult = null;
    private bool _hasNewData = false;

    void Start()
    {
        if (handLandmarkerRunner == null)
        {
            Debug.LogError("Lack of HandLandmarkerRunner!");
            return;
        }
        handLandmarkerRunner.OnLandmarkerResult.AddListener(OnLandmarksReceived);
    }

    void Update()
    {
        if (!_hasNewData) return;

        HandLandmarkerResult? currentResult;
        lock (_dataLock)
        {
            currentResult = _latestResult;
            _hasNewData = false;
        }

        if (currentResult.HasValue)
        {
            ProcessHandData(currentResult.Value);
        }
    }

    void OnLandmarksReceived(HandLandmarkerResult result)
    {
        lock (_dataLock)
        {
            _latestResult = result;
            _hasNewData = true;
        }
    }

    // 3D (World) Normalized for Pinching
    List<Vector3> ConvertWorldLandmarkList(IReadOnlyList<Landmark> landmarkList)
    {
        return landmarkList.Select(lm => new Vector3(-lm.x, lm.y, -lm.z)).ToList();
    }

    // 2D Normalized for Cursor
    List<NormalizedLandmark> ConvertNormalizedLandmarkList(IReadOnlyList<NormalizedLandmark> landmarkList)
    {
        return landmarkList.ToList();
    }

    void ProcessHandData(HandLandmarkerResult result)
    {
        List<Vector3> rightHandWorldPoints = null;
        List<Vector3> leftHandWorldPoints = null;
        List<NormalizedLandmark> rightHandNormalizedPoints = null;
        List<NormalizedLandmark> leftHandNormalizedPoints = null;

        // 1. LẤY DỮ LIỆU
        if (result.handWorldLandmarks != null)
        {
            for (int i = 0; i < result.handWorldLandmarks.Count; i++)
            {
                var handednessContainer = result.handedness[i];
                string handednessLabel = handednessContainer.categories[0].categoryName;
                var landmarksContainer = result.handWorldLandmarks[i];
                var worldPoints = ConvertWorldLandmarkList(landmarksContainer.landmarks);

                if (handednessLabel == "Right") rightHandWorldPoints = worldPoints;
                else if (handednessLabel == "Left") leftHandWorldPoints = worldPoints;
            }
        }
        if (result.handLandmarks != null)
        {
            for (int i = 0; i < result.handLandmarks.Count; i++)
            {
                var handednessContainer = result.handedness[i];
                string handednessLabel = handednessContainer.categories[0].categoryName;
                var landmarksContainer = result.handLandmarks[i];
                var normalizedPoints = ConvertNormalizedLandmarkList(landmarksContainer.landmarks);

                if (handednessLabel == "Right") rightHandNormalizedPoints = normalizedPoints;
                else if (handednessLabel == "Left") leftHandNormalizedPoints = normalizedPoints;
            }
        }

        // 2. GỌI LOGIC GAME (VỚI LOGIC DỌN DẸP)

        // --- TAY PHẢI (Hand 0) ---
        if (rightHandNormalizedPoints != null && rightHandWorldPoints != null)
        {
            // Có Tay Phải: Cập nhật
            if (cursorObject != null) cursorObject.SetActive(true);
            CursorManagement(cursorObject, rightHandNormalizedPoints, INDEX_TIP_INDEX);
            targetManager.HandleHandPinchPlugin(rightHandWorldPoints, 0);
        }
        else
        {
            // Không có Tay Phải: Ẩn Cursor và gọi Dọn Dẹp
            if (cursorObject != null) cursorObject.SetActive(false);
            targetManager.StopPinching(0); // <<< SỬA LỖI: GỌI HÀM MỚI
        }

        // --- TAY TRÁI (Hand 1) ---
        if (leftHandNormalizedPoints != null && leftHandWorldPoints != null)
        {
            // Có Tay Trái: Cập nhật
            if (leftCursorObject != null) leftCursorObject.SetActive(true);
            CursorManagement(leftCursorObject, leftHandNormalizedPoints, INDEX_TIP_INDEX);
            targetManager.HandleHandPinchPlugin(leftHandWorldPoints, 1);
        }
        else
        {
            // Không có Tay Trái: Ẩn Cursor và gọi Dọn Dẹp
            if (leftCursorObject != null) leftCursorObject.SetActive(false);
            targetManager.StopPinching(1); // <<< SỬA LỖI: GỌI HÀM MỚI
        }

        // --- CẬP NHẬT UI TEXT ---
        if (NumOfHandsTMP != null)
        {
            int handCount = (rightHandWorldPoints != null ? 1 : 0) + (leftHandWorldPoints != null ? 1 : 0);
            NumOfHandsTMP.text = "Hand: " + handCount;
        }
    }

    void CursorManagement(GameObject cursor, List<NormalizedLandmark> landmarks, int landmarkIndex)
    {
        if (cursor == null || landmarks == null || landmarks.Count <= landmarkIndex) return;

        Vector3 rawPos = new Vector3(landmarks[landmarkIndex].x, landmarks[landmarkIndex].y, 0);

        if (Flip_X_Axis)
        {
            rawPos.x = 1.0f - rawPos.x;
        }
        if (Flip_Y_Axis)
        {
            rawPos.y = 1.0f - rawPos.y;
        }
        float x_pos = (rawPos.x - 0.5f) * Horizontal_Scale;
        float y_pos = (rawPos.y - 0.5f) * Vertical_Scale;

        x_pos += Horizontal_Offset;
        y_pos += Vertical_Offset;
        float z_pos = Z_Fixed_Position;

        cursor.transform.localPosition = new Vector3(x_pos, y_pos, z_pos);
    }
}