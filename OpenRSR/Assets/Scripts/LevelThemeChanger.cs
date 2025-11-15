using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelThemeChanger : MonoBehaviour
{
    public GameObject ball;
    public ThemeChanger themeChanger;
    public string jsonFilePath;
    public int themeID = 0;
    private List<BaseEvent> levelEvents = new List<BaseEvent>();
    private List<float> themeZPositions;
    private List<int> themeIds;
    private GameManager gameManager;
    private string jsonString;
    public float normalSpeed;
    private int normalDuration;
    public List<string> levelFilePaths = new List<string>();
    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Balus");
        themeChanger = GetComponent<ThemeChanger>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gameManager.isDataDownloaded)
        {
            levelFilePaths.Add(Application.persistentDataPath);
            string correctFilePath = levelFilePaths.FirstOrDefault(x => File.Exists(x + "/" + jsonFilePath + ".json"));
            if (correctFilePath == null) {
                Debug.LogError("File not found");
                return;
            }
            jsonString = File.ReadAllText(correctFilePath + "/" + jsonFilePath + ".json");
        }
        else 
        {
            jsonString = Resources.Load<TextAsset>(jsonFilePath).text;
        }
        
        LevelEventData jsonData = JsonConvert.DeserializeObject<LevelEventData>(jsonString);
        levelEvents = jsonData.level_events;

        // Set initial theme ID for ThemeChanger script
        foreach (BaseEvent level_event in levelEvents) {
            if (level_event.event_type == "theme_change") {
                themeChanger.themeID = Convert.ToInt32(level_event.event_fields["theme_id"]);
                break;
            }
        }
    }

    private void Update()
    {
        // Get current ball position
        float ballZPos = ball.transform.position.z;
        //Debug.Log(ballZPos + " " + levelEvents[themeID].z_position);
        //Debug.Log(ballZPos >= levelEvents[themeID].z_position);
        if (normalSpeed == 0f) {
            SphereMovement sphereMovement = ball.GetComponent<SphereMovement>();
            normalSpeed = sphereMovement.speed;
        }

        // Check if ball has reached the next theme Z position
        if (themeID < levelEvents.Count && ballZPos >= levelEvents[themeID].z_position)
        {
            //Debug.Log(levelEvents[themeID].event_fields["theme_id"].GetType());
            if (levelEvents[themeID].event_type == "theme_change") {
                themeChanger.UpdateTheme(Convert.ToInt32(levelEvents[themeID].event_fields["theme_id"]));
            } else if (levelEvents[themeID].event_type == "filter_change") {
                if (FilterManager.filterManager != null) {
                    if (levelEvents[themeID].event_fields["filter_type"].ToString() == "grayscale") {
                        Color startColor = Color.HSVToRGB(
                            (float)Convert.ToInt32(levelEvents[themeID].event_fields["hue"]) / 100f, 
                            (float)Convert.ToInt32(levelEvents[themeID].event_fields["saturation"]) / 100f, 
                            (float)Convert.ToInt32(levelEvents[themeID].event_fields["value"]) / 100f
                        );
                        Color endColor = Color.HSVToRGB(
                            (float)Convert.ToInt32(levelEvents[themeID].event_fields["hue_end"]) / 100f, 
                            (float)Convert.ToInt32(levelEvents[themeID].event_fields["saturation_end"]) / 100f, 
                            (float)Convert.ToInt32(levelEvents[themeID].event_fields["value_end"]) / 100f
                        );
                        string grayScaleString = $"{levelEvents[themeID].z_position + (float)Convert.ToInt32(levelEvents[themeID].event_fields["end"])},0/{startColor.r.ToString()},{startColor.g.ToString()},{startColor.b.ToString()},{(float)Convert.ToInt32(levelEvents[themeID].event_fields["value"]) / 100f}/{endColor.r.ToString()},{endColor.g.ToString()},{endColor.b.ToString()},{(float)Convert.ToInt32(levelEvents[themeID].event_fields["value_end"]) / 100f}";
                        Debug.Log(grayScaleString);
                        FilterManager.filterManager.SetEffect(levelEvents[themeID].z_position, new string[] { grayScaleString });
                    } else if (levelEvents[themeID].event_fields["filter_type"].ToString() == "chromatic") {
                        float sChromaticAberrationXOffset = (float)Convert.ToInt32(levelEvents[themeID].event_fields["x_offset"]) / 10000f;
                        float sChromaticAberrationYOffset = (float)Convert.ToInt32(levelEvents[themeID].event_fields["y_offset"]) / 10000f;
                        float eChromaticAberrationXOffset = (float)Convert.ToInt32(levelEvents[themeID].event_fields["x_offset_end"]) / 10000f;
                        float eChromaticAberrationYOffset = (float)Convert.ToInt32(levelEvents[themeID].event_fields["y_offset_end"]) / 10000f;
                        string chromaticString = $"{levelEvents[themeID].z_position + (float)Convert.ToInt32(levelEvents[themeID].event_fields["end"])},1/{sChromaticAberrationXOffset},{sChromaticAberrationYOffset}/{eChromaticAberrationXOffset},{eChromaticAberrationYOffset}";
                        FilterManager.filterManager.SetEffect(levelEvents[themeID].z_position, new string[] { chromaticString });
                    } else if (levelEvents[themeID].event_fields["filter_type"].ToString() == "negative") {
                        float sNegativeIntensity = (float)Convert.ToInt32(levelEvents[themeID].event_fields["intensity"]) / 100f;
                        float eNegativeIntensity = (float)Convert.ToInt32(levelEvents[themeID].event_fields["intensity_end"]) / 100f;
                        string negativeString = $"{levelEvents[themeID].z_position + (float)Convert.ToInt32(levelEvents[themeID].event_fields["end"])},2/{sNegativeIntensity}/{eNegativeIntensity}";
                        FilterManager.filterManager.SetEffect(levelEvents[themeID].z_position, new string[] { negativeString });
                    } else if (levelEvents[themeID].event_fields["filter_type"].ToString() == "glitch") {
                        float sArtifactsIntensity = (float)Convert.ToInt32(levelEvents[themeID].event_fields["stability"]) / 10f;
                        float eArtifactsIntensity = (float)Convert.ToInt32(levelEvents[themeID].event_fields["stability_end"]) / 10f;
                        string glitchString = $"{levelEvents[themeID].z_position + (float)Convert.ToInt32(levelEvents[themeID].event_fields["end"])},3/{sArtifactsIntensity}/{eArtifactsIntensity}";
                        FilterManager.filterManager.SetEffect(levelEvents[themeID].z_position, new string[] { glitchString });
                    } else if (levelEvents[themeID].event_fields["filter_type"].ToString() == "scan_lines") {
                        float sScanLinesIntensity = (float)Convert.ToInt32(levelEvents[themeID].event_fields["intensity"]) / 100f;
                        float eScanLinesIntensity = (float)Convert.ToInt32(levelEvents[themeID].event_fields["intensity_end"]) / 100f;
                        string scanLinesString = $"{levelEvents[themeID].z_position + (float)Convert.ToInt32(levelEvents[themeID].event_fields["end"])},4/{sScanLinesIntensity}/{eScanLinesIntensity}";
                        FilterManager.filterManager.SetEffect(levelEvents[themeID].z_position, new string[] { scanLinesString });
                    } else if (levelEvents[themeID].event_fields["filter_type"].ToString() == "hue_shift") {
                        float sHueShift = (float)Convert.ToInt32(levelEvents[themeID].event_fields["hue_shift"]) / 360f;
                        float eHueShift = (float)Convert.ToInt32(levelEvents[themeID].event_fields["hue_shift_end"]) / 360f;
                        string hueShiftString = $"{levelEvents[themeID].z_position + (float)Convert.ToInt32(levelEvents[themeID].event_fields["end"])},5/{sHueShift}/{eHueShift}";
                        FilterManager.filterManager.SetEffect(levelEvents[themeID].z_position, new string[] { hueShiftString });
                    }
                }
            } else if (levelEvents[themeID].event_type == "speed_change") {
                Debug.Log(Convert.ToSingle(levelEvents[themeID].event_fields["multiplier"]));
                SphereMovement sphm = GameObject.Find("Balus").GetComponent<SphereMovement>();
                normalSpeed = sphm.speed;
                normalDuration = ((int)levelEvents[themeID].z_position + Convert.ToInt32(levelEvents[themeID].event_fields["duration"]));
                sphm.speed *= Convert.ToSingle(levelEvents[themeID].event_fields["multiplier"]);
            }
            themeID++;
        }

        if (ballZPos >= normalDuration) {
            SphereMovement sphm = GameObject.Find("Balus").GetComponent<SphereMovement>();
            sphm.speed = normalSpeed;
        }
    }
    
    public void UpdateData() {
        //Debug.Log(jsonString);
        string correctFilePath = levelFilePaths.FirstOrDefault(x => File.Exists(x + "/" + jsonFilePath + ".json"));
        if (correctFilePath == null) {
            Debug.LogError("File not found");
            return;
        }
        jsonString = File.ReadAllText(correctFilePath + "/" + jsonFilePath + ".json");
        //Debug.Log(jsonString);

        LevelEventData jsonData = JsonConvert.DeserializeObject<LevelEventData>(jsonString);
        levelEvents = jsonData.level_events;

        // Set initial theme ID for ThemeChanger script
        foreach (BaseEvent level_event in levelEvents) {
            if (level_event.event_type == "theme_change") {
                themeChanger.themeID = Convert.ToInt32(level_event.event_fields["theme_id"]);
                break;
            }
        }
    }

    public LevelEventData GetData() {
        string correctFilePath = levelFilePaths.FirstOrDefault(x => File.Exists(x + "/" + jsonFilePath + ".json"));
        if (correctFilePath == null) {
            Debug.LogError("File not found");
            return null;
        }
        jsonString = File.ReadAllText(correctFilePath + "/" + jsonFilePath + ".json");
        return JsonConvert.DeserializeObject<LevelEventData>(jsonString);
    }
}

[System.Serializable]
public class LevelThemeData
{
    public List<float> themeZPositions;
    public List<int> themeIds;
}

[System.Serializable]
public class LevelEventData
{
    public List<BaseEvent> level_events = new List<BaseEvent>();
}

[System.Serializable]
public class BaseEvent
{
    public float z_position;
    public string event_type;
    public Dictionary<string, object> event_fields = new Dictionary<string, object>();
}