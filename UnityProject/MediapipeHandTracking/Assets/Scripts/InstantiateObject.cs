using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    [Header("Dependencies")]
    public HandTracking handTrackingScript;

    [Header("Target Settings")]
    public GameObject goodTargetPrefab;
    public GameObject badTargetPrefab;
    public float spawnInterval = 2f;
    public float spawnRangeX = 5f;   // Phạm vi X ngẫu nhiên (từ -4 đến 4)
    public float spawnRangeY = 8.0f;   // Phạm vi Y ngẫu nhiên (từ 0 đến 4)
    public float targetZ = 4f;      // Vị trí Z cố định của Target (nên khớp với Z_Fixed_Position)

    [Header("Spawn Collision Settings")]
    public float minDistanceBetweenTargets = 1.5f; // Khoảng cách tối thiểu giữa các Target
    public int maxSpawnAttempts = 10;
    public float badTargetSpawnChance = 0.2f;

    [Header("Score Settings")]
    public TextMeshProUGUI scoreTextTMP;
    public int currentScore = 0;
    public int pointsPerTarget = 10;
    private List<GameObject> activeTargets = new List<GameObject>();

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
        GameObject prefabToSpawn;

        if (UnityEngine.Random.value < badTargetSpawnChance)
        {
            prefabToSpawn = badTargetPrefab;
        }
        else
        {
            prefabToSpawn = goodTargetPrefab;
        }

        Vector3 spawnPosition = Vector3.zero;
        bool foundValidPosition = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float randomX = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX);
            float randomY = UnityEngine.Random.Range(2.1f, spawnRangeY);

            Vector3 potentialPosition = new Vector3(randomX, randomY, targetZ);

            if (IsPositionValid(potentialPosition))
            {
                spawnPosition = potentialPosition;
                foundValidPosition = true;
                break;
            }
        }
        if (foundValidPosition)
        {
            float randomRotation = UnityEngine.Random.Range(0, 360f);
            Quaternion spawnRotation = Quaternion.Euler(randomRotation, 90f, 90f);
            GameObject newTarget = Instantiate(prefabToSpawn, spawnPosition, spawnRotation);
            activeTargets.Add(newTarget);
        }
    }

    public void RemoveTargetFromList(GameObject targetToRemove)
    {
        if (activeTargets.Contains(targetToRemove))
        {
            activeTargets.Remove(targetToRemove);
        }
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (GameObject target in activeTargets)
        {
            if (target == null) continue;

            float distance = Vector3.Distance(position, target.transform.position);

            if (distance < minDistanceBetweenTargets)
            {
                return false;
            }
        }
        return true; // Hợp lệ
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

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreDisplay();
    }

    public void DeductScore(int amount)
    {
        if(currentScore - amount < 0)
        {
            currentScore = 0;
        }
        else
        {
            currentScore -= amount;
        }
        UpdateScoreDisplay();
    }

    public float GetPinchDistance(string[] points, int handIndex)
    {
        int offset = handIndex * 21;

        Vector3 thumbPos = GetLandmarkPosition(points, offset + THUMB_TIP_INDEX);
        Vector3 indexPos = GetLandmarkPosition(points, offset + INDEX_TIP_INDEX);

        return Vector3.Distance(thumbPos, indexPos);
    }
}