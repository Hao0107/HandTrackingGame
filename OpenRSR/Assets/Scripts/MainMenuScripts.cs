using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Globalization;
using UnityEngine.UI;
using TMPro;
using SimpleFileBrowser;
using UnityEngine.InputSystem;

[System.Serializable]
public enum SortBy {
    difficulty_min,
    difficulty_max,
    difficulty_avg
}

[System.Serializable]
public enum SortOrder {
    ascending,
    descending
}

[System.Serializable]
public enum AvgType {
    // minimum
    min,
    // akin to the generalized mean with p=-10
    softmin,
    // harmonic mean
    harmonic,
    // geometric mean
    geometric,
    // arithmetic mean
    arithmetic,
    // quadratic mean
    quadratic,
    // akin to the generalized mean with p=10
    softmax,
    // maximum
    max
}

[System.Serializable]
public class MenuLevelJson {
    public string display_title;
    public int display_gems;
    public int display_crowns;
    public bool display_author;
    public bool display_progress;
    public int display_background;
    public float level_difficulty_easy;
    public float level_difficulty_hard;
    public string level_id;
    public List<string> level_variations = new List<string>();
}

[System.Serializable]
public class MenuLevelJsonWithVariations {
    public Dictionary<string, string> display_title = new Dictionary<string, string>();
    public Dictionary<string, int> display_gems = new Dictionary<string, int>();
    public Dictionary<string, int> display_crowns = new Dictionary<string, int>();
    public Dictionary<string, bool> display_author = new Dictionary<string, bool>();
    public Dictionary<string, bool> display_progress = new Dictionary<string, bool>();
    public Dictionary<string, int> display_background = new Dictionary<string, int>();
    public Dictionary<string, float> level_difficulty_easy = new Dictionary<string, float>();
    public Dictionary<string, float> level_difficulty_hard = new Dictionary<string, float>();
    public string level_id;
    public List<string> level_variations = new List<string>();
    public string default_variation = "default";
}

[System.Serializable]
public class MenuDataJson {
    [JsonConverter(typeof(StringEnumConverter))]
    public SortBy sortBy = SortBy.difficulty_avg;
    [JsonConverter(typeof(StringEnumConverter))]
    public SortOrder sortOrder = SortOrder.ascending;
    [JsonConverter(typeof(StringEnumConverter))]
    public AvgType avgType = AvgType.arithmetic;
    public List<MenuLevelJsonWithVariations> level_list = new List<MenuLevelJsonWithVariations>();
}

[System.Serializable]
public class AppendMenuDataJson {
    public List<MenuLevelJsonWithVariations> level_list = new List<MenuLevelJsonWithVariations>();
}

public class MainMenuScripts : MonoBehaviour
{
    private GameManager gameManager;
    private bool isMovingCamera = false;
    private float t = 0f;
    private GameObject mainMenuCanvas;
    private GameObject level1CoverObject;
    private GameObject level1TitleObject;
    public MenuDataJson menuData = new MenuDataJson();
    private string jsonString;
    private GameObject titleText;
    private bool isInTitleScreen = true;
    private GameObject levelUIComponent;
    private GameObject levelItem;
    private GameObject leftButton;
    private GameObject rightButton;
    private GameObject createLevelPanel;
    private TMP_InputField levelNameInputField;
    private TMP_Dropdown modPathDropdown;
    private GameObject createLevelButton;
    public int menuIdx = 0;
    public string currentLevelId = "";
    public string currentVariation = "default";
    public int currentVariationIdx = 0;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private int levelCount = 1;
    public static MainMenuScripts instance;
    public bool isInitialized = false;
    private ScriptLoader scriptLoader;
    // Start is called before the first frame update
    void Awake() {
        instance = this;
    }
    void Start()
    {
        
    }

    public void Initialize() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //instance = this;
        mainMenuCanvas = GameObject.Find("MainMenu");
        leftButton = mainMenuCanvas.transform.Find("LeftButton").gameObject;
        rightButton = mainMenuCanvas.transform.Find("RightButton").gameObject;
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        GameObject dialogPopUpCanvas = GameObject.Find("DialogPopUpCanvas");
        createLevelPanel = dialogPopUpCanvas.transform.Find("NewLevelPanel").gameObject;
        createLevelPanel.SetActive(false);
        levelNameInputField = createLevelPanel.transform.Find("LevelIDInput").gameObject.GetComponent<TMP_InputField>();
        modPathDropdown = createLevelPanel.transform.Find("ModPathDropdown").gameObject.GetComponent<TMP_Dropdown>();
        createLevelButton = createLevelPanel.transform.Find("CreateButton").gameObject;
        titleText = GameObject.Find("TitleImage");
        levelUIComponent = GameObject.Find("LevelUI");
        levelItem = levelUIComponent.transform.Find("LevelItem").gameObject;
        menuData = JsonConvert.DeserializeObject<MenuDataJson>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "MenuData/MenuData.json")));
        scriptLoader = GameObject.Find("ModManager").GetComponent<ScriptLoader>();
        scriptLoader.AppendMenuData();
        CreateLevelObjects();
        foreach (MenuLevelJsonWithVariations level in menuData.level_list) {
            GameObject levelCoverObject = GameObject.Find($"Level_{level.level_id}_Cover_Button");
            levelCoverObject.GetComponent<Button>().onClick.AddListener(() => {
                gameManager.LoadLevel(level.level_id);
                mainMenuCanvas.SetActive(false);
            });
            GameObject levelTitleObject = GameObject.Find($"Level_{level.level_id}_Title");
            levelTitleObject.GetComponent<TextMeshProUGUI>().text = level.display_title[level.default_variation];
            GameObject levelCoverObjectImage = GameObject.Find($"Level_{level.level_id}_Cover");
            try {
                byte[] levelCoverBytes = File.ReadAllBytes(Path.Combine(Application.persistentDataPath, $"WorldShow/World_{level.level_id}.png"));
                Texture2D levelCoverTexture = new Texture2D(1080, 1920);
                levelCoverTexture.LoadImage(levelCoverBytes);
                Image levelCoverImage = levelCoverObjectImage.GetComponent<Image>();
                levelCoverImage.sprite = Sprite.Create(levelCoverTexture, new Rect(0, 0, levelCoverTexture.width, levelCoverTexture.height), new Vector2(0.5f, 0.5f));
            } catch (System.Exception e) {
                levelCoverObjectImage.GetComponent<Image>().sprite = Sprite.Create(Resources.Load<Texture2D>("WorldShow/default_world"), new Rect(0, 0, 256, 512), new Vector2(0.5f, 0.5f), 100f);
            }
            GameObject levelPercentObject = GameObject.Find($"Level_{level.level_id}_Percent");
            levelPercentObject.GetComponent<TextMeshProUGUI>().text = "0%";
            GameObject levelGemCountObject = GameObject.Find($"Level_{level.level_id}_Gems");
            levelGemCountObject.GetComponent<TextMeshProUGUI>().text = $"0/{level.display_gems[level.default_variation].ToString()}";
            GameObject levelCrownsObject = GameObject.Find($"Level_{level.level_id}_Crowns");
            switch (level.display_crowns[level.default_variation]) {
                case 1:
                    GameObject oneCrownObject = levelCrownsObject.transform.GetChild(0).gameObject;
                    oneCrownObject.SetActive(true);
                    break;
                case 2:
                    GameObject twoCrownsObject = levelCrownsObject.transform.GetChild(1).gameObject;
                    twoCrownsObject.SetActive(true);
                    break;
                case 3:
                    GameObject threeCrownsObject = levelCrownsObject.transform.GetChild(2).gameObject;
                    threeCrownsObject.SetActive(true);
                    break;
                default:
                    break;
            }
            GameObject levelVariationsObject = GameObject.Find($"Level_{level.level_id}_Variations");
            int defaultVariationIdx = level.level_variations.IndexOf(level.default_variation);
            if (level.level_variations.Count <= 1) {
                levelVariationsObject.SetActive(false);
            } else {
                TextMeshProUGUI variationName = levelVariationsObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                variationName.text = textInfo.ToTitleCase(level.level_variations[defaultVariationIdx]);
            }
            GameObject rightVariationObject = levelVariationsObject.transform.GetChild(1).gameObject;
            rightVariationObject.GetComponent<Button>().onClick.AddListener(NextVariation);
            GameObject leftVariationObject = levelVariationsObject.transform.GetChild(2).gameObject;
            leftVariationObject.GetComponent<Button>().onClick.AddListener(PreviousVariation);
        }
        currentVariationIdx = menuData.level_list[-menuIdx].level_variations.IndexOf(menuData.level_list[-menuIdx].default_variation);
        currentVariation = menuData.level_list[-menuIdx].level_variations[currentVariationIdx];
        levelCount = menuData.level_list.Count;
        //Debug.Log(levelCount);
        GameObject importLevelItem = Instantiate(levelItem, new Vector3(levelItem.transform.localPosition.x, 0f, 0f), Quaternion.identity, levelUIComponent.transform);
        importLevelItem.transform.localPosition = new Vector3(levelItem.transform.localPosition.x + 800f * (levelCount + 2), 0f, 0f);
        importLevelItem.transform.localRotation = Quaternion.identity;
        GameObject importLevelThumbnail = importLevelItem.transform.GetChild(0).gameObject;
        importLevelThumbnail.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Sprite.Create(Resources.Load<Texture2D>("WorldShow/default_world"), new Rect(0, 0, 256, 512), new Vector2(0.5f, 0.5f), 100f);
        importLevelThumbnail.GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("Not implemented yet");
        });
        GameObject importLevelTitle = importLevelItem.transform.GetChild(1).gameObject;
        importLevelTitle.GetComponent<TextMeshProUGUI>().text = "Import";
        GameObject importLevelPercentage = importLevelItem.transform.GetChild(2).gameObject;
        importLevelPercentage.GetComponent<TextMeshProUGUI>().text = "?%";
        GameObject importLevelGems = importLevelItem.transform.GetChild(3).gameObject;
        importLevelGems.GetComponent<TextMeshProUGUI>().text = "?/?";
        GameObject newLevelItem = Instantiate(levelItem, new Vector3(levelItem.transform.localPosition.x, 0f, 0f), Quaternion.identity, levelUIComponent.transform);
        newLevelItem.transform.localPosition = new Vector3(levelItem.transform.localPosition.x + 800f * (levelCount + 3), 0f, 0f);
        newLevelItem.transform.localRotation = Quaternion.identity;
        GameObject newLevelThumbnail = newLevelItem.transform.GetChild(0).gameObject;
        newLevelThumbnail.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Sprite.Create(Resources.Load<Texture2D>("WorldShow/default_world"), new Rect(0, 0, 256, 512), new Vector2(0.5f, 0.5f), 100f);
        newLevelThumbnail.GetComponent<Button>().onClick.AddListener(() => { 
            ShowCreateLevelPanel();
        });
        GameObject newLevelTitle = newLevelItem.transform.GetChild(1).gameObject;
        newLevelTitle.GetComponent<TextMeshProUGUI>().text = "New";
        GameObject newLevelPercentage = newLevelItem.transform.GetChild(2).gameObject;
        newLevelPercentage.GetComponent<TextMeshProUGUI>().text = "?%";
        GameObject newLevelGems = newLevelItem.transform.GetChild(3).gameObject;
        newLevelGems.GetComponent<TextMeshProUGUI>().text = "?/?";
        isInitialized = true;
    }

    public void ResetMenu() {
        menuIdx = 0;
        levelUIComponent.transform.localPosition = new Vector3(800f, 0f, 0f);
        //Debug.Log(levelUIComponent.transform.localPosition);
        titleText.SetActive(true);
        isInTitleScreen = true;
        leftButton.SetActive(false);
        rightButton.SetActive(false);
    }

    public void ShowCreateLevelPanel() {
        createLevelPanel.SetActive(true);
        
        if (modPathDropdown.options.Count == 0) {
            modPathDropdown.options.Add(new TMP_Dropdown.OptionData("null"));
            foreach (string modPath in scriptLoader.installedModPaths) {
                string substringToRemove = Application.persistentDataPath + "/Mods/";
                if (modPath.StartsWith(substringToRemove)) {
                    modPathDropdown.options.Add(new TMP_Dropdown.OptionData(modPath.Substring(substringToRemove.Length)));
                }
                //modPathDropdown.options.Add(new TMP_Dropdown.OptionData(modPath));
            }
        }
    }
    public void CloseCreateLevelPanel() {
        createLevelPanel.SetActive(false);
    }

    public void CreateLevelObjects() {
        for (int i = 0; i < menuData.level_list.Count; i++) {
            MenuLevelJsonWithVariations level = menuData.level_list[i];
            GameObject mhn_levelItem = Instantiate(levelItem, new Vector3(levelItem.transform.localPosition.x, 0f, 0f), Quaternion.identity, levelUIComponent.transform);
            mhn_levelItem.transform.localPosition = new Vector3(levelItem.transform.localPosition.x + 800f * (i + 1 + 1), 0f, 0f);
            mhn_levelItem.transform.localRotation = Quaternion.identity;
            GameObject mhn_CoverObject = mhn_levelItem.transform.GetChild(0).gameObject;
            mhn_CoverObject.name = $"Level_{level.level_id}_Cover_Button";
            GameObject mhn_CoverObjectImage = mhn_CoverObject.transform.GetChild(0).gameObject;
            mhn_CoverObjectImage.name = $"Level_{level.level_id}_Cover";
            GameObject mhn_LevelTitle = mhn_levelItem.transform.GetChild(1).gameObject;
            mhn_LevelTitle.name = $"Level_{level.level_id}_Title";
            GameObject mhn_LevelPercentage = mhn_levelItem.transform.GetChild(2).gameObject;
            mhn_LevelPercentage.name = $"Level_{level.level_id}_Percent";
            GameObject mhn_LevelGems = mhn_levelItem.transform.GetChild(3).gameObject;
            mhn_LevelGems.name = $"Level_{level.level_id}_Gems";
            GameObject mhn_LevelCrowns = mhn_levelItem.transform.GetChild(5).gameObject;
            mhn_LevelCrowns.name = $"Level_{level.level_id}_Crowns";
            GameObject mhn_LevelVariations = mhn_levelItem.transform.GetChild(6).gameObject;
            mhn_LevelVariations.name = $"Level_{level.level_id}_Variations";
        }
    }

    public float GeneralizedMean(float[] values, float p) {
        float sum = 0f;
        foreach (float value in values) {
            sum += Mathf.Pow(value, p);
        }
        return Mathf.Pow(sum / values.Length, 1f / p);
    }

    public float HarmonicMean(float[] values) {
        float sum = 0f;
        foreach (float value in values) {
            sum += 1f / value;
        }
        return values.Length / sum;
    }

    public float GeometricMean(float[] values) {
        float product = 1f;
        foreach (float value in values) {
            product *= value;
        }
        return Mathf.Pow(product, 1f / values.Length);
    }

    public float ArithmeticMean(float[] values) {
        float sum = 0f;
        foreach (float value in values) {
            sum += value;
        }
        return sum / values.Length;
    }

    public float QuadraticMean(float[] values) {
        float sum = 0f;
        foreach (float value in values) {
            sum += value * value;
        }
        return Mathf.Sqrt(sum / values.Length);
    }

    public float Softmax(float[] values) {
        return GeneralizedMean(values, 10f);
    }

    public float Softmin(float[] values) {
        return GeneralizedMean(values, -10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null) {
            if (gameManager.isInMainMenu) {
                DetectClick();
            } else {
                if (isMovingCamera && isInTitleScreen) {
                    MoveCamera(new Vector3(5f, 2.25f, -2f));
                } else if (isMovingLeft) {
                    MoveMenuLeft();
                } else if (isMovingRight) {
                    MoveMenuRight();
                }
            }
        }
    }

    void DetectClick()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Pointer.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject gameManagerObject = GameObject.Find("GameManager");
                if (hit.transform == gameManagerObject.transform || hit.transform == this.transform)
                {
                    // Add your code here to handle the click event
                    gameManager.SetIsInMainMenu(false);
                    isMovingCamera = true;
                    titleText.SetActive(false);
                    leftButton.SetActive(true);
                    rightButton.SetActive(true);
                }
            }
        }
    }

    public void LoadLevel(string level) {
        gameManager.LoadLevel(level);
        mainMenuCanvas.SetActive(false);
    }

    public void CreateNewLevel() {
        gameManager.CreateNewLevel(levelNameInputField.text, modPathDropdown.captionText.text);
        mainMenuCanvas.SetActive(false);
        levelCount++;
        CloseCreateLevelPanel();
    }

    private void MoveCamera(Vector3 targetPosition) {
        if (Camera.main.transform.position != targetPosition) {
            Camera.main.transform.position = Vector3.Lerp(new Vector3(0f, 2.25f, -2f), targetPosition, t);
            levelUIComponent.transform.localPosition = Vector3.Lerp(new Vector3(800f, 0f, 0), new Vector3(0f, 0f, 0f), t);
            t += 0.02f;
            if (t >= 0.98f) { 
                levelUIComponent.transform.localPosition = new Vector3(0f, 0f, 0f);
                currentLevelId = menuData.level_list[-menuIdx].level_id;
                currentVariation = menuData.level_list[-menuIdx].default_variation;
                if (isInTitleScreen) {
                    isInTitleScreen = false;
                }
                isMovingCamera = false;
                t = 0f;
                return;
            }
        }
    }
    public void MoveMenuRight() {
        if (!isMovingRight) isMovingRight = true;
        levelUIComponent.transform.localPosition = Vector3.Lerp(new Vector3(800f * (float)(menuIdx), 0f, 0f), new Vector3(800f * (float)(menuIdx - 1), 0f, 0f), t);
        t += 0.02f;
        if (t >= 0.98f) { 
            levelUIComponent.transform.localPosition = new Vector3(800f * (float)(menuIdx - 1), 0f, 0f);
            menuIdx--;
            if (-menuIdx < menuData.level_list.Count) {
                currentLevelId = menuData.level_list[-menuIdx].level_id;
                currentVariation = menuData.level_list[-menuIdx].default_variation;
            }
            try {
                gameManager.themeChanger2.UpdateTheme(menuData.level_list[-menuIdx].display_background[currentVariation]);
            } catch (System.Exception e) {
                gameManager.themeChanger2.UpdateTheme(0);
            }
            t = 0f;
            isMovingRight = false;
        }
    }

    public void MoveMenuLeft() {
        if (!isMovingLeft) isMovingLeft = true;
        levelUIComponent.transform.localPosition = Vector3.Lerp(new Vector3(800f * (float)(menuIdx), 0f, 0f), new Vector3(800f * (float)(menuIdx + 1), 0f, 0f), t);
        t += 0.02f;
        if (t >= 0.98f) {
            levelUIComponent.transform.localPosition = new Vector3(800f * (float)(menuIdx + 1), 0f, 0f);
            menuIdx++;
            if (-menuIdx >= 0) {
                currentLevelId = menuData.level_list[-menuIdx].level_id;
                currentVariation = menuData.level_list[-menuIdx].default_variation;
            }
            try {
                gameManager.themeChanger2.UpdateTheme(menuData.level_list[-menuIdx].display_background[currentVariation]);
            } catch (System.Exception e) {
                gameManager.themeChanger2.UpdateTheme(0);
            }
            t = 0f;
            isMovingLeft = false;
        }
    }

    public void NextVariation() {
        try {
            currentVariationIdx++;
            currentVariation = menuData.level_list[-menuIdx].level_variations[currentVariationIdx];
            MenuLevelJsonWithVariations level = menuData.level_list[-menuIdx];
            GameObject levelCoverObject = GameObject.Find($"Level_{level.level_id}_Cover_Button");
            levelCoverObject.GetComponent<Button>().onClick.RemoveAllListeners();
            levelCoverObject.GetComponent<Button>().onClick.AddListener(() => {
                if (currentVariation == "default") {
                    gameManager.LoadLevel(level.level_id);
                } else {
                    gameManager.LoadLevel(level.level_id + "_" + currentVariation);
                }
                mainMenuCanvas.SetActive(false);
            });
            GameObject levelTitleObject = GameObject.Find($"Level_{level.level_id}_Title");
            levelTitleObject.GetComponent<TextMeshProUGUI>().text = level.display_title[currentVariation];
            GameObject levelPercentObject = GameObject.Find($"Level_{level.level_id}_Percent");
            levelPercentObject.GetComponent<TextMeshProUGUI>().text = "0%";
            GameObject levelGemCountObject = GameObject.Find($"Level_{level.level_id}_Gems");
            levelGemCountObject.GetComponent<TextMeshProUGUI>().text = $"0/{level.display_gems[currentVariation].ToString()}";
            GameObject levelCrownsObject = GameObject.Find($"Level_{level.level_id}_Crowns");
            switch (level.display_crowns[currentVariation]) {
                case 1:
                    GameObject oneCrownObject = levelCrownsObject.transform.GetChild(0).gameObject;
                    oneCrownObject.SetActive(true);
                    break;
                case 2:
                    GameObject twoCrownsObject = levelCrownsObject.transform.GetChild(1).gameObject;
                    twoCrownsObject.SetActive(true);
                    break;
                case 3:
                    GameObject threeCrownsObject = levelCrownsObject.transform.GetChild(2).gameObject;
                    threeCrownsObject.SetActive(true);
                    break;
                default:
                    GameObject oneCrownObject2 = levelCrownsObject.transform.GetChild(0).gameObject;
                    GameObject twoCrownsObject2 = levelCrownsObject.transform.GetChild(1).gameObject;
                    GameObject threeCrownsObject2 = levelCrownsObject.transform.GetChild(2).gameObject;
                    oneCrownObject2.SetActive(false);
                    twoCrownsObject2.SetActive(false);
                    threeCrownsObject2.SetActive(false);
                    break;
            }
            GameObject levelVariationsObject = GameObject.Find($"Level_{level.level_id}_Variations");
            if (level.level_variations.Count <= 1) {
                levelVariationsObject.SetActive(false);
            } else {
                TextMeshProUGUI variationName = levelVariationsObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                variationName.text = textInfo.ToTitleCase(level.level_variations[currentVariationIdx]);
            }
            gameManager.themeChanger2.UpdateTheme(menuData.level_list[-menuIdx].display_background[currentVariation]);
        } catch (System.Exception e) {
            Debug.LogError(e);
        }
    }

    public void PreviousVariation() {
        try {
            currentVariationIdx--;
            currentVariation = menuData.level_list[-menuIdx].level_variations[currentVariationIdx];
            //Debug.Log(currentVariation);
            MenuLevelJsonWithVariations level = menuData.level_list[-menuIdx];
            GameObject levelCoverObject = GameObject.Find($"Level_{level.level_id}_Cover_Button");
            levelCoverObject.GetComponent<Button>().onClick.RemoveAllListeners();
            levelCoverObject.GetComponent<Button>().onClick.AddListener(() => {
                if (currentVariation == "default") {
                    gameManager.LoadLevel(level.level_id);
                } else {
                    gameManager.LoadLevel(level.level_id + "_" + currentVariation);
                }
                mainMenuCanvas.SetActive(false);
            });
            GameObject levelTitleObject = GameObject.Find($"Level_{level.level_id}_Title");
            levelTitleObject.GetComponent<TextMeshProUGUI>().text = level.display_title[currentVariation];
            GameObject levelPercentObject = GameObject.Find($"Level_{level.level_id}_Percent");
            levelPercentObject.GetComponent<TextMeshProUGUI>().text = "0%";
            GameObject levelGemCountObject = GameObject.Find($"Level_{level.level_id}_Gems");
            levelGemCountObject.GetComponent<TextMeshProUGUI>().text = $"0/{level.display_gems[currentVariation].ToString()}";
            GameObject levelCrownsObject = GameObject.Find($"Level_{level.level_id}_Crowns");
            switch (level.display_crowns[currentVariation]) {
                case 1:
                    GameObject oneCrownObject = levelCrownsObject.transform.GetChild(0).gameObject;
                    oneCrownObject.SetActive(true);
                    break;
                case 2:
                    GameObject twoCrownsObject = levelCrownsObject.transform.GetChild(1).gameObject;
                    twoCrownsObject.SetActive(true);
                    break;
                case 3:
                    GameObject threeCrownsObject = levelCrownsObject.transform.GetChild(2).gameObject;
                    threeCrownsObject.SetActive(true);
                    break;
                default:
                    GameObject oneCrownObject2 = levelCrownsObject.transform.GetChild(0).gameObject;
                    GameObject twoCrownsObject2 = levelCrownsObject.transform.GetChild(1).gameObject;
                    GameObject threeCrownsObject2 = levelCrownsObject.transform.GetChild(2).gameObject;
                    oneCrownObject2.SetActive(false);
                    twoCrownsObject2.SetActive(false);
                    threeCrownsObject2.SetActive(false);
                    break;
            }
            GameObject levelVariationsObject = GameObject.Find($"Level_{level.level_id}_Variations");
            if (level.level_variations.Count <= 1) {
                levelVariationsObject.SetActive(false);
            } else {
                TextMeshProUGUI variationName = levelVariationsObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                variationName.text = textInfo.ToTitleCase(level.level_variations[currentVariationIdx]);
            }
            gameManager.themeChanger2.UpdateTheme(menuData.level_list[-menuIdx].display_background[currentVariation]);
        } catch (System.Exception e) {
            Debug.LogError(e);
        }
    }
}
