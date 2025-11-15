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
    public GameObject startPortalObject2;
    public List<string> levelFilePaths = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        //balus = GameObject.FindGameObjectWithTag("Balus");
        rb = balus.GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager.isDataDownloaded) {
            levelFilePaths.Add(Application.persistentDataPath);
            string correctFilePath = levelFilePaths.FirstOrDefault(x => File.Exists(x + "/" + jsonFilePath + ".json"));
            if (correctFilePath == null) {
                Debug.LogError("File not found");
                return;
            }
            jsonString = File.ReadAllText(correctFilePath + "/" + jsonFilePath + ".json");
        } else {
            jsonString = Resources.Load<TextAsset>(jsonFilePath).text;
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
        startPortalObject2.SetActive(false);
        //rb.position = new Vector3(balus.transform.position.x, balus.transform.position.y, startPos);
    }

    public void LoadLevelConfig() {
        if (gameManager.isDataDownloaded) {
            string correctFilePath = levelFilePaths.FirstOrDefault(x => File.Exists(x + "/" + jsonFilePath + ".json"));
            if (correctFilePath == null) {
                Debug.LogError("File not found");
                return;
            }
            jsonString = File.ReadAllText(correctFilePath + "/" + jsonFilePath + ".json");
        } else {
            jsonString = Resources.Load<TextAsset>(jsonFilePath).text;
        }
        LevelConfigJson config = JsonConvert.DeserializeObject<LevelConfigJson>(jsonString);
        levelName = config.level_name;
        levelAuthor = config.level_author;
        levelSpeed = config.level_speed;
        startPos = config.start_pos;
        musicPath = config.music_path;
        worldshowPath = config.worldshow_path;
        startPortal = config.start_portal;
        if (startPortal) {
            startPortalObject2.SetActive(true);
            startPortalObject2.transform.position = new Vector3(0f, 0f, startPos);
        } else {
            startPortalObject2.SetActive(false);
            startPortalObject2.transform.position = new Vector3(0f, 0f, startPos);
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
        le.SetPopupOpen(true);
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
