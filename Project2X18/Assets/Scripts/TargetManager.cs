using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class TargetManager : MonoBehaviour
{
    [Header("Dependencies")]
    public MediaPipeHandler mediaPipeHandler;

    [Header("Target Prefabs")]
    public GameObject goodTargetPrefab;
    public GameObject badTargetPrefab;

    // ... (Các biến Spawn, Score, Effects) ...
    [Header("Spawn Settings")]
    public TextMeshProUGUI scoreTextTMP;
    public int currentScore = 0;
    public int pointsPerTarget = 10;
    public GameObject[] explosionEffectPrefabs;
    public float spawnInterval = 2f;
    public float badTargetSpawnChance = 0.3f;
    private const int maxSpawnAttempts = 10;
    public const float spawnRangeX = 200;
    public const float spawnRangeY = 120;
    public const float targetZ = 0f;
    public float minDistanceBetweenTargets = 30f;

    [Header("Pinch Settings")]
    // QUAN TRỌNG: Ngưỡng này phải RẤT NHỎ (vì dùng 3D mét)
    public float pinchThreshold = 0.05f;
    public GameObject pinchEffectPrefab;

    // Trạng thái Pinch
    private bool isRightPinching = false;
    private GameObject activeRightPinchEffect;
    private bool isLeftPinching = false;
    private GameObject activeLeftPinchEffect;

    // Constants Landmark (Index trong List 21 điểm)
    private const int THUMB_TIP_INDEX = 4;
    private const int INDEX_TIP_INDEX = 8;

    private float nextSpawnTime = 0f;
    private List<GameObject> activeTargets = new List<GameObject>();

    void Start()
    {
        activeTargets = new List<GameObject>();
        UpdateScoreDisplay();
    }

    void Update()
    {
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

        if (prefabToSpawn == null) return;

        Vector3 spawnPosition = Vector3.zero;
        bool foundValidPosition = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float randomX = UnityEngine.Random.Range(20, spawnRangeX);
            float randomY = UnityEngine.Random.Range(20, spawnRangeY);
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
        return true;
    }

    public void HandleHandPinchPlugin(List<Vector3> worldLandmarks, int handIndex)
    {
        if (worldLandmarks == null || mediaPipeHandler == null) return;

        try
        {
            float distance = GetPinchDistancePlugin(worldLandmarks);

            ref bool isPinchingState = ref (handIndex == 0 ? ref isRightPinching : ref isLeftPinching);
            ref GameObject activeEffect = ref (handIndex == 0 ? ref activeRightPinchEffect : ref activeLeftPinchEffect);

            Vector3 pinchPosition;
            if (handIndex == 0)
            {
                pinchPosition = mediaPipeHandler.cursorObject.transform.position;
            }
            else
            {
                pinchPosition = mediaPipeHandler.leftCursorObject.transform.position;
            }


            if (distance < pinchThreshold) // ĐANG PINCH
            {
                if (!isPinchingState) // First Pinch
                {
                    isPinchingState = true;
                    if (pinchEffectPrefab != null && activeEffect == null)
                    {
                        activeEffect = Instantiate(pinchEffectPrefab, pinchPosition, Quaternion.identity);
                    }
                }
                if (activeEffect != null)
                {
                    activeEffect.transform.position = pinchPosition;
                }
            }
            else // Not PINCH
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
        catch (Exception e)
        {
            Debug.LogError($"Lỗi Pinch Hand {handIndex}: " + e.Message);
        }
    }

    public float GetPinchDistancePlugin(List<Vector3> landmarks)
    {
        return Vector3.Distance(landmarks[THUMB_TIP_INDEX], landmarks[INDEX_TIP_INDEX]);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreDisplay();
    }

    public void DeductScore(int amount)
    {
        currentScore -= amount;
        if (currentScore < 0) currentScore = 0;
        UpdateScoreDisplay();
    }

    public void UpdateScoreDisplay()
    {
        if (scoreTextTMP != null)
        {
            scoreTextTMP.text = $"{currentScore}";
        }
    }

    public void RemoveTargetFromList(GameObject targetToRemove)
    {
        if (activeTargets.Contains(targetToRemove))
        {
            activeTargets.Remove(targetToRemove);
        }
    }

    public void StopPinching(int handIndex)
    {
        // Xác định đúng biến trạng thái và hiệu ứng
        ref bool isPinchingState = ref (handIndex == 0 ? ref isRightPinching : ref isLeftPinching);
        ref GameObject activeEffect = ref (handIndex == 0 ? ref activeRightPinchEffect : ref activeLeftPinchEffect);

        // Nếu tay đang ở trạng thái Pinch (nhưng giờ đã biến mất)
        if (isPinchingState)
        {
            // Hủy vệt sáng (Hitbox)
            if (activeEffect != null)
            {
                Destroy(activeEffect);
                activeEffect = null;
            }
            // Đặt lại trạng thái
            isPinchingState = false;
        }
    }
}