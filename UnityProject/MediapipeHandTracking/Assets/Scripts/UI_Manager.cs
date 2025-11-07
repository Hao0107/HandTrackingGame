using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Header("UI Control Settings")]
    public Slider targetSlider;
    //public TextMeshPro selectedValueText;
    public GameObject UIHandCursor;

    [Header("Slider Mapping Range(for hand)")]
    public float minYPosition = 1.0f;
    public float maxYPosition = 5.0f;

    public void HighlightSlider(float normalizedYPosition)
    {
        if (targetSlider == null) return;

        float value = (normalizedYPosition - minYPosition) / (maxYPosition - minYPosition);

        value = Mathf.Clamp01(value);

        targetSlider.value = value;

        //if (selectedValueText != null)
        //{
        //    selectedValueText.text = $"Value: {targetSlider.value:F2}";
        //}
    }

    public void ConfirmSelection()
    {
        if (targetSlider == null) return;

        Debug.Log($"CONFIRMED SELECTION: Slider value set to {targetSlider.value:F2}");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
