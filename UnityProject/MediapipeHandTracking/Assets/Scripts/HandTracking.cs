using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandTracking : MonoBehaviour
{
    public TargetManager targetManager;

    public UdpReceiver UDPReceive;

    public GameObject[] handPoints;
    public GameObject[] handline;

    [Header("Cursor Objects")]
    public GameObject cursorObject;
    public GameObject leftCursorObject;

    [Header("Sensitivity Settings")]
    public float Control_Sensitivity = 1.5f;
    public float XY_Scale_Factor = 120f;
    public float xPos_Offset = 8f;

    [Header("Cursor & Click Settings")]
    public float Z_Fixed_Position = 4f;
    public float Z_Click_Threshold = -30f;

    private const int TOTAL_LANDMARKS = 42;

    [Header("UI Components")]
    public TextMeshProUGUI NumOfHandsTMP;

    [Header("Don't touch this")]
    public string[] dataPoints;

    private const int RIGHT_HAND_INDEX_TIP = 8;
    private const int LEFT_HAND_INDEX_TIP = 29;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        string data = UDPReceive.returnData;

        if (string.IsNullOrEmpty(data) || data.Length < 2)
        {
            SetAllHandsActive(false);
            NumOfHandsTMP.text = "Hand: 0";
            return;
        }

        // loai bo dau ngoac vuong neu co
        if (data.StartsWith("[")) data = data.Remove(0, 1);
        if (data.EndsWith("]")) data = data.Remove(data.Length - 1, 1);

        dataPoints = data.Split(',');

        // xu ly cho ca hai tay
        if (dataPoints.Length == 126)
        {
            NumOfHandsTMP.text = "Hand: 2";
            //Debug.Log("Detect 2 hands");
            SetAllHandsActive(true);
            leftCursorObject.SetActive(true);

            for (int i = 0; i < TOTAL_LANDMARKS; i++)
            {
                ApplyHandPosition(i, dataPoints);
            }

            CursorManagement(cursorObject, dataPoints, RIGHT_HAND_INDEX_TIP);
            CursorManagement(leftCursorObject, dataPoints, LEFT_HAND_INDEX_TIP);

            targetManager.HandleHandPinch(dataPoints, 0);
            targetManager.HandleHandPinch(dataPoints, 1);
        }

        else if (dataPoints.Length == 63)
        {
            NumOfHandsTMP.text = "Hand: 1";
            //Debug.Log("Detect 1 hand");
            SetHandActive(0, true); //Enable 1 hand
            SetHandActive(1, false);

            for (int i = 0; i < 21; i++)
            {
                if (i * 3 + 2 >= dataPoints.Length || i >= handPoints.Length) break;
                
                ApplyHandPosition(i, dataPoints);

                //hide the other  points
                if (i + 21 < handPoints.Length && handPoints[i + 21] != null)
                {
                    handPoints[i + 21].SetActive(false);
                    leftCursorObject.SetActive(false);
                }
            }
            CursorManagement(cursorObject, dataPoints, RIGHT_HAND_INDEX_TIP);
            targetManager.HandleHandPinch(dataPoints, 0);

            //hide the other hand lines
            for (int i = 0; i<handline.Length; i++)
            {
                handline[i].SetActive(false);
            }
        }
    }

    void ApplyHandPosition(int i, string[] points)
    {
        try
        {
            float x_raw = float.Parse(points[i * 3]);
            float y_raw = float.Parse(points[i * 3 + 1]);
            float z_raw = float.Parse(points[i * 3 + 2]);

            float x_pos = xPos_Offset - (x_raw / XY_Scale_Factor) * Control_Sensitivity;
            float y_pos = (y_raw / XY_Scale_Factor) * Control_Sensitivity;
            float z_pos = z_raw / 50f;

            if (handPoints[i] != null)
            {
                handPoints[i].transform.localPosition = new Vector3(-x_pos, y_pos, z_pos);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("ERROR Parse data: " + e.Message);
        }
    }

    void SetAllHandsActive(bool active)
    {
        foreach (var point in handPoints)
        {
            if (point != null) point.SetActive(active);
        }
        foreach (var line in handline)
        {
            if (line != null) line.SetActive(active);
        }
    }

    void SetHandActive(int handIndex, bool active)
    {
        int startIndex = handIndex * 21;
        int endIndex = startIndex + 21;

        for (int i = startIndex; i < endIndex; i++)
        {
            if (i < handPoints.Length && handPoints[i] != null)
            {
                handPoints[i].SetActive(active);
            }
        }
    }


    void CursorManagement(GameObject cursor, string[] points, int landmarkIndex)
    {
        if (cursor == null) return;

        int dataIndex_X = landmarkIndex * 3;
        int dataIndex_Y = landmarkIndex * 3 + 1;

        if (dataIndex_Y >= points.Length) return;

        float x_raw = float.Parse(points[dataIndex_X]);
        float y_raw = float.Parse(points[dataIndex_Y]);

        float x_pos = xPos_Offset - (x_raw / XY_Scale_Factor) * Control_Sensitivity;
        float y_pos = (y_raw / XY_Scale_Factor) * Control_Sensitivity;

        cursor.transform.localPosition = new Vector3(-x_pos, y_pos, Z_Fixed_Position);
    }
}
