using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandTracking : MonoBehaviour
{
    public TargetManager targetManager;
    public UI_Manager uiManager;

    public UdpReceiver UDPReceive;

    public GameObject[] handPoints;
    public GameObject[] handline;
    public GameObject cursorObject;
    public float XY_Scale_Factor = 120f;

    [Header("Cursor & Click Settings")]
    public float Z_Fixed_Position = 4f;
    public float Z_Click_Threshold = -30f;
    private bool isPunching = false;
    private bool isPinching = false;

    private const int TOTAL_LANDMARKS = 42;

    [Header("UI Components")]
    public TextMeshProUGUI NumOfHandsTMP;

    [Header("Don't touch this")]
    public string[] dataPoints;
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

            for (int i = 0; i < TOTAL_LANDMARKS; i++)
            {
                if (i * 3 + 2 >= dataPoints.Length || i >= handPoints.Length)
                {
                    break;
                }
                ApplyHandPosition(i, dataPoints);
            }

            CursorManagement(dataPoints, 8);
            targetManager.HandleUISelection(dataPoints);
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
                }
            }
            CursorManagement(dataPoints, 8);
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

            float x_pos = 5f - x_raw / XY_Scale_Factor;
            float y_pos = y_raw / XY_Scale_Factor;
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


    void CursorManagement(string[] points, int landmarkIndex)
    {
        // Tính toán chỉ số data
        int dataIndex_X = landmarkIndex * 3;
        int dataIndex_Y = landmarkIndex * 3 + 1;
        int dataIndex_Z = landmarkIndex * 3 + 2;

        if (dataIndex_Z >= points.Length) return;

        // 1. Quản lý Vị trí (X, Y)
        float x_raw = float.Parse(points[dataIndex_X]);
        float y_raw = float.Parse(points[dataIndex_Y]);
        float x_pos = 5f - x_raw / XY_Scale_Factor;
        float y_pos = y_raw / XY_Scale_Factor;

        // Di chuyển Cursor GameObject ở Z cố định
        cursorObject.transform.localPosition = new Vector3(-x_pos, y_pos, Z_Fixed_Position);


        // 2. Quản lý Hành động (Z) - Click/Punch
        float z_tip = float.Parse(points[dataIndex_Z]);

        if (z_tip < Z_Click_Threshold)
        {
            if (!isPunching)
            {
                // Hành động CLICK/PUNCH chỉ diễn ra MỘT LẦN
                Debug.Log("ACTION: PUNCH/CLICK! Z=" + z_tip);

                // --- THỰC HIỆN LOGIC GAME TẠI ĐÂY ---
                // Ví dụ: Kích hoạt animation đấm cho cursor, kiểm tra va chạm với Target
                // cursorObject.GetComponent<Animator>().SetTrigger("Punch"); 
                // ------------------------------------

                isPunching = true;
            }
        }
        else // Z đã quay lại (tay rút ra/quay về)
        {
            isPunching = false;
        }
    }

    // UI
    void UIManagement(string[] points, int landmarkIndex)
    {
        if (uiManager == null) return;

        // Lấy Vị trí Ngón Cái Tay Trái (Landmark 4 của Tay 2 = Index 25)
        int dataIndex_Y = landmarkIndex * 3 + 1;
        int dataIndex_Z = landmarkIndex * 3 + 2; // Dùng cho Pinch

        if (dataIndex_Y >= points.Length) return;

        // Lấy vị trí Y đã được scale/flip (từ code cũ của bạn)
        float y_raw = float.Parse(points[dataIndex_Y]);
        float y_pos = y_raw / XY_Scale_Factor;

        // 1. HIGHLIGHT: Gọi hàm HighlightSlider
        uiManager.HighlightSlider(y_pos);

        // 2. PINCH: Xử lý Xác nhận (Confirm)
        float pinchDist = targetManager.GetPinchDistance(points, 21); // Giả sử bạn đang dùng TargetManager để tính Pinch

        if (pinchDist < targetManager.pinchThreshold)
        {
            if (!isPinching) // isPinching là biến cờ để tránh spam
            {
                uiManager.ConfirmSelection();
                isPinching = true;
            }
        }
        else
        {
            isPinching = false;
        }
    }
}
