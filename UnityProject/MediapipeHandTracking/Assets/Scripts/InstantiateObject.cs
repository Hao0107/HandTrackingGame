using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

public class TargetManager : MonoBehaviour
{
    // Tham chiếu đến script HandTracking để lấy dữ liệu điểm
    [Header("Dependencies")]
    public HandTracking handTrackingScript;

    [Header("Target Settings")]
    public GameObject targetPrefab;
    public float spawnInterval = 2f;
    public float spawnRangeX = 5f;   // Phạm vi X ngẫu nhiên (từ -4 đến 4)
    public float spawnRangeY = 8.0f;   // Phạm vi Y ngẫu nhiên (từ 0 đến 4)
    public float targetZ = 4f;      // Vị trí Z cố định của Target (nên khớp với Z_Fixed_Position)

    [Header("Score Settings")]
    public TextMeshProUGUI scoreTextTMP; // Kéo đối tượng TextMeshPro UI vào đây
    public int currentScore = 0;
    public int pointsPerTarget = 10;

    [Header("Effects")]
    public GameObject[] explosionEffectPrefabs;

    [Header("Pinch Settings")]
    public float pinchThreshold = 0.5f;
    public GameObject pinchEffectPrefab;
    public float pinchCheckRadius = 1.5f;

    //[Header("Hands setting")]
    private bool isRightPinching = false;
    private GameObject activeRightPinchEffect;

    private bool isLeftPinching = false;
    private GameObject activeLeftPinchEffect;


    // Constants Landmark
    private const int THUMB_TIP_INDEX = 4;
    private const int INDEX_TIP_INDEX = 8;

    private float nextSpawnTime = 0f;

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
    }

    void SpawnTarget()
    {
        if (targetPrefab == null) return;

        float randomX = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX);
        float randomY = UnityEngine.Random.Range(2.1f, spawnRangeY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, targetZ);

        Quaternion spawnRotation = Quaternion.Euler(90f, 90f, 90f);

        Instantiate(targetPrefab, spawnPosition, spawnRotation);
    }   

    public void HandleHandPinch(string[] points, int handIndex)
    {
        try
        {
            float distance = GetPinchDistance(points, handIndex);

            ref bool isPinchingState = ref (handIndex == 0 ? ref isRightPinching : ref isLeftPinching);
            ref GameObject activeEffect = ref (handIndex == 0 ? ref activeRightPinchEffect : ref activeLeftPinchEffect);

            int thumbIndex = (handIndex * 21) + THUMB_TIP_INDEX;
            int indexIndex = (handIndex * 21) + INDEX_TIP_INDEX;
            Vector3 thumbPos = GetLandmarkPosition(points, thumbIndex);
            Vector3 indexPos = GetLandmarkPosition(points, indexIndex);
            Vector3 pinchPosition = (thumbPos + indexPos) / 2f;


            if (distance < pinchThreshold)
            {
                if (!isPinchingState) // First Pinch
                {
                    isPinchingState = true;

                    if (pinchEffectPrefab != null && activeEffect == null)
                    {
                        // PinchHitbox generation
                        activeEffect = Instantiate(pinchEffectPrefab, pinchPosition, Quaternion.identity);
                    }
                }

                if (activeEffect != null)
                {
                    activeEffect.transform.position = pinchPosition;
                }
            }
            else // NOT PINCH
            {
                if (isPinchingState) // First Pinch release
                {
                    if (activeEffect != null)
                    {
                        Destroy(activeEffect);
                        activeEffect = null;
                    }
                }

                isPinchingState = false;
            }
        }
        catch (FormatException e)
        {
            Debug.LogWarning($"ERROR at Parse data Pinch Hand {handIndex}: " + e.Message);
        }
    }

    Vector3 GetLandmarkPosition(string[] points, int landmarkIndex)
    {
        int dataIndex = landmarkIndex * 3;

        float x_raw = float.Parse(points[dataIndex]);
        float y_raw = float.Parse(points[dataIndex + 1]);
        float z_raw = float.Parse(points[dataIndex + 2]);

        float x_pos = handTrackingScript.xPos_Offset - (x_raw / handTrackingScript.XY_Scale_Factor) * handTrackingScript.Control_Sensitivity;
        float y_pos = (y_raw / handTrackingScript.XY_Scale_Factor) * handTrackingScript.Control_Sensitivity;
        float z_pos = handTrackingScript.Z_Fixed_Position;

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
}