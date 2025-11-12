using UnityEngine;
using System;

public class PinchHitbox : MonoBehaviour
{
    private TargetManager manager;

    [Header("Bad Explosion Effect")]
    public GameObject badExplosionEffectPrefab;

    void Start()
    {
        manager = FindObjectOfType<TargetManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (manager == null) return;

        Vector3 targetPosition = other.transform.position;

        BadTarget badTarget = other.gameObject.GetComponent<BadTarget>();
        if (badTarget != null)
        {
            badTarget.OnHit();
            Instantiate(badExplosionEffectPrefab, targetPosition, Quaternion.identity);
            return;
        }

        if (other.gameObject.CompareTag("Target") && manager != null)
        {

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