using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class LevelConfigJson
{
    public string level_name;
    public string level_author;
    public float level_speed;
    public int start_pos;
    public string music_path;
    public string worldshow_path;
    public bool start_portal;
}

[System.Serializable]
public class GeoBufferJson
{
    public List<int> ground;
    public List<int> enemies;
}

public class LevelConfigurator : MonoBehaviour
{
    public string jsonFilePath;
    public string jsonString;
    public string levelName;
    public string levelAuthor;
    public float levelSpeed;
    public int startPos;
    public string musicPath;
    public string worldshowPath;
    public bool startPortal;

    [Header("References")]
    public GameObject balus;
    public GameObject levelConfigPanel;
    [Header("Start Portal Object")]
    public GameObject startPortalObject2;

    private Rigidbody rb;
    private GameManager gameManager;

    // UI References (Cached to avoid Find)
    private GameObject levelNameObject;
    private GameObject levelAuthorObject;
    private GameObject levelSpeedObject;
    private GameObject levelSpeedObject2;
    private GameObject startPosObject;
    private GameObject musicPathObject;
    private GameObject startPortalObject;

    TMP_InputField levelNameInput;
    TMP_InputField levelAuthorInput;
    public TMP_InputField levelSpeedInput;
    TMP_InputField startPosInput;
    TMP_InputField musicPathInput;
    Slider levelSpeedSlider;
    Toggle startPortalToggle;

    public List<string> levelFilePaths = new List<string>();
    private LevelEditor levelEditor;

    void Start()
    {
        // 1. Setup Balus & LevelEditor
        if (balus == null) balus = GameObject.FindGameObjectWithTag("Balus");
        if (balus == null) balus = GameObject.Find("Balus"); // Fallback

        if (balus != null)
        {
            rb = balus.GetComponent<Rigidbody>();
            levelEditor = balus.GetComponent<LevelEditor>();
        }
        else
        {
            Debug.LogError("LevelConfigurator: Could not find 'Balus'!");
        }

        // 2. Setup GameManager
        if (gameManager == null)
        {
            GameObject gm = GameObject.Find("GameManager");
            if (gm) gameManager = gm.GetComponent<GameManager>();
        }

        // 3. Load Config Data
        string persistentPath = Path.Combine(Application.persistentDataPath, jsonFilePath + ".json");
        if (File.Exists(persistentPath))
        {
            jsonString = File.ReadAllText(persistentPath);
        }
        else
        {
            TextAsset asset = Resources.Load<TextAsset>(jsonFilePath);
            if (asset != null) jsonString = asset.text;
            else
            {
                Debug.LogError($"[Config] File not found: {jsonFilePath}");
                return;
            }
        }

        LevelConfigJson config = JsonConvert.DeserializeObject<LevelConfigJson>(jsonString);
        if (config != null)
        {
            levelName = config.level_name;
            levelAuthor = config.level_author;
            levelSpeed = config.level_speed;
            startPos = config.start_pos;
            musicPath = config.music_path;
            worldshowPath = config.worldshow_path;
            startPortal = config.start_portal;
        }

    }

    public void LoadLevelConfig()
    {
        if (gameManager == null)
        {
            GameObject gmObj = GameObject.Find("GameManager");
            if (gmObj != null) gameManager = gmObj.GetComponent<GameManager>();
        }

        // Load File Logic
        if (gameManager != null && gameManager.isDataDownloaded)
        {
            string persistentPath = Path.Combine(Application.persistentDataPath, jsonFilePath + ".json");
            if (File.Exists(persistentPath))
            {
                jsonString = File.ReadAllText(persistentPath);
            }
            else
            {
                // Fallback
                TextAsset asset = Resources.Load<TextAsset>(jsonFilePath);
                if (asset != null) jsonString = asset.text;
                else
                {
                    Debug.LogError($"Config not found: {jsonFilePath}");
                    return;
                }
            }
        }
        else
        {
            TextAsset asset = Resources.Load<TextAsset>(jsonFilePath);
            if (asset != null) jsonString = asset.text;
            else return;
        }

        LevelConfigJson config = JsonConvert.DeserializeObject<LevelConfigJson>(jsonString);
        if (config == null) return;

        levelName = config.level_name;
        levelAuthor = config.level_author;
        levelSpeed = config.level_speed;
        startPos = config.start_pos;
        musicPath = config.music_path;
        worldshowPath = config.worldshow_path;
        startPortal = config.start_portal;

        // Find Start Portal if needed
        if (startPortalObject2 == null)
        {
            startPortalObject2 = GameObject.Find("DeceBalus_Pod_Start");
        }

        // Apply Portal State
        if (startPortalObject2 != null)
        {
            if (startPortal)
            {
                startPortalObject2.SetActive(true);
                startPortalObject2.transform.position = new Vector3(0f, 0f, startPos);
            }
            else
            {
                startPortalObject2.SetActive(false);
                startPortalObject2.transform.position = new Vector3(0f, 0f, startPos);
            }
        }
    }

