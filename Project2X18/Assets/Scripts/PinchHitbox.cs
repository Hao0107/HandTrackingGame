using UnityEngine;
using System;

public class PinchHitbox : MonoBehaviour
{
    private TargetManager manager;

    void Start()
    {
        manager = FindObjectOfType<TargetManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (manager == null) return;

        BadTargetController badTarget = other.gameObject.GetComponent<BadTargetController>();
        if (badTarget != null)
        {
            badTarget.OnHit();
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Target"))
        {
            Vector3 targetPosition = other.transform.position;

            manager.AddScore(manager.pointsPerTarget);

            if (manager.explosionEffectPrefabs.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, manager.explosionEffectPrefabs.Length);
                GameObject selectedExplosion = manager.explosionEffectPrefabs[randomIndex];
                Instantiate(selectedExplosion, targetPosition, Quaternion.identity);
            }

            manager.RemoveTargetFromList(other.gameObject);
            Destroy(other.gameObject);
            //Destroy(gameObject);
        }
    }
}