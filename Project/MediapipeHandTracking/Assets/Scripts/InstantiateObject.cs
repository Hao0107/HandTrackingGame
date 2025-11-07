using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

public class TargetManager : MonoBehaviour
{
    // Tham chiếu đến script HandTracking để lấy dữ liệu điểm
    public HandTracking handTrackingScript;

    [Header("Target Settings")]
    public GameObject targetPrefab;
    public float spawnInterval = 2f;
    public float spawnRangeX = 4f;   // Phạm vi X ngẫu nhiên (từ -4 đến 4)
    public float spawnRangeY = 4f;   // Phạm vi Y ngẫu nhiên (từ 0 đến 4)
    public float targetZ = 4f;      // Vị trí Z cố định của Target (nên khớp với Z_Fixed_Position)

    [Header("Score Settings")]
    public TextMeshProUGUI scoreTextTMP; // Kéo đối tượng TextMeshPro UI vào đây
    public int currentScore = 0;
    public int pointsPerTarget = 10;

    [Header("Effects")]
    public GameObject[] explosionEffectPrefabs;

    [Header("Pinch Settings")]
    public float pinchThreshold = 0.5f; // Khoảng cách tối đa (đơn vị Unity) để coi là Pinch

    [Header("UI Gesture Control")]
    public float FingerUpThreshold = 0.5f;
    public bool isUIFingerDown = false;

    // Index của các điểm: Ngón cái (4) và Ngón trỏ (8)
    private const int THUMB_TIP_INDEX = 4;
    private const int INDEX_TIP_INDEX = 8;

    private float nextSpawnTime = 0f;
    private bool isPinching = false; // Biến cờ tránh spam Pinch

    void Start()
    {
        if (handTrackingScript == null)
        {
            Debug.LogError("Thiếu tham chiếu đến HandTracking Script!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Logic Sinh ra Target
        if (Time.time >= nextSpawnTime)
        {
            SpawnTarget();
            nextSpawnTime = Time.time + spawnInterval;
        }

        // Chỉ xử lý Pinch nếu có ít nhất 1 tay được phát hiện
        if (handTrackingScript.dataPoints != null && handTrackingScript.dataPoints.Length >= 63)
        {
            HandlePinch(handTrackingScript.dataPoints);
        }
    }

    void SpawnTarget()
    {
        if (targetPrefab == null) return;

        float randomX = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX);
        float randomY = UnityEngine.Random.Range(0, spawnRangeY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, targetZ);

        Quaternion spawnRotation = Quaternion.Euler(90f, 90f, 90f);

        Instantiate(targetPrefab, spawnPosition, spawnRotation);
    }

    void HandlePinch(string[] points)
    {

        try
        {
            Vector3 thumbPos = GetLandmarkPosition(points, THUMB_TIP_INDEX);
            Vector3 indexPos = GetLandmarkPosition(points, INDEX_TIP_INDEX);

            float distance = Vector3.Distance(thumbPos, indexPos);

            Vector3 cursorPos = handTrackingScript.cursorObject.transform.position;

            if (distance < pinchThreshold)
            {
                if (!isPinching)
                {
                    CheckForTargetDestruction(cursorPos);
                    isPinching = true;
                }
            }
            else
            {
                isPinching = false;
            }
        }
        catch (FormatException e)
        {
            Debug.LogWarning("Lỗi Parse data trong TargetManager: " + e.Message);
        }
    }

    void CheckForTargetDestruction(Vector3 cursorPos)
    {
        float checkRadius = 1.5f;
        Collider[] colliders = Physics.OverlapSphere(cursorPos, checkRadius);

        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("Target"))
            {
                Vector3 targetPosition = col.transform.position;

                if (explosionEffectPrefabs.Length > 0)
                {
                    // Chọn một chỉ số ngẫu nhiên trong mảng
                    int randomIndex = UnityEngine.Random.Range(0, explosionEffectPrefabs.Length);

                    // Lấy Prefab ngẫu nhiên
                    GameObject selectedExplosion = explosionEffectPrefabs[randomIndex];

                    // SINH RA HIỆU ỨNG NỔ
                    Instantiate(selectedExplosion, targetPosition, Quaternion.identity);
                }

                Debug.Log("TARGET DESTROYED by PINCH!");

                Destroy(col.gameObject);

                currentScore += pointsPerTarget;
                UpdateScoreDisplay();

                break;
            }
        }
    }

    Vector3 GetLandmarkPosition(string[] points, int landmarkIndex)
    {
        int dataIndex = landmarkIndex * 3;

        float x_raw = float.Parse(points[dataIndex]);
        float y_raw = float.Parse(points[dataIndex + 1]);
        float z_raw = float.Parse(points[dataIndex + 2]);

        float x_pos = 5f - x_raw / handTrackingScript.XY_Scale_Factor;
        float y_pos = y_raw / handTrackingScript.XY_Scale_Factor;
        float z_pos = z_raw / 50f;

        return new Vector3(-x_pos, y_pos, z_pos);
    }

    // UI

    public void UpdateScoreDisplay()
    {
        if (scoreTextTMP != null)
        {
            scoreTextTMP.text = $"{currentScore}";
        }
    }

    public float GetPinchDistance(string[] points, int handIndex)
    {
        int offset = handIndex * 21;

        Vector3 thumbPos = GetLandmarkPosition(points, offset + THUMB_TIP_INDEX);
        Vector3 indexPos = GetLandmarkPosition(points, offset + INDEX_TIP_INDEX);

        return Vector3.Distance(thumbPos, indexPos);
    }

    public bool CheckFingerUp(string[] points, int landmarkTipIndex, int landmarkPIPIndex)
    {
        if (landmarkTipIndex * 3 + 2 >= points.Length) return false;

        // Lấy vị trí Y của đầu ngón và đốt thứ 2 (sau khi đã chuẩn hóa)
        float tipY = GetLandmarkPosition(points, landmarkTipIndex).y;
        float pipY = GetLandmarkPosition(points, landmarkPIPIndex).y;

        // Ngón tay giơ lên nếu đầu ngón cao hơn đốt ngón tay liền kề
        // (Lưu ý: Nếu bạn có flip Y, logic này có thể cần đảo ngược)
        return tipY > pipY + FingerUpThreshold;
    }

    public void HandleUISelection(string[] points)
    {
        // Chỉ số bắt đầu cho Tay Trái (21)
        const int LEFT_HAND_OFFSET = 21;

        int fingerCount = 0;

        // Các cặp (Tip Index, PIP Index) cho 4 ngón (trừ Ngón Cái)
        int[] tipIndices = { 28, 32, 36, 40 };
        int[] pipIndices = { 26, 30, 34, 38 };

        for (int i = 0; i < 4; i++)
        {
            if (CheckFingerUp(points, tipIndices[i], pipIndices[i]))
            {
                fingerCount++;
                Debug.Log("FingerCount: " + fingerCount);
            }
        }

        // Kiểm tra Ngón Cái (logic khác)
        float thumbDist = GetLandmarkPosition(points, LEFT_HAND_OFFSET + 4).x - GetLandmarkPosition(points, LEFT_HAND_OFFSET + 3).x;
        if (thumbDist > 0.5f) fingerCount++;

        // 2. Xử lý Lựa Chọn UI
        // Tùy thuộc vào fingerCount, bạn có thể Highlight một nút UI

        if (fingerCount > 0 && fingerCount < 6)
        {
            Debug.Log("UI Selection: Highlight Item " + fingerCount);
            // Ví dụ: Gọi hàm UI_Manager.HighlightItem(fingerCount);
        }

        // 3. Xử lý Pinch để XÁC NHẬN LỰA CHỌN (CLICK)
        float pinchDist = GetPinchDistance(points, 1); // Lấy Pinch cho Tay Trái

        if (pinchDist < pinchThreshold)
        {
            if (!isUIFingerDown)
            {
                Debug.Log($"UI Action: CONFIRM Click on Item {fingerCount}");
                // Ví dụ: Gọi hàm UI_Manager.ExecuteAction(fingerCount);
                isUIFingerDown = true;
            }
        }
        else
        {
            isUIFingerDown = false;
        }
    }
}