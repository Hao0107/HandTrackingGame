using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class LevelConfigJson {
    public string level_name;
    public string level_author;
    public float level_speed;
    public int start_pos;
    public string music_path;
    public string worldshow_path;
    public bool start_portal;
}

[System.Serializable]
public class GeoBufferJson {
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
    public GameObject balus;
    private Rigidbody rb;
    private GameManager gameManager;
    public GameObject levelConfigPanel;
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
    [Header("Start Portal Object")]
    public GameObject startPortalObject2;
    public List<string> levelFilePaths = new List<string>();

    private LevelEditor levelEditor;
    // Start is called before the first frame update
    void Start()
    {
        //balus = GameObject.FindGameObjectWithTag("Balus");
        rb = balus.GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        levelName = config.level_name;
        levelAuthor = config.level_author;
        levelSpeed = config.level_speed;
        startPos = config.start_pos;
        musicPath = config.music_path;
        worldshowPath = config.worldshow_path;
        startPortal = config.start_portal;
        startPortalObject2 = GameObject.Find("DeceBalus_Pod_Start");
        //startPortalObject2.SetActive(false);
        //rb.position = new Vector3(balus.transform.position.x, balus.transform.position.y, startPos);

        GameObject balusObj = GameObject.Find("Balus");
        if (balusObj != null)
        {
            levelEditor = balusObj.GetComponent<LevelEditor>();
        }
        else
        {
            Debug.LogError("Could not find GameObject named 'Balus'");
        }
    }

    public void LoadLevelConfig()
    {
        if (gameManager == null)
        {
            GameObject gmObj = GameObject.Find("GameManager");
            if (gmObj != null) gameManager = gmObj.GetComponent<GameManager>();
        }

        if (gameManager != null && gameManager.isDataDownloaded)
        {
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

        if (startPortalObject2 == null)
        {
            startPortalObject2 = GameObject.Find("DeceBalus_Pod_Start");
        }

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
        else
        {
            Debug.LogWarning("Can't find 'DeceBalus_Pod_Start' in Scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInputValueChanged(TMP_InputField inputField) {
        levelSpeedSlider.value = float.Parse(inputField.text);
    }

    public void ShowConfigPanel() {
        levelConfigPanel.SetActive(true);
        levelNameObject = GameObject.Find("LevelNameInput");
        levelNameInput = levelNameObject.GetComponent<TMP_InputField>();
        levelNameInput.text = levelName;
        levelAuthorObject = GameObject.Find("LevelAuthorInput");
        levelAuthorInput = levelAuthorObject.GetComponent<TMP_InputField>();
        levelAuthorInput.text = levelAuthor;
        levelSpeedObject2 = GameObject.Find("LevelSpeedInput");
        levelSpeedInput = levelSpeedObject2.GetComponent<TMP_InputField>();
        levelSpeedInput.text = levelSpeed.ToString();
        levelSpeedInput.onValueChanged.AddListener(delegate { OnInputValueChanged(levelSpeedInput); });
        levelSpeedObject = GameObject.Find("LevelSpeedSlider");
        levelSpeedSlider = levelSpeedObject.GetComponent<Slider>();
        levelSpeedSlider.value = levelSpeed;
        startPosObject = GameObject.Find("StartPosInput");
        startPosInput = startPosObject.GetComponent<TMP_InputField>();
        startPosInput.text = startPos.ToString();
        musicPathObject = GameObject.Find("MusicPathInput");
        musicPathInput = musicPathObject.GetComponent<TMP_InputField>();
        musicPathInput.text = musicPath;
        startPortalObject = GameObject.Find("StartPortalToggle");
        startPortalToggle = startPortalObject.GetComponent<Toggle>();
        startPortalToggle.isOn = startPortal;
        LevelEditor le = GameObject.Find("Balus").GetComponent<LevelEditor>();
        if(levelEditor != null) {
            levelEditor.SetPopupOpen(true);
        }
    }

    public void CloseConfigPanel() {
        levelName = levelNameInput.text;
        levelAuthor = levelAuthorInput.text;
        levelSpeed = levelSpeedSlider.value;
        startPos = int.Parse(startPosInput.text);
        musicPath = musicPathInput.text;
        AudioPlayer ap = balus.GetComponent<AudioPlayer>();
        ap.audioPath = musicPath;
        ap.LoadAudioClip();
        startPortal = startPortalToggle.isOn;
        if (startPortal) {
            startPortalObject2.SetActive(true);
            startPortalObject2.transform.position = new Vector3(0f, 0f, startPos);
        } else {
            startPortalObject2.SetActive(false);
            startPortalObject2.transform.position = new Vector3(0f, 0f, startPos);
        }

        if (levelEditor != null)
        {
            levelEditor.SetPopupOpen(false);
        }
        else
        {
            GameObject balusObj = GameObject.Find("Balus");
            if (balusObj != null)
            {
                levelEditor = balusObj.GetComponent<LevelEditor>();
                if (levelEditor != null)
                {
                    levelEditor.SetPopupOpen(false);
                }
            }

            if (levelEditor == null)
                Debug.LogWarning("LevelEditor component not found on 'Balus' object, popup state not updated.");
        }

        levelConfigPanel.SetActive(false);
        LevelEditor le = GameObject.Find("Balus").GetComponent<LevelEditor>();
        le.SetPopupOpen(false);
    }

    public LevelConfigJson SaveConfig() {
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