    public void OnInputValueChanged(TMP_InputField inputField)
    {
        if (levelSpeedSlider != null)
            levelSpeedSlider.value = float.Parse(inputField.text);
    }

    public void ShowConfigPanel()
    {
        levelConfigPanel.SetActive(true);

        // Find UI Elements (Only do this once ideally, but kept here for compatibility)
        if (!levelNameObject) levelNameObject = GameObject.Find("LevelNameInput");
        if (levelNameObject)
        {
            levelNameInput = levelNameObject.GetComponent<TMP_InputField>();
            levelNameInput.text = levelName;
        }

        if (!levelAuthorObject) levelAuthorObject = GameObject.Find("LevelAuthorInput");
        if (levelAuthorObject)
        {
            levelAuthorInput = levelAuthorObject.GetComponent<TMP_InputField>();
            levelAuthorInput.text = levelAuthor;
        }

        if (!levelSpeedObject2) levelSpeedObject2 = GameObject.Find("LevelSpeedInput");
        if (levelSpeedObject2)
        {
            levelSpeedInput = levelSpeedObject2.GetComponent<TMP_InputField>();
            levelSpeedInput.text = levelSpeed.ToString();
            levelSpeedInput.onValueChanged.RemoveAllListeners();
            levelSpeedInput.onValueChanged.AddListener(delegate { OnInputValueChanged(levelSpeedInput); });
        }

        if (!levelSpeedObject) levelSpeedObject = GameObject.Find("LevelSpeedSlider");
        if (levelSpeedObject)
        {
            levelSpeedSlider = levelSpeedObject.GetComponent<Slider>();
            levelSpeedSlider.value = levelSpeed;
        }

        if (!startPosObject) startPosObject = GameObject.Find("StartPosInput");
        if (startPosObject)
        {
            startPosInput = startPosObject.GetComponent<TMP_InputField>();
            startPosInput.text = startPos.ToString();
        }

        if (!musicPathObject) musicPathObject = GameObject.Find("MusicPathInput");
        if (musicPathObject)
        {
            musicPathInput = musicPathObject.GetComponent<TMP_InputField>();
            musicPathInput.text = musicPath;
        }

        if (!startPortalObject) startPortalObject = GameObject.Find("StartPortalToggle");
        if (startPortalObject)
        {
            startPortalToggle = startPortalObject.GetComponent<Toggle>();
            startPortalToggle.isOn = startPortal;
        }

        // Use cached LevelEditor
        if (levelEditor != null)
        {
            levelEditor.SetPopupOpen(true);
        }
    }

    public void CloseConfigPanel()
    {
        // Get values from UI if inputs exist
        if (levelNameInput) levelName = levelNameInput.text;
        if (levelAuthorInput) levelAuthor = levelAuthorInput.text;
        if (levelSpeedSlider) levelSpeed = levelSpeedSlider.value;
        if (startPosInput) int.TryParse(startPosInput.text, out startPos);
        if (musicPathInput) musicPath = musicPathInput.text;

        // Audio Update
        if (balus != null)
        {
            AudioPlayer ap = balus.GetComponent<AudioPlayer>();
            if (ap != null)
            {
                ap.audioPath = musicPath;
                ap.UpdateAudioClip(); // Make sure AudioPlayer script has this public method
            }
        }

        // Portal Update
        if (startPortalToggle) startPortal = startPortalToggle.isOn;

        if (startPortalObject2 != null)
        {
            if (startPortal)
            {
                startPortalObject2.SetActive(true);
                startPortalObject2.transform.position = new Vector3(0f, 0f, startPos);
            }
            else
            {
                startPortalObject2.SetActive(false);
                startPortalObject2.transform.position = new Vector3(0f, 0f, startPos);
            }
        }

        // Update Popup State
        if (levelEditor != null)
        {
            levelEditor.SetPopupOpen(false);
        }
        else
        {
            // Last resort finding logic
            GameObject b = GameObject.Find("Balus");
            if (b != null)
            {
                LevelEditor le = b.GetComponent<LevelEditor>();
                if (le != null) le.SetPopupOpen(false);
            }
        }

        levelConfigPanel.SetActive(false);
    }

    public LevelConfigJson SaveConfig()
    {
        LevelConfigJson config = new LevelConfigJson();
        config.level_name = levelName;
        config.level_author = levelAuthor;
        config.level_speed = levelSpeed;
        config.start_pos = startPos;
        config.music_path = musicPath;
        config.start_portal = startPortal;
        return config;
    }
}