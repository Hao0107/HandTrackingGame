using UnityEngine;
using System;

public class PinchHitbox : MonoBehaviour
{
    private TargetManager manager;

    void Start()
    {
        manager = FindObjectOfType<TargetManager>();
        // Destroy(gameObject, 0.5f); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target") && manager != null)
        {
            Vector3 targetPosition = other.transform.position;

            manager.currentScore += manager.pointsPerTarget;
            manager.UpdateScoreDisplay();

            if (manager.explosionEffectPrefabs.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, manager.explosionEffectPrefabs.Length);
                GameObject selectedExplosion = manager.explosionEffectPrefabs[randomIndex];
                Instantiate(selectedExplosion, targetPosition, Quaternion.identity);
            }

            Destroy(other.gameObject);
        }
    }
}