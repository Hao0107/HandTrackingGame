using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadTarget : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 360f;

    [Header("Score Penalty")]
    public int penaltyAmount = 20;

    private TargetManager targetManager;

    void Start()
    {
        targetManager = FindObjectOfType<TargetManager>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }

    public void OnHit()
    {
        if (targetManager != null)
        {
            targetManager.DeductScore(penaltyAmount);

            targetManager.RemoveTargetFromList(gameObject);
        }
        Destroy(gameObject);
    }
}
