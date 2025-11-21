using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ThemeEditor : MonoBehaviour
{
    public GameObject editThemePanel;
    public GameObject eventDropdown;
    private GameObject themeEventFields;
    private GameObject filterEventFields;
    private GameObject grayscaleFilterFields;
    private GameObject grayscaleEndFilterFields;
    private GameObject chromaticFilterFields;
    private GameObject chromaticEndFilterFields;
    private GameObject negativeFilterFields;
    private GameObject negativeEndFilterFields;
    private GameObject glitchFilterFields;
    private GameObject glitchEndFilterFields;
    private GameObject scanLinesFilterFields;
    private GameObject scanLinesEndFilterFields;
    private GameObject hueShiftFilterFields;
    private GameObject hueShiftEndFilterFields;
    private GameObject speedEventFields;
    public TMP_Dropdown eventDropdownDropdown;
    private GameObject filterDropdown;
    public TMP_Dropdown filterDropdownDropdown;
    private GameObject transitionDropdown;
    public TMP_Dropdown transitionDropdownDropdown;
    TMP_InputField themeIDInputField;
    public int themeID = 0;
    public int duration = 0;
    public float multiplier = 1f;
    public int hue = 0;
    public int hueEnd = 0;
    public int saturation = 0;
    public int saturationEnd = 0;
    public int value = 0;
    public int valueEnd = 0;
    public int xOffset = 0;
    public int xOffsetEnd = 0;
    public int yOffset = 0;
    public int yOffsetEnd = 0;
    public int intensity = 0;
    public int intensityEnd = 0;
    public int stability = 0;
    public int stabilityEnd = 0;
    public int shiftOffset = 0;
    public int shiftOffsetEnd = 0;
    public int endAt = 0;
    private float z_position = 0f;
    public string event_type = "";
    public string filter_type = "";
    public GameObject closeButtonObj;
    Button closeButton;

    private void Start() {
        themeEventFields = editThemePanel.transform.Find("ThemeEventFields").gameObject;
        filterEventFields = editThemePanel.transform.Find("FilterEventFields").gameObject;
        grayscaleFilterFields = filterEventFields.transform.Find("GrayScaleFilterFields").gameObject;
        grayscaleEndFilterFields = filterEventFields.transform.Find("GrayScaleEndFilterFields").gameObject;
        chromaticFilterFields = filterEventFields.transform.Find("ChromaticFilterFields").gameObject;
        chromaticEndFilterFields = filterEventFields.transform.Find("ChromaticEndFilterFields").gameObject;
        negativeFilterFields = filterEventFields.transform.Find("NegativeFilterFields").gameObject;
        negativeEndFilterFields = filterEventFields.transform.Find("NegativeEndFilterFields").gameObject;
        glitchFilterFields = filterEventFields.transform.Find("GlitchFilterFields").gameObject;
        glitchEndFilterFields = filterEventFields.transform.Find("GlitchEndFilterFields").gameObject;
        scanLinesFilterFields = filterEventFields.transform.Find("ScanLinesFilterFields").gameObject;
        scanLinesEndFilterFields = filterEventFields.transform.Find("ScanLinesEndFilterFields").gameObject;
        hueShiftFilterFields = filterEventFields.transform.Find("HueShiftFilterFields").gameObject;
        hueShiftEndFilterFields = filterEventFields.transform.Find("HueShiftEndFilterFields").gameObject;
        speedEventFields = editThemePanel.transform.Find("SpeedEventFields").gameObject;
        eventDropdown = editThemePanel.transform.GetChild(1).gameObject;
        eventDropdownDropdown = eventDropdown.GetComponent<TMP_Dropdown>();
        filterDropdown = filterEventFields.transform.GetChild(1).gameObject;
        filterDropdownDropdown = filterDropdown.GetComponent<TMP_Dropdown>();
        transitionDropdown = filterEventFields.transform.Find("TransitionDropdown").gameObject;
        transitionDropdownDropdown = transitionDropdown.GetComponent<TMP_Dropdown>();
        AddListenerToAllTextFields();
    }

    public void OpenEditThemePanel(float z_position)
    {
        if (transform.position.z != z_position) return;
        this.z_position = z_position;
        
        editThemePanel.SetActive(true);
        themeEventFields = editThemePanel.transform.Find("ThemeEventFields").gameObject;
        filterEventFields = editThemePanel.transform.Find("FilterEventFields").gameObject;
        grayscaleFilterFields = filterEventFields.transform.Find("GrayScaleFilterFields").gameObject;
        grayscaleEndFilterFields = filterEventFields.transform.Find("GrayScaleEndFilterFields").gameObject;
        chromaticFilterFields = filterEventFields.transform.Find("ChromaticFilterFields").gameObject;
        chromaticEndFilterFields = filterEventFields.transform.Find("ChromaticEndFilterFields").gameObject;
        negativeFilterFields = filterEventFields.transform.Find("NegativeFilterFields").gameObject;
        negativeEndFilterFields = filterEventFields.transform.Find("NegativeEndFilterFields").gameObject;
        glitchFilterFields = filterEventFields.transform.Find("GlitchFilterFields").gameObject;
        glitchEndFilterFields = filterEventFields.transform.Find("GlitchEndFilterFields").gameObject;
        scanLinesFilterFields = filterEventFields.transform.Find("ScanLinesFilterFields").gameObject;
        scanLinesEndFilterFields = filterEventFields.transform.Find("ScanLinesEndFilterFields").gameObject;
        hueShiftFilterFields = filterEventFields.transform.Find("HueShiftFilterFields").gameObject;
        hueShiftEndFilterFields = filterEventFields.transform.Find("HueShiftEndFilterFields").gameObject;
        speedEventFields = editThemePanel.transform.Find("SpeedEventFields").gameObject;
        eventDropdown = editThemePanel.transform.GetChild(1).gameObject;
        eventDropdownDropdown = eventDropdown.GetComponent<TMP_Dropdown>();
        transitionDropdown = filterEventFields.transform.Find("TransitionDropdown").gameObject;
        transitionDropdownDropdown = transitionDropdown.GetComponent<TMP_Dropdown>();
        switch (eventDropdownDropdown.captionText.text) {
            case "Theme Change":
                event_type = "theme_change";
                themeEventFields.SetActive(true);
                filterEventFields.SetActive(false);
                speedEventFields.SetActive(false);
                themeIDInputField = themeEventFields.transform.GetChild(1).GetComponent<TMP_InputField>();
                themeIDInputField.text = this.themeID.ToString();
                break;
            case "Filter Change":
                event_type = "filter_change";
                themeEventFields.SetActive(false);
                filterEventFields.SetActive(true);
                speedEventFields.SetActive(false);
                filterDropdown = filterEventFields.transform.GetChild(1).gameObject;
                filterDropdownDropdown = filterDropdown.GetComponent<TMP_Dropdown>();
                switch (filterDropdownDropdown.captionText.text) {
                    case "Grayscale":
                        filter_type = "grayscale";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(true);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField hueInputField = grayscaleFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                        hueInputField.text = hue.ToString();
                        TMP_InputField saturationInputField = grayscaleFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                        saturationInputField.text = saturation.ToString();
                        TMP_InputField valueInputField = grayscaleFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                        valueInputField.text = value.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(true);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField hueInputField = grayscaleEndFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                        hueInputField.text = hueEnd.ToString();
                        TMP_InputField saturationInputField = grayscaleEndFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                        saturationInputField.text = saturationEnd.ToString();
                        TMP_InputField valueInputField = grayscaleEndFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                        valueInputField.text = valueEnd.ToString();
                        }
                        break;
                    case "Chromatic Aberration":
                        filter_type = "chromatic";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(true);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField xOffsetInputField = chromaticFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        xOffsetInputField.text = xOffset.ToString();
                        TMP_InputField yOffsetInputField = chromaticFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        yOffsetInputField.text = yOffset.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(true);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField xOffsetInputField = chromaticEndFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        xOffsetInputField.text = xOffsetEnd.ToString();
                        TMP_InputField yOffsetInputField = chromaticEndFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        yOffsetInputField.text = yOffsetEnd.ToString();
                        }
                        break;
                    case "Negative":
                        filter_type = "negative";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(true);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField intensityInputField = negativeFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                        intensityInputField.text = intensity.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(true);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField intensityInputField = negativeEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                        intensityInputField.text = intensityEnd.ToString();
                        }
                        break;
                    case "Glitch":
                        filter_type = "glitch";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(true);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField stabilityInputField = glitchFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                        stabilityInputField.text = stability.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(true);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField stabilityInputField = glitchEndFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                        stabilityInputField.text = stabilityEnd.ToString();
                        }
                        break;
                    case "Scan Lines":
                        filter_type = "scan_lines";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(true);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField intensity2InputField = negativeFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                        intensity2InputField.text = intensity.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(true);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField intensity2InputField = negativeEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                        intensity2InputField.text = intensityEnd.ToString();
                        }
                        break;
                    case "Hue Shift":
                        filter_type = "hue_shift";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(true);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField shiftOffsetInputField = hueShiftFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        shiftOffsetInputField.text = shiftOffset.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(true);
                        TMP_InputField shiftOffsetInputField = hueShiftEndFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        shiftOffsetInputField.text = shiftOffsetEnd.ToString();
                        }
                        break;
                }
                transitionDropdownDropdown.onValueChanged.AddListener(delegate { OnTransitionDropDownValueChanged(transitionDropdownDropdown); });
                TMP_InputField endAtInputField = filterEventFields.transform.Find("EndAtInput").gameObject.GetComponent<TMP_InputField>();
                endAtInputField.text = endAt.ToString();
                filterDropdownDropdown.onValueChanged.AddListener(delegate { OnFilterDropDownValueChanged(filterDropdownDropdown); });
                break;
            case "Speed Change":
                event_type = "speed_change";
                themeEventFields.SetActive(false);
                filterEventFields.SetActive(false);
                speedEventFields.SetActive(true);
                TMP_InputField multiplierInputField = speedEventFields.transform.Find("MultiplierInput").gameObject.GetComponent<TMP_InputField>();
                multiplierInputField.text = multiplier.ToString();
                TMP_InputField durationInputField = speedEventFields.transform.Find("DurationInput").gameObject.GetComponent<TMP_InputField>();
                durationInputField.text = duration.ToString();
                break;
        }
        eventDropdownDropdown.onValueChanged.AddListener(delegate { OnDropDownValueChanged(eventDropdownDropdown); });
        closeButton = closeButtonObj.GetComponent<Button>();
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseEditThemePanel);
        LevelEditor le = GameObject.Find("Balus").GetComponent<LevelEditor>();
        le.SetPopupOpen(true);
    }

    public void OnDropDownValueChanged(TMP_Dropdown eventDropdownDropdown) {
        switch (eventDropdownDropdown.captionText.text) {
            case "Theme Change":
                event_type = "theme_change";
                themeEventFields.SetActive(true);
                filterEventFields.SetActive(false);
                speedEventFields.SetActive(false);
                themeIDInputField = themeEventFields.transform.GetChild(1).GetComponent<TMP_InputField>();
                themeIDInputField.text = themeID.ToString();
                break;
            case "Filter Change":
                event_type = "filter_change";
                themeEventFields.SetActive(false);
                filterEventFields.SetActive(true);
                speedEventFields.SetActive(false);
                filterDropdown = filterEventFields.transform.GetChild(1).gameObject;
                filterDropdownDropdown = filterDropdown.GetComponent<TMP_Dropdown>();
                switch (filterDropdownDropdown.captionText.text) {
                    case "Grayscale":
                        filter_type = "grayscale";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(true);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField hueInputField = grayscaleFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                        hueInputField.text = hue.ToString();
                        TMP_InputField saturationInputField = grayscaleFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                        saturationInputField.text = saturation.ToString();
                        TMP_InputField valueInputField = grayscaleFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                        valueInputField.text = value.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(true);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField hueInputField = grayscaleEndFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                        hueInputField.text = hueEnd.ToString();
                        TMP_InputField saturationInputField = grayscaleEndFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                        saturationInputField.text = saturationEnd.ToString();
                        TMP_InputField valueInputField = grayscaleEndFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                        valueInputField.text = valueEnd.ToString();
                        }
                        break;
                    case "Chromatic Aberration":
                        filter_type = "chromatic";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(true);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField xOffsetInputField = chromaticFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        xOffsetInputField.text = xOffset.ToString();
                        TMP_InputField yOffsetInputField = chromaticFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        yOffsetInputField.text = yOffset.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(true);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField xOffsetInputField = chromaticEndFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        xOffsetInputField.text = xOffsetEnd.ToString();
                        TMP_InputField yOffsetInputField = chromaticEndFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        yOffsetInputField.text = yOffsetEnd.ToString();
                        }
                        break;
                    case "Negative":
                        filter_type = "negative";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(true);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField intensityInputField = negativeFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                        intensityInputField.text = intensity.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(true);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField intensityInputField = negativeEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                        intensityInputField.text = intensityEnd.ToString();
                        }
                        break;
                    case "Glitch":
                        filter_type = "glitch";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(true);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField stabilityInputField = glitchFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                        stabilityInputField.text = stability.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(true);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField stabilityInputField = glitchEndFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                        stabilityInputField.text = stabilityEnd.ToString();
                        }
                        break;
                    case "Scan Lines":
                        filter_type = "scan_lines";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(true);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField intensity2InputField = negativeFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                        intensity2InputField.text = intensity.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(true);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField intensity2InputField = negativeEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                        intensity2InputField.text = intensityEnd.ToString();
                        }
                        break;
                    case "Hue Shift":
                        filter_type = "hue_shift";
                        if (transitionDropdownDropdown.captionText.text == "Start") {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(true);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(false);
                        TMP_InputField shiftOffsetInputField = hueShiftFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        shiftOffsetInputField.text = shiftOffset.ToString();
                        } else {
                        grayscaleFilterFields.SetActive(false);
                        chromaticFilterFields.SetActive(false);
                        negativeFilterFields.SetActive(false);
                        glitchFilterFields.SetActive(false);
                        scanLinesFilterFields.SetActive(false);
                        hueShiftFilterFields.SetActive(false);
                        grayscaleEndFilterFields.SetActive(false);
                        chromaticEndFilterFields.SetActive(false);
                        negativeEndFilterFields.SetActive(false);
                        glitchEndFilterFields.SetActive(false);
                        scanLinesEndFilterFields.SetActive(false);
                        hueShiftEndFilterFields.SetActive(true);
                        TMP_InputField shiftOffsetInputField = hueShiftEndFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                        shiftOffsetInputField.text = shiftOffsetEnd.ToString();
                        }
                        break;
                }
                transitionDropdownDropdown.onValueChanged.AddListener(delegate { OnTransitionDropDownValueChanged(transitionDropdownDropdown); });
                TMP_InputField endAtInputField = filterEventFields.transform.Find("EndAtInput").gameObject.GetComponent<TMP_InputField>();
                endAtInputField.text = endAt.ToString();
                filterDropdownDropdown.onValueChanged.AddListener(delegate { OnFilterDropDownValueChanged(filterDropdownDropdown); });
                break;
            case "Speed Change":
                event_type = "speed_change";
                themeEventFields.SetActive(false);
                filterEventFields.SetActive(false);
                speedEventFields.SetActive(true);
                TMP_InputField multiplierInputField = speedEventFields.transform.Find("MultiplierInput").gameObject.GetComponent<TMP_InputField>();
                multiplierInputField.text = multiplier.ToString();
                TMP_InputField durationInputField = speedEventFields.transform.Find("DurationInput").gameObject.GetComponent<TMP_InputField>();
                durationInputField.text = duration.ToString();
                break;
        }
    }

    public void OnFilterDropDownValueChanged(TMP_Dropdown filterDropdownDropdown) {
        switch (filterDropdownDropdown.captionText.text) {
            case "Grayscale":
                filter_type = "grayscale";
                if (transitionDropdownDropdown.captionText.text == "Start") {
                grayscaleFilterFields.SetActive(true);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField hueInputField = grayscaleFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                hueInputField.text = hue.ToString();
                TMP_InputField saturationInputField = grayscaleFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                saturationInputField.text = saturation.ToString();
                TMP_InputField valueInputField = grayscaleFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                valueInputField.text = value.ToString();
                } else {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(true);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField hueInputField = grayscaleEndFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                hueInputField.text = hueEnd.ToString();
                TMP_InputField saturationInputField = grayscaleEndFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                saturationInputField.text = saturationEnd.ToString();
                TMP_InputField valueInputField = grayscaleEndFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                valueInputField.text = valueEnd.ToString();
                }
                break;
            case "Chromatic Aberration":
                filter_type = "chromatic";
                if (transitionDropdownDropdown.captionText.text == "Start") {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(true);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField xOffsetInputField = chromaticFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                xOffsetInputField.text = xOffset.ToString();
                TMP_InputField yOffsetInputField = chromaticFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                yOffsetInputField.text = yOffset.ToString();
                } else {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(true);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField xOffsetInputField = chromaticEndFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                xOffsetInputField.text = xOffsetEnd.ToString();
                TMP_InputField yOffsetInputField = chromaticEndFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                yOffsetInputField.text = yOffsetEnd.ToString();
                }
                break;
            case "Negative":
                filter_type = "negative";
                if (transitionDropdownDropdown.captionText.text == "Start") {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(true);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField intensityInputField = negativeFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensityInputField.text = intensity.ToString();
                } else {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(true);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField intensityInputField = negativeEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensityInputField.text = intensityEnd.ToString();
                }
                break;
            case "Glitch":
                filter_type = "glitch";
                if (transitionDropdownDropdown.captionText.text == "Start") {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(true);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField stabilityInputField = glitchFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                stabilityInputField.text = stability.ToString();
                } else {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(true);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField stabilityInputField = glitchEndFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                stabilityInputField.text = stabilityEnd.ToString();
                }
                break;
            case "Scan Lines":
                filter_type = "scan_lines";
                if (transitionDropdownDropdown.captionText.text == "Start") {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(true);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField intensity2InputField = negativeFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensity2InputField.text = intensity.ToString();
                } else {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(true);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField intensity2InputField = negativeEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensity2InputField.text = intensityEnd.ToString();
                }
                break;
            case "Hue Shift":
                filter_type = "hue_shift";
                if (transitionDropdownDropdown.captionText.text == "Start") {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(true);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(false);
                TMP_InputField shiftOffsetInputField = hueShiftFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                shiftOffsetInputField.text = shiftOffset.ToString();
                } else {
                grayscaleFilterFields.SetActive(false);
                chromaticFilterFields.SetActive(false);
                negativeFilterFields.SetActive(false);
                glitchFilterFields.SetActive(false);
                scanLinesFilterFields.SetActive(false);
                hueShiftFilterFields.SetActive(false);
                grayscaleEndFilterFields.SetActive(false);
                chromaticEndFilterFields.SetActive(false);
                negativeEndFilterFields.SetActive(false);
                glitchEndFilterFields.SetActive(false);
                scanLinesEndFilterFields.SetActive(false);
                hueShiftEndFilterFields.SetActive(true);
                TMP_InputField shiftOffsetInputField = hueShiftEndFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                shiftOffsetInputField.text = shiftOffsetEnd.ToString();
                }
                break;
        }
        TMP_InputField endAtInputField = filterEventFields.transform.Find("EndAtInput").gameObject.GetComponent<TMP_InputField>();
        endAtInputField.text = endAt.ToString();
    }
    
    public void OnTransitionDropDownValueChanged(TMP_Dropdown dropdown) {
        switch (filter_type) {
            case "grayscale":
                if (dropdown.captionText.text == "Start") {
                    grayscaleFilterFields.SetActive(true);
                    grayscaleEndFilterFields.SetActive(false);
                    TMP_InputField hueInputField = grayscaleFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                    hueInputField.text = hue.ToString();
                    TMP_InputField saturationInputField = grayscaleFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                    saturationInputField.text = saturation.ToString();
                    TMP_InputField valueInputField = grayscaleFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                    valueInputField.text = value.ToString();
                } else {
                    grayscaleFilterFields.SetActive(false);
                    grayscaleEndFilterFields.SetActive(true);
                    TMP_InputField hueInputField = grayscaleEndFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                    hueInputField.text = hueEnd.ToString();
                    TMP_InputField saturationInputField = grayscaleEndFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                    saturationInputField.text = saturationEnd.ToString();
                    TMP_InputField valueInputField = grayscaleEndFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                    valueInputField.text = valueEnd.ToString();
                }
                break;
            case "chromatic":
                if (dropdown.captionText.text == "Start") {
                    chromaticFilterFields.SetActive(true);
                    chromaticEndFilterFields.SetActive(false);
                    TMP_InputField xOffsetInputField = chromaticFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                    xOffsetInputField.text = xOffset.ToString();
                    TMP_InputField yOffsetInputField = chromaticFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                    yOffsetInputField.text = yOffset.ToString();
                } else {
                    chromaticFilterFields.SetActive(false);
                    chromaticEndFilterFields.SetActive(true);
                    TMP_InputField xOffsetInputField = chromaticEndFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                    xOffsetInputField.text = xOffsetEnd.ToString();
                    TMP_InputField yOffsetInputField = chromaticEndFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                    yOffsetInputField.text = yOffsetEnd.ToString();
                }
                break;
            case "negative":
                if (dropdown.captionText.text == "Start") {
                    negativeFilterFields.SetActive(true);
                    negativeEndFilterFields.SetActive(false);
                    TMP_InputField intensityInputField = negativeFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                    intensityInputField.text = intensity.ToString();
                } else {
                    negativeFilterFields.SetActive(false);
                    negativeEndFilterFields.SetActive(true);
                    TMP_InputField intensityInputField = negativeEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                    intensityInputField.text = intensityEnd.ToString();
                }
                break;
            case "glitch":
                if (dropdown.captionText.text == "Start") {
                    glitchFilterFields.SetActive(true);
                    glitchEndFilterFields.SetActive(false);
                    TMP_InputField stabilityInputField = glitchFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                    stabilityInputField.text = stability.ToString();
                } else {
                    glitchFilterFields.SetActive(false);
                    glitchEndFilterFields.SetActive(true);
                    TMP_InputField stabilityInputField = glitchEndFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                    stabilityInputField.text = stabilityEnd.ToString();
                }
                break;
            case "scan_lines":
                if (dropdown.captionText.text == "Start") {
                    scanLinesFilterFields.SetActive(true);
                    scanLinesEndFilterFields.SetActive(false);
                    TMP_InputField intensity2InputField = scanLinesFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                    intensity2InputField.text = intensity.ToString();
                } else {
                    scanLinesFilterFields.SetActive(false);
                    scanLinesEndFilterFields.SetActive(true);
                    TMP_InputField intensity2InputField = scanLinesEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                    intensity2InputField.text = intensityEnd.ToString();
                }
                break;
            case "hue_shift":
                if (dropdown.captionText.text == "Start") {
                    hueShiftFilterFields.SetActive(true);
                    hueShiftEndFilterFields.SetActive(false);
                    TMP_InputField shiftOffsetInputField = hueShiftFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                    shiftOffsetInputField.text = shiftOffset.ToString();
                } else {
                    hueShiftFilterFields.SetActive(false);
                    hueShiftEndFilterFields.SetActive(true);
                    TMP_InputField shiftOffsetInputField = hueShiftEndFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                    shiftOffsetInputField.text = shiftOffsetEnd.ToString();
                }
                break;
        }
    }

    public void OnInputValueChanged(string input) {
        if (transform.position.z != this.z_position) return;
        Debug.Log(event_type + " " + transform.position.z);
        if (event_type == "theme_change") {
            themeIDInputField = themeEventFields.transform.GetChild(1).GetComponent<TMP_InputField>();
            themeID = int.Parse(themeIDInputField.text);
        } else if (event_type == "filter_change") {
            endAt = int.Parse(filterEventFields.transform.Find("EndAtInput").gameObject.GetComponent<TMP_InputField>().text);
            if (filter_type == "grayscale") {
                TMP_InputField hueInputField = grayscaleFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                hue = int.Parse(hueInputField.text);
                TMP_InputField saturationInputField = grayscaleFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                saturation = int.Parse(saturationInputField.text);
                TMP_InputField valueInputField = grayscaleFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                value = int.Parse(valueInputField.text);
                TMP_InputField hueEndInputField = grayscaleEndFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                hueEnd = int.Parse(hueEndInputField.text);
                TMP_InputField saturationEndInputField = grayscaleEndFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                saturationEnd = int.Parse(saturationEndInputField.text);
                TMP_InputField valueEndInputField = grayscaleEndFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                valueEnd = int.Parse(valueEndInputField.text);
            } else if (filter_type == "chromatic") {
                TMP_InputField xOffsetInputField = chromaticFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                xOffset = int.Parse(xOffsetInputField.text);
                TMP_InputField yOffsetInputField = chromaticFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                yOffset = int.Parse(yOffsetInputField.text);
                TMP_InputField xOffsetEndInputField = chromaticEndFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                xOffsetEnd = int.Parse(xOffsetEndInputField.text);
                TMP_InputField yOffsetEndInputField = chromaticEndFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                yOffsetEnd = int.Parse(yOffsetEndInputField.text);
            } else if (filter_type == "negative") {
                TMP_InputField intensityInputField = negativeFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensity = int.Parse(intensityInputField.text);
                TMP_InputField intensityEndInputField = negativeEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensityEnd = int.Parse(intensityEndInputField.text);
            } else if (filter_type == "glitch") {
                TMP_InputField stabilityInputField = glitchFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                stability = int.Parse(stabilityInputField.text);
                TMP_InputField stabilityEndInputField = glitchEndFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                stabilityEnd = int.Parse(stabilityEndInputField.text);
            } else if (filter_type == "scan_lines") {
                TMP_InputField intensityInputField = scanLinesFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensity = int.Parse(intensityInputField.text);
                TMP_InputField intensityEndInputField = scanLinesEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensityEnd = int.Parse(intensityEndInputField.text);
            } else if (filter_type == "hue_shift") {
                TMP_InputField shiftOffsetInputField = hueShiftFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                shiftOffset = int.Parse(shiftOffsetInputField.text);
                TMP_InputField shiftOffsetEndInputField = hueShiftEndFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                shiftOffsetEnd = int.Parse(shiftOffsetEndInputField.text);
            }
        } else if (event_type == "speed_change") {
            TMP_InputField multiplierInputField = speedEventFields.transform.Find("MultiplierInput").gameObject.GetComponent<TMP_InputField>();
            multiplier = float.Parse(multiplierInputField.text);
            TMP_InputField durationInputField = speedEventFields.transform.Find("DurationInput").gameObject.GetComponent<TMP_InputField>();
            duration = int.Parse(durationInputField.text);
        }
    }

    public void AddListenerToAllTextFields() {
        TMP_InputField[] inputFields = editThemePanel.GetComponentsInChildren<TMP_InputField>();
        foreach (TMP_InputField inputField in inputFields) {
            inputField.onValueChanged.AddListener((string s) => OnInputValueChanged(s));
        } 
    }

    public void CloseEditThemePanel()
    {
        if (transform.position.z == z_position) {
            z_position = 0f;
        }
        Debug.Log(event_type + " " + transform.position.z);
        if (event_type == "theme_change") {
            themeID = int.Parse(themeIDInputField.text);
        } else if (event_type == "filter_change") {
            endAt = int.Parse(filterEventFields.transform.Find("EndAtInput").gameObject.GetComponent<TMP_InputField>().text);
            if (filter_type == "grayscale") {
                TMP_InputField hueInputField = grayscaleFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                hue = int.Parse(hueInputField.text);
                TMP_InputField saturationInputField = grayscaleFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                saturation = int.Parse(saturationInputField.text);
                TMP_InputField valueInputField = grayscaleFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                value = int.Parse(valueInputField.text);
                TMP_InputField hueEndInputField = grayscaleEndFilterFields.transform.Find("HueInput").gameObject.GetComponent<TMP_InputField>();
                hueEnd = int.Parse(hueEndInputField.text);
                TMP_InputField saturationEndInputField = grayscaleEndFilterFields.transform.Find("SaturationInput").gameObject.GetComponent<TMP_InputField>();
                saturationEnd = int.Parse(saturationEndInputField.text);
                TMP_InputField valueEndInputField = grayscaleEndFilterFields.transform.Find("ValueInput").gameObject.GetComponent<TMP_InputField>();
                valueEnd = int.Parse(valueEndInputField.text);
            } else if (filter_type == "chromatic") {
                TMP_InputField xOffsetInputField = chromaticFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                xOffset = int.Parse(xOffsetInputField.text);
                TMP_InputField yOffsetInputField = chromaticFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                yOffset = int.Parse(yOffsetInputField.text);
                TMP_InputField xOffsetEndInputField = chromaticEndFilterFields.transform.Find("XOffsetInput").gameObject.GetComponent<TMP_InputField>();
                xOffsetEnd = int.Parse(xOffsetEndInputField.text);
                TMP_InputField yOffsetEndInputField = chromaticEndFilterFields.transform.Find("YOffsetInput").gameObject.GetComponent<TMP_InputField>();
                yOffsetEnd = int.Parse(yOffsetEndInputField.text);
            } else if (filter_type == "negative") {
                TMP_InputField intensityInputField = negativeFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensity = int.Parse(intensityInputField.text);
                TMP_InputField intensityEndInputField = negativeEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensityEnd = int.Parse(intensityEndInputField.text);
            } else if (filter_type == "glitch") {
                TMP_InputField stabilityInputField = glitchFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                stability = int.Parse(stabilityInputField.text);
                TMP_InputField stabilityEndInputField = glitchEndFilterFields.transform.Find("StabilityInput").gameObject.GetComponent<TMP_InputField>();
                stabilityEnd = int.Parse(stabilityEndInputField.text);
            } else if (filter_type == "scan_lines") {
                TMP_InputField intensityInputField = scanLinesFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensity = int.Parse(intensityInputField.text);
                TMP_InputField intensityEndInputField = scanLinesEndFilterFields.transform.Find("IntensityInput").gameObject.GetComponent<TMP_InputField>();
                intensityEnd = int.Parse(intensityEndInputField.text);
            } else if (filter_type == "hue_shift") {
                TMP_InputField shiftOffsetInputField = hueShiftFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                shiftOffset = int.Parse(shiftOffsetInputField.text);
                TMP_InputField shiftOffsetEndInputField = hueShiftEndFilterFields.transform.Find("ShiftOffsetInput").gameObject.GetComponent<TMP_InputField>();
                shiftOffsetEnd = int.Parse(shiftOffsetEndInputField.text);
            }
        } else if (event_type == "speed_change") {
            TMP_InputField multiplierInputField = speedEventFields.transform.Find("MultiplierInput").gameObject.GetComponent<TMP_InputField>();
            multiplier = float.Parse(multiplierInputField.text);
            TMP_InputField durationInputField = speedEventFields.transform.Find("DurationInput").gameObject.GetComponent<TMP_InputField>();
            duration = int.Parse(durationInputField.text);
        }
        editThemePanel.SetActive(false);
        LevelEditor le = GameObject.Find("Balus").GetComponent<LevelEditor>();
        le.SetPopupOpen(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
