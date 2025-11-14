using UnityEngine;

public class BadTargetController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 360f;

    [Header("Score Penalty")]
    public int penaltyAmount = 20;

    [Header("Explosion Effects")]
    public GameObject explosionEffectPrefabs;

    private TargetManager targetManager;

    void Start()
    {
        targetManager = FindObjectOfType<TargetManager>();
    }

    void Update()
    {
        // Xoay liên tục
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }

    public void OnHit()
    {
        if (targetManager != null)
        {
            Instantiate(explosionEffectPrefabs, transform.position, Quaternion.identity);
            targetManager.DeductScore(penaltyAmount);
            targetManager.RemoveTargetFromList(gameObject);
        }
        Destroy(gameObject);
    }
}