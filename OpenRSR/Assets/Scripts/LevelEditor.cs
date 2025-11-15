using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System;
using UnityEngine.InputSystem;

public class LevelEditor : MonoBehaviour
{
    public GameObject balus;
    public GameObject gameOverPanel;
    public GameObject gamePlayCanvas;
    public GameObject gameEditCanvas;
    public GroundRenderer gre;
    public GameFreeze GFreeze;
    public Camera m_camera;
    public CameraFollow CFollow;
    public GameObject grid;
    private string realPercent;
    public EnemyRenderer ere;
    public LevelRenderer levelRenderer;
    public int gridSize = 10;
    public bool isInEditor = false;
    private bool isPopupOpen = false;
    private GameObject themeText;
    private LevelThemeChanger themeChanger;
    private SphereMovement sphm;
    private SphereDragger sphd;
    private ThemeChanger themeChanger2;
    private GameObject levelRendererObject;
    private Rigidbody rb;
    private GameManager manager;
    private string groundJsonString;
    private string enemyJsonString;
    private string levelJsonString;
    private string themeJsonString;
    private PositionsData gdata;
    private EnemyPositionsData edata;
    private NewLevelJson ldata;
    private LevelEventData ledata;
    public int objectLayer = 0;
    public int objectID = 0;
    public GameObject tilesPanel;
    public GameObject obstaclesPanel;
    public GameObject themesPanel;
    private LevelConfigurator levelConfig;
    // Start is called before the first frame update
    void Start()
    {
        balus = GameObject.FindGameObjectWithTag("Balus");
        levelRendererObject = GameObject.Find("LevelRenderer");
        rb = GetComponent<Rigidbody>();
        sphd = GetComponent<SphereDragger>();
        themeChanger = levelRendererObject.GetComponent<LevelThemeChanger>();
        sphm = GetComponent<SphereMovement>();
        themeChanger2 = levelRendererObject.GetComponent<ThemeChanger>();
        levelConfig = levelRendererObject.GetComponent<LevelConfigurator>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!manager.isDataDownloaded) {
            //groundJsonString = Resources.Load<TextAsset>("LevelData/Ground_valea").text;
            //enemyJsonString = Resources.Load<TextAsset>("LevelData/Enemies_valea").text;
            levelJsonString = Resources.Load<TextAsset>("LevelData/Level_valea").text;
            themeJsonString = Resources.Load<TextAsset>("LevelData/Themes_valea").text;
        } else {
            //groundJsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, gre.jsonFilePath + ".json"));
            //enemyJsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, ere.jsonFilePath + ".json"));
            levelJsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, levelRenderer.jsonFilePath + ".json"));
            themeJsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, themeChanger.jsonFilePath + ".json"));
        }
        //gdata = JsonConvert.DeserializeObject<PositionsData>(groundJsonString);
        //edata = JsonConvert.DeserializeObject<EnemyPositionsData>(enemyJsonString);
        ldata = JsonConvert.DeserializeObject<NewLevelJson>(levelJsonString);
        ledata = JsonConvert.DeserializeObject<LevelEventData>(themeJsonString);
        themeText = GameObject.Find("DeceBalus_Theme_Text");
    }

    public void ClearEverything() {
        GameObject[] normalTiles = GameObject.FindGameObjectsWithTag("NormalTile");
        GameObject[] jumpTiles = GameObject.FindGameObjectsWithTag("JumpTile");
        GameObject[] glassTiles = GameObject.FindGameObjectsWithTag("GlassTile");
        GameObject[] glassTilesGroup1 = GameObject.FindGameObjectsWithTag("GlassGroup1");
        GameObject[] glassTilesGroup2 = GameObject.FindGameObjectsWithTag("GlassGroup2");
        GameObject[] glassTilesGroup3 = GameObject.FindGameObjectsWithTag("GlassGroup3");
        GameObject[] moverTilesGroup1 = GameObject.FindGameObjectsWithTag("MoverGroup1");
        GameObject[] moverTilesGroup2 = GameObject.FindGameObjectsWithTag("MoverGroup2");
        GameObject[] moverTilesGroup3 = GameObject.FindGameObjectsWithTag("MoverGroup3");
        GameObject[] moverAutoTilesGroup1 = GameObject.FindGameObjectsWithTag("MoverAutoGroup1");
        GameObject[] moverAutoTilesGroup2 = GameObject.FindGameObjectsWithTag("MoverAutoGroup2");
        GameObject[] moverAutoTilesGroup3 = GameObject.FindGameObjectsWithTag("MoverAutoGroup3");
        GameObject[] risers = GameObject.FindGameObjectsWithTag("Riser");
        Vector3 jumpTilePos = new Vector3(-32f, 0.2f, -2f);
        Vector3 glassTilePos = new Vector3(-46f, 0f, 0f);
        Vector3 glassGroup1Pos = new Vector3(-49f, 0f, 0f);
        Vector3 glassGroup2Pos = new Vector3(-50f, 0f, 0f);
        Vector3 glassGroup3Pos = new Vector3(-51f, 0f, 0f);
        Vector3 moverGroup1Pos = new Vector3(-99f, 0f, 0f);
        Vector3 moverGroup2Pos = new Vector3(-101f, 0f, 0f);
        Vector3 moverGroup3Pos = new Vector3(-103f, 0f, 0f);
        Vector3 moverAutoGroup1Pos = new Vector3(-107f, 0f, 0f);
        Vector3 moverAutoGroup2Pos = new Vector3(-109f, 0f, 0f);
        Vector3 moverAutoGroup3Pos = new Vector3(-111f, 0f, 0f);
        Vector3 normalTilePos = new Vector3(-35f, 0.2f, 0.08f);
        List<Vector3> ObstaclePoses = new List<Vector3>();
        ObstaclePoses.Add(new Vector3(-33f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-40f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-42f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-51f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-54f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-56f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-60f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-64f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-66f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-68f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-70f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-72f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-74f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-76f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-5f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-83f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-97f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-102f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-100f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-104f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-106f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-108f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-110f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-112f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-114f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-116f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-118f, 0f, 0f));
        ObstaclePoses.Add(new Vector3(-124f, 0f, 0f));
        foreach (GameObject tile in normalTiles) {
            if (tile.transform.position == normalTilePos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in jumpTiles) {
            if (tile.transform.position == jumpTilePos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in glassTiles) {
            if (tile.transform.position == glassTilePos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in glassTilesGroup1) {
            if (tile.transform.position == glassGroup1Pos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in glassTilesGroup2) {
            if (tile.transform.position == glassGroup2Pos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in glassTilesGroup3) {
            if (tile.transform.position == glassGroup3Pos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in moverTilesGroup1) {
            if (tile.transform.position == moverGroup1Pos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in moverTilesGroup2) {
            if (tile.transform.position == moverGroup2Pos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in moverTilesGroup3) {
            if (tile.transform.position == moverGroup3Pos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in moverAutoTilesGroup1) {
            if (tile.transform.position == moverAutoGroup1Pos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in moverAutoTilesGroup2) {
            if (tile.transform.position == moverAutoGroup2Pos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in moverAutoTilesGroup3) {
            if (tile.transform.position == moverAutoGroup3Pos) {
                continue;
            }
            Destroy(tile);
        }
        foreach (GameObject tile in risers) {
            if (ObstaclePoses.Contains(tile.transform.position)) {
                continue;
            }
            Destroy(tile);
        }
    }

    // This is what replaced Mihnea with Workspace
    public void editorTransition() {
        if (!isInEditor) {
        //gre.enabled = false;
        //ere.enabled = false;
        levelRenderer.enabled = false;
        manager.enabled = false;
        GFreeze.enabled = false;
        gamePlayCanvas.SetActive(false);
        gameEditCanvas.SetActive(true);
        CFollow.enabled = false;
        sphd.enabled = false;
        sphm.enabled = false;
        balus.transform.position = new Vector3(0f, 0.5f, levelConfig.startPos);
        Vector3 camPos = new Vector3(0f, 10f, 0f);
        Quaternion q = Quaternion.Euler(90f, 0f, 0f);
        m_camera.transform.position = camPos;
        m_camera.transform.rotation = q;
        ClearEverything();
        themeChanger.UpdateData();
        //gre.UpdateData();
        //ere.UpdateData();
        levelRenderer.UpdateData();
        //gdata = gre.GetData();
        //edata = ere.GetData();
        ldata = levelRenderer.GetData();
        ledata = themeChanger.GetData();
        List<List<int>> gpositions = ldata.tiles;
        List<List<int>> epositions = ldata.enemies;
        for (int i = 0; i < gpositions.Count; i++) {
            for (int j = 0; j < gpositions[i].Count; j++) {
                int hasPrefab = gpositions[i][j];
                float x = j - 2;
                float z = i;
                Vector3 spawnPosition = new Vector3(x, 0f, z);
                GameObject spawnedPrefab = Instantiate(levelRenderer.tileSets[hasPrefab], spawnPosition, Quaternion.identity);
                if (hasPrefab == 4) {
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "1";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                } else if (hasPrefab == 5) {
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "2";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                } else if (hasPrefab == 6) {
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "3";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                } else if (hasPrefab == 7) {
                    if (spawnedPrefab.TryGetComponent<LeftMovingTileAnim>(out LeftMovingTileAnim leftMovingTileAnim)) {
                        leftMovingTileAnim.enabled = false;
                        GameObject tileBaseObject = spawnedPrefab.transform.Find("DeceBalus_Normal_Tile_Base").gameObject;
                        tileBaseObject.transform.position = new Vector3(x, 0f, z);
                        GameObject canvasObject = new GameObject("Text_Canvas");
                        Canvas canvas = canvasObject.AddComponent<Canvas>();
                        GameObject textObject = new GameObject("Text");
                        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                        textComponent.text = "<~";
                        textComponent.alignment = TextAlignmentOptions.Center;
                        canvas.renderMode = RenderMode.WorldSpace;
                        textComponent.fontStyle = FontStyles.Bold;
                        textComponent.fontSize = 0.4f;
                        canvasObject.transform.SetParent(spawnedPrefab.transform);
                        textObject.transform.SetParent(canvasObject.transform);
                        RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                        canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                        canvasObject.transform.position = new Vector3(x, 1f, z);
                    }
                } else if (hasPrefab == 8) {
                    if (spawnedPrefab.TryGetComponent<RightMovingTileAnim>(out RightMovingTileAnim rightMovingTileAnim)) {
                        rightMovingTileAnim.enabled = false;
                        GameObject tileBaseObject = spawnedPrefab.transform.Find("DeceBalus_Normal_Tile_Base").gameObject;
                        tileBaseObject.transform.position = new Vector3(x, 0f, z);
                        GameObject canvasObject = new GameObject("Text_Canvas");
                        Canvas canvas = canvasObject.AddComponent<Canvas>();
                        GameObject textObject = new GameObject("Text");
                        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                        textComponent.text = "~>";
                        textComponent.alignment = TextAlignmentOptions.Center;
                        canvas.renderMode = RenderMode.WorldSpace;
                        textComponent.fontStyle = FontStyles.Bold;
                        textComponent.fontSize = 0.4f;
                        canvasObject.transform.SetParent(spawnedPrefab.transform);
                        textObject.transform.SetParent(canvasObject.transform);
                        RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                        canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                        canvasObject.transform.position = new Vector3(x, 1f, z);
                    }
                } else if (hasPrefab == 9 || hasPrefab == 12) {
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "1";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                    GameObject moverEdge = hasPrefab == 9 ? levelRenderer.moverEdge : levelRenderer.moverAutoEdge;
                    GameObject moverEdge1 = Instantiate(moverEdge, new Vector3(x + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                    GameObject moverEdge2 = Instantiate(moverEdge, new Vector3(x - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                    GameObject moverEdge3 = Instantiate(moverEdge, new Vector3(x, 0f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    GameObject moverEdge4 = Instantiate(moverEdge, new Vector3(x, 0f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    moverEdge1.transform.parent = spawnedPrefab.transform;
                    moverEdge2.transform.parent = spawnedPrefab.transform;
                    moverEdge3.transform.parent = spawnedPrefab.transform;
                    moverEdge4.transform.parent = spawnedPrefab.transform;
                } else if (hasPrefab == 10 || hasPrefab == 13) {
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "2";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                    GameObject moverEdge = hasPrefab == 10 ? levelRenderer.moverEdge : levelRenderer.moverAutoEdge;
                    GameObject moverEdge1 = Instantiate(moverEdge, new Vector3(x + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                    GameObject moverEdge2 = Instantiate(moverEdge, new Vector3(x - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                    GameObject moverEdge3 = Instantiate(moverEdge, new Vector3(x, 0f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    GameObject moverEdge4 = Instantiate(moverEdge, new Vector3(x, 0f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    moverEdge1.transform.parent = spawnedPrefab.transform;
                    moverEdge2.transform.parent = spawnedPrefab.transform;
                    moverEdge3.transform.parent = spawnedPrefab.transform;
                    moverEdge4.transform.parent = spawnedPrefab.transform;
                } else if (hasPrefab == 11 || hasPrefab == 14) {
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "3";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                    GameObject moverEdge = hasPrefab == 11 ? levelRenderer.moverEdge : levelRenderer.moverAutoEdge;
                    GameObject moverEdge1 = Instantiate(moverEdge, new Vector3(x + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                    GameObject moverEdge2 = Instantiate(moverEdge, new Vector3(x - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                    GameObject moverEdge3 = Instantiate(moverEdge, new Vector3(x, 0f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    GameObject moverEdge4 = Instantiate(moverEdge, new Vector3(x, 0f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    moverEdge1.transform.parent = spawnedPrefab.transform;
                    moverEdge2.transform.parent = spawnedPrefab.transform;
                    moverEdge3.transform.parent = spawnedPrefab.transform;
                    moverEdge4.transform.parent = spawnedPrefab.transform;
                }
                // Obsolete code. Will remove later
                /*
                if (gpositions[i][j] == 1) {
                    float x = j - 2;
                    float z = i * gre.prefabSpacing;
                    Vector3 spawnPosition = new Vector3(x, 0f, z);
                    GameObject spawnedPrefab = Instantiate(gre.prefab, spawnPosition, Quaternion.identity);
                }
                else if (gpositions[i][j] == 2) {
                    float x = j - 2;
                    float z = i * gre.prefabSpacing;
                    Vector3 spawnPosition = new Vector3(x, 0f, z);
                    GameObject spawnedPrefab = Instantiate(gre.prefab2, spawnPosition, Quaternion.identity);
                } else if (gpositions[i][j] == 3) {
                    float x = j - 2;
                    float z = i * gre.prefabSpacing;
                    Vector3 spawnPosition = new Vector3(x, 0f, z);
                    GameObject spawnedPrefab = Instantiate(gre.prefab3, spawnPosition, Quaternion.identity);
                } */
            }
        }
        for (int i = 0; i < epositions.Count; i++) {
            for (int j = 0; j < epositions[i].Count; j++) {
                int hasPrefab = epositions[i][j];
                float x = j - 2;
                float z = i;
                Vector3 spawnPosition = new Vector3(x, 0.55f, z);
                if (hasPrefab == 4 || hasPrefab == 5 || (hasPrefab >= 10 && hasPrefab <= 15) || hasPrefab == 27) {
                    spawnPosition = new Vector3(x, 0f, z);
                } else if (hasPrefab == 6 || hasPrefab == 7) {
                    spawnPosition = new Vector3(x, 0.2f, z);
                } else if (hasPrefab == 8) {
                    spawnPosition = new Vector3(x + 1f, 0.2f, z);
                } else if (hasPrefab == 9) {
                    spawnPosition = new Vector3(x - 1f, 0.2f, z);
                } else {
                    spawnPosition = new Vector3(x, 0.55f, z);
                }
                GameObject spawnedPrefab = Instantiate(levelRenderer.enemySets[hasPrefab], spawnPosition, Quaternion.identity);
                
                if (hasPrefab == 3) {
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "SUS";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 2f, z);
                } else if (hasPrefab == 6 || hasPrefab == 7 || hasPrefab == 15) {
                    if (gpositions[i][j] == 3
                    || gpositions[i][j] == 4
                    || gpositions[i][j] == 5
                    || gpositions[i][j] == 6) {
                        GameObject rotor = Instantiate(levelRenderer.glassRotor, new Vector3(x, 0f, z), Quaternion.identity);
                        rotor.transform.SetParent(spawnedPrefab.transform);
                    } else {
                        GameObject rotor = Instantiate(levelRenderer.groundRotor, new Vector3(x, 0f, z), Quaternion.identity);
                        rotor.transform.SetParent(spawnedPrefab.transform);
                    }
                } else if (hasPrefab == 8) {
                    if (gpositions[i][j] == 3
                    || gpositions[i][j] == 4
                    || gpositions[i][j] == 5
                    || gpositions[i][j] == 6) {
                        GameObject rotor = Instantiate(levelRenderer.glassRotor, new Vector3(x + 1f, 0f, z), Quaternion.identity);
                        rotor.transform.SetParent(spawnedPrefab.transform);
                    } else {
                        GameObject rotor = Instantiate(levelRenderer.groundRotor, new Vector3(x + 1f, 0f, z), Quaternion.identity);
                        rotor.transform.SetParent(spawnedPrefab.transform);
                    }
                } else if (hasPrefab == 9) {
                    if (gpositions[i][j] == 3
                    || gpositions[i][j] == 4
                    || gpositions[i][j] == 5
                    || gpositions[i][j] == 6) {
                        GameObject rotor = Instantiate(levelRenderer.glassRotor, new Vector3(x - 1f, 0f, z), Quaternion.identity);
                        rotor.transform.SetParent(spawnedPrefab.transform);
                    } else {
                        GameObject rotor = Instantiate(levelRenderer.groundRotor, new Vector3(x - 1f, 0f, z), Quaternion.identity);
                        rotor.transform.SetParent(spawnedPrefab.transform);
                    }
                } else if (hasPrefab == 16) {
                    if (spawnedPrefab.TryGetComponent<LeftRollerAnim>(out LeftRollerAnim leftRollerAnim)) {
                        leftRollerAnim.enabled = false;
                        GameObject tileBaseObject = spawnedPrefab.transform.Find("DeceBalus_Roller_Base").gameObject;
                        tileBaseObject.transform.position = new Vector3(x, 0.55f, z);
                        GameObject canvasObject = new GameObject("Text_Canvas");
                        Canvas canvas = canvasObject.AddComponent<Canvas>();
                        GameObject textObject = new GameObject("Text");
                        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                        textComponent.text = "<~";
                        textComponent.alignment = TextAlignmentOptions.Center;
                        canvas.renderMode = RenderMode.WorldSpace;
                        textComponent.fontStyle = FontStyles.Bold;
                        textComponent.fontSize = 0.4f;
                        canvasObject.transform.SetParent(spawnedPrefab.transform);
                        textObject.transform.SetParent(canvasObject.transform);
                        RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                        canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                        canvasObject.transform.position = new Vector3(x, 1f, z);
                    }
                } else if (hasPrefab == 17) {
                    if (spawnedPrefab.TryGetComponent<RightRollerAnim>(out RightRollerAnim rightRollerAnim)) {
                        rightRollerAnim.enabled = false;
                        GameObject tileBaseObject = spawnedPrefab.transform.Find("DeceBalus_Roller_Base").gameObject;
                        tileBaseObject.transform.position = new Vector3(x, 0.55f, z);
                        GameObject canvasObject = new GameObject("Text_Canvas");
                        Canvas canvas = canvasObject.AddComponent<Canvas>();
                        GameObject textObject = new GameObject("Text");
                        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                        textComponent.text = "~>";
                        textComponent.alignment = TextAlignmentOptions.Center;
                        canvas.renderMode = RenderMode.WorldSpace;
                        textComponent.fontStyle = FontStyles.Bold;
                        textComponent.fontSize = 0.4f;
                        canvasObject.transform.SetParent(spawnedPrefab.transform);
                        textObject.transform.SetParent(canvasObject.transform);
                        RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                        canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                        canvasObject.transform.position = new Vector3(x, 1f, z);
                    }
                } else if (hasPrefab == 33) {
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "1";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 2f, z);
                } else if (hasPrefab == 34) {
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "2";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 2f, z);
                }
            }
        }
        List<float> themeZPositions = new List<float>();
        List<int> themeIds = new List<int>();
        Quaternion themetextRotation = Quaternion.Euler(90f, 0f, 0f);
        //Debug.Log(ledata.level_events.Count);
        foreach (BaseEvent level_event in ledata.level_events) {
            //Debug.Log(level_event.event_type);
            if (level_event.event_type == "theme_change") {
                GameObject[] themes = GameObject.FindGameObjectsWithTag("Theme");
                float z = level_event.z_position;
                foreach (GameObject theme in themes) {
                    if (theme.transform.position == new Vector3(-1.4f, 0.1f, z - 0.4f)) {
                        Destroy(theme);
                    }
                }
                Vector3 spawnPosition = new Vector3(-1.4f, 0.1f, z - 0.4f);
                GameObject spawnedPrefab = Instantiate(themeText, spawnPosition, themetextRotation);
                ThemeEditor te = spawnedPrefab.GetComponent<ThemeEditor>();
                //Debug.Log(themeIds[i]);
                te.themeID = Convert.ToInt32(level_event.event_fields["theme_id"]);
                te.event_type = level_event.event_type;
                TMP_Dropdown eventDropdownDropdown = te.eventDropdownDropdown;
                eventDropdownDropdown.value = 0;
            } else if (level_event.event_type == "filter_change") {
                GameObject[] themes = GameObject.FindGameObjectsWithTag("Theme");
                float z = level_event.z_position;
                foreach (GameObject theme in themes) {
                    if (theme.transform.position == new Vector3(-1.4f, 0.1f, z - 0.4f)) {
                        Destroy(theme);
                    }
                }
                Vector3 spawnPosition = new Vector3(-1.4f, 0.1f, z - 0.4f);
                GameObject spawnedPrefab = Instantiate(themeText, spawnPosition, themetextRotation);
                ThemeEditor te = spawnedPrefab.GetComponent<ThemeEditor>();
                te.event_type = level_event.event_type;
                te.filter_type = level_event.event_fields["filter_type"].ToString();
                TMP_Dropdown filterDropdownDropdown = te.filterDropdownDropdown;
                te.endAt = Convert.ToInt32(level_event.event_fields["end"]);
                switch (level_event.event_fields["filter_type"].ToString()) {
                    case "grayscale":
                        filterDropdownDropdown.value = 0;
                        te.hue = Convert.ToInt32(level_event.event_fields["hue"]);
                        te.saturation = Convert.ToInt32(level_event.event_fields["saturation"]);
                        te.value = Convert.ToInt32(level_event.event_fields["value"]);
                        te.hueEnd = Convert.ToInt32(level_event.event_fields["hue_end"]);
                        te.saturationEnd = Convert.ToInt32(level_event.event_fields["saturation_end"]);
                        te.valueEnd = Convert.ToInt32(level_event.event_fields["value_end"]);
                        break;
                    case "chromatic":
                        filterDropdownDropdown.value = 1;
                        te.xOffset = Convert.ToInt32(level_event.event_fields["x_offset"]);
                        te.yOffset = Convert.ToInt32(level_event.event_fields["y_offset"]);
                        te.xOffsetEnd = Convert.ToInt32(level_event.event_fields["x_offset_end"]);
                        te.yOffsetEnd = Convert.ToInt32(level_event.event_fields["y_offset_end"]);
                        break;
                    case "negative":
                        filterDropdownDropdown.value = 2;
                        te.intensity = Convert.ToInt32(level_event.event_fields["intensity"]);
                        te.intensityEnd = Convert.ToInt32(level_event.event_fields["intensity_end"]);
                        break;
                    case "glitch":
                        filterDropdownDropdown.value = 3;
                        te.stability = Convert.ToInt32(level_event.event_fields["stability"]);
                        te.stabilityEnd = Convert.ToInt32(level_event.event_fields["stability_end"]);
                        break;
                    case "scan_lines":
                        filterDropdownDropdown.value = 4;
                        te.intensity = Convert.ToInt32(level_event.event_fields["intensity"]);
                        te.intensityEnd = Convert.ToInt32(level_event.event_fields["intensity_end"]);
                        break;
                    case "hue_shift":
                        filterDropdownDropdown.value = 5;
                        te.shiftOffset = Convert.ToInt32(level_event.event_fields["shift_offset"]);
                        te.shiftOffsetEnd = Convert.ToInt32(level_event.event_fields["shift_offset_end"]);
                        break;
                }
                TMP_Dropdown eventDropdownDropdown = te.eventDropdownDropdown;
                eventDropdownDropdown.value = 1;
            } else if (level_event.event_type == "speed_change") {
                GameObject[] themes = GameObject.FindGameObjectsWithTag("Theme");
                float z = level_event.z_position;
                foreach (GameObject theme in themes) {
                    if (theme.transform.position == new Vector3(-1.4f, 0.1f, z - 0.4f)) {
                        Destroy(theme);
                    }
                }
                Vector3 spawnPosition = new Vector3(-1.4f, 0.1f, z - 0.4f);
                GameObject spawnedPrefab = Instantiate(themeText, spawnPosition, themetextRotation);
                ThemeEditor te = spawnedPrefab.GetComponent<ThemeEditor>();
                te.duration = Convert.ToInt32(level_event.event_fields["duration"]);
                Debug.Log(level_event.event_fields["multiplier"].GetType());
                te.multiplier = Convert.ToSingle(level_event.event_fields["multiplier"]);
                te.event_type = level_event.event_type;
                TMP_Dropdown eventDropdownDropdown = te.eventDropdownDropdown;
                eventDropdownDropdown.value = 2;
            }
        }

        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < 5; j++) {
                float x = j - 2;
                float z = i;
                Vector3 spawnPosition = new Vector3(x, 0f, z);
                GameObject spawnedPrefab = Instantiate(grid, spawnPosition, Quaternion.identity);
            }
        }
        isInEditor = true;
    }
    }

    public void saveLevelData() {
        for (int i = ldata.tiles.Count - 1; i >= 0; i--) {
            List<bool> isZero = new List<bool>();
            for (int j = ldata.tiles[i].Count - 1; j >= 0; j--) {
                if (ldata.tiles[i][j] == 0) {
                    isZero.Add(true);
                } else {
                    isZero.Add(false);
                }
            }
            if (isZero.All(x => x == true)) {
                ldata.tiles.RemoveAt(i);
                ldata.enemies.RemoveAt(i);
            } else {
                break;
            }
        }
        //string groundJsonString = JsonConvert.SerializeObject(gdata);
        //string enemyJsonString = JsonConvert.SerializeObject(edata);
        string levelJsonString = JsonConvert.SerializeObject(ldata);
        List<int> m_themeIDs = new List<int>();
        List<float> m_themeZPositions = new List<float>();
        GameObject[] m_themes = GameObject.FindGameObjectsWithTag("Theme");
        Array.Sort(m_themes, new GameObjectComparer());
        Vector3 m_themePos = new Vector3(-37f, 0f, 0f);
        List<BaseEvent> newLevelEvents = new List<BaseEvent>();
        foreach (GameObject m_theme in m_themes) {
            if (m_theme.transform.position == m_themePos) {
                continue;
            }
            ThemeEditor te = m_theme.GetComponent<ThemeEditor>();
            if (te.event_type == "theme_change") {
                // this code is retained for backwards compatibility
                m_themeIDs.Add(te.themeID);
                m_themeZPositions.Add(m_theme.transform.position.z + 0.4f);
            } else if (te.event_type == "filter_change") {
                if (te.filter_type == "grayscale") {
                    newLevelEvents.Add(new BaseEvent() { 
                        event_type = "filter_change", 
                        z_position = m_theme.transform.position.z + 0.4f, 
                        event_fields = new Dictionary<string, object>() { 
                            { "filter_type", "grayscale" }, 
                            { "hue" , te.hue }, 
                            { "saturation" , te.saturation }, 
                            { "value" , te.value }, 
                            { "hue_end" , te.hueEnd },
                            { "saturation_end" , te.saturationEnd },
                            { "value_end" , te.valueEnd },
                            { "end", te.endAt } 
                        }
                    });
                } else if (te.filter_type == "chromatic") {
                    newLevelEvents.Add(new BaseEvent() { 
                        event_type = "filter_change", 
                        z_position = m_theme.transform.position.z + 0.4f, 
                        event_fields = new Dictionary<string, object>() { 
                            { "filter_type", "chromatic" }, 
                            { "x_offset" , te.xOffset }, 
                            { "y_offset" , te.yOffset }, 
                            { "x_offset_end" , te.xOffsetEnd },
                            { "y_offset_end" , te.yOffsetEnd },
                            { "end", te.endAt } 
                        } 
                    });
                } else if (te.filter_type == "negative") {
                    newLevelEvents.Add(new BaseEvent() { 
                        event_type = "filter_change", 
                        z_position = m_theme.transform.position.z + 0.4f, 
                        event_fields = new Dictionary<string, object>() { 
                            { "filter_type", "negative" }, 
                            { "intensity" , te.intensity }, 
                            { "intensity_end" , te.intensityEnd },
                            { "end", te.endAt } 
                        } 
                    });
                } else if (te.filter_type == "glitch") {
                    newLevelEvents.Add(new BaseEvent() { 
                        event_type = "filter_change", 
                        z_position = m_theme.transform.position.z + 0.4f, 
                        event_fields = new Dictionary<string, object>() { 
                            { "filter_type", "glitch" }, 
                            { "stability" , te.stability }, 
                            { "stability_end" , te.stabilityEnd },
                            { "end", te.endAt } 
                        } 
                    });
                } else if (te.filter_type == "scan_lines") {
                    newLevelEvents.Add(new BaseEvent() { 
                        event_type = "filter_change", 
                        z_position = m_theme.transform.position.z + 0.4f, 
                        event_fields = new Dictionary<string, object>() { 
                            { "filter_type", "scan_lines" }, 
                            { "intensity" , te.intensity },
                            { "intensity_end" , te.intensityEnd }, 
                            { "end", te.endAt } 
                        } 
                    });
                } else if (te.filter_type == "hue_shift") {
                    newLevelEvents.Add(new BaseEvent() { 
                        event_type = "filter_change", 
                        z_position = m_theme.transform.position.z + 0.4f, 
                        event_fields = new Dictionary<string, object>() { 
                            { "filter_type", "hue_shift" }, 
                            { "shift_offset" , te.shiftOffset },
                            { "shift_offset_end" , te.shiftOffsetEnd }, 
                            { "end", te.endAt } 
                        } 
                    });
                }
            } else if (te.event_type == "speed_change") {
                newLevelEvents.Add(new BaseEvent() { event_type = "speed_change", z_position = m_theme.transform.position.z + 0.4f, event_fields = new Dictionary<string, object>() { { "duration", te.duration }, { "multiplier", te.multiplier } } });
            }
            Destroy(m_theme);
        }
        Debug.Log("Theme IDs: " + m_themeIDs.Count);
        for (int i = 0; i < m_themeIDs.Count; i++) {
            newLevelEvents.Add(new BaseEvent() { event_type = "theme_change", z_position = m_themeZPositions[i], event_fields = new Dictionary<string, object>() { { "theme_id", m_themeIDs[i] } } });
        }
        newLevelEvents.Sort(new BaseEventComparer());
        ledata.level_events = newLevelEvents;
        string themeJsonString = JsonConvert.SerializeObject(ledata);
        LevelConfigJson m_config = levelConfig.SaveConfig();
        string configJsonString = JsonConvert.SerializeObject(m_config);
        string SaveFileFolder = levelRenderer.levelFilePaths.FirstOrDefault(x => File.Exists(x + "/" + levelRenderer.jsonFilePath + ".json"));
        //File.WriteAllText(Path.Combine(SaveFileFolder, gre.jsonFilePath + ".json"), groundJsonString);
        //File.WriteAllText(Path.Combine(SaveFileFolder, ere.jsonFilePath + ".json"), enemyJsonString);
        File.WriteAllText(Path.Combine(SaveFileFolder, levelRenderer.jsonFilePath + ".json"), levelJsonString);
        File.WriteAllText(Path.Combine(SaveFileFolder, levelConfig.jsonFilePath + ".json"), configJsonString);
        File.WriteAllText(Path.Combine(SaveFileFolder, themeChanger.jsonFilePath + ".json"), themeJsonString);
        Quaternion defaultCameraRotation = Quaternion.Euler(40f, 0f, 0f);
        m_camera.transform.rotation = defaultCameraRotation;
        //gre.enabled = true;
        //ere.enabled = true;
        levelRenderer.enabled = true;
        themeChanger.enabled = true;
        //gre.UpdateData();
        //ere.UpdateData();
        levelRenderer.UpdateData();
        themeChanger.UpdateData();
        AudioPlayer audioPlayer = GetComponent<AudioPlayer>();
        audioPlayer.audioPath = levelConfig.musicPath;
        audioPlayer.UpdateAudioClip();
        Vector3 gridPosition = grid.transform.position;
        GameObject[] Grids = GameObject.FindGameObjectsWithTag("Grid");
        foreach (GameObject m_grid in Grids) {
            if (m_grid.transform.position == gridPosition) {
                continue;
            }
            Destroy(m_grid);
        }
        manager.enabled = true;
        manager.RestartGame();
        isInEditor = false;
        gameEditCanvas.SetActive(false);
        balus.transform.position = new Vector3(0f, 0.5f, levelConfig.startPos);
    }

    public void OnGoToButtonClick() {
        TMP_InputField offsetInputField = GameObject.Find("GoToInputField").GetComponent<TMP_InputField>();
        int offset = int.Parse(offsetInputField.text);
        GoToPosition(offset);
    }

    public void GoToPosition(int position) {
        m_camera.transform.position = new Vector3(m_camera.transform.position.x, m_camera.transform.position.y, (float)position);
    }

    // scrolls sus
    public void ScrollUp() {
        float camPosZ = m_camera.transform.position.z;
        camPosZ += 0.2f;
        Vector3 camPos = new Vector3(m_camera.transform.position.x, m_camera.transform.position.y, camPosZ);
        m_camera.transform.position = camPos;
    }

    // scrolls jos
    public void ScrollDown() {
        float camPosZ = m_camera.transform.position.z;
        camPosZ -= 0.2f;
        Vector3 camPos = new Vector3(m_camera.transform.position.x, m_camera.transform.position.y, camPosZ);
        if (m_camera.transform.position.z > 0f) {
            m_camera.transform.position = camPos;
        }
    }

    public void SetObjectID(int id) {
        objectID = id;
    }

    public void SetObjectLayer(int layer) {
        objectLayer = layer;
    }

    private void UpdateLevelData(int i, int j) {
        float x = j - 2;
        float z = i;
        GameObject[] normalTiles = GameObject.FindGameObjectsWithTag("NormalTile");
        GameObject[] jumpTiles = GameObject.FindGameObjectsWithTag("JumpTile");
        GameObject[] glassTiles = GameObject.FindGameObjectsWithTag("GlassTile");
        GameObject[] risers = GameObject.FindGameObjectsWithTag("Riser");
        GameObject[] glassTilesGroup1 = GameObject.FindGameObjectsWithTag("GlassGroup1");
        GameObject[] glassTilesGroup2 = GameObject.FindGameObjectsWithTag("GlassGroup2");
        GameObject[] glassTilesGroup3 = GameObject.FindGameObjectsWithTag("GlassGroup3");
        GameObject[] moverTilesGroup1 = GameObject.FindGameObjectsWithTag("MoverGroup1");
        GameObject[] moverTilesGroup2 = GameObject.FindGameObjectsWithTag("MoverGroup2");
        GameObject[] moverTilesGroup3 = GameObject.FindGameObjectsWithTag("MoverGroup3");
        GameObject[] moverAutoTilesGroup1 = GameObject.FindGameObjectsWithTag("MoverAutoGroup1");
        GameObject[] moverAutoTilesGroup2 = GameObject.FindGameObjectsWithTag("MoverAutoGroup2");
        GameObject[] moverAutoTilesGroup3 = GameObject.FindGameObjectsWithTag("MoverAutoGroup3");
        if (objectLayer == 0) {
            foreach (GameObject tile in normalTiles) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in jumpTiles) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in glassTiles) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in glassTilesGroup1) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in glassTilesGroup2) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in glassTilesGroup3) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in moverTilesGroup1) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in moverTilesGroup2) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in moverTilesGroup3) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in moverAutoTilesGroup1) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in moverAutoTilesGroup2) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            foreach (GameObject tile in moverAutoTilesGroup3) {
                if (tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            int hasPrefab = ldata.tiles[i][j];
            GameObject spawnedPrefab = Instantiate(levelRenderer.tileSets[hasPrefab], new Vector3(x, 0f, z), Quaternion.identity);
            if (hasPrefab == 4) {
                GameObject canvasObject = new GameObject("Text_Canvas");
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                GameObject textObject = new GameObject("Text");
                TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = "1";
                textComponent.alignment = TextAlignmentOptions.Center;
                canvas.renderMode = RenderMode.WorldSpace;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.fontSize = 0.4f;
                canvasObject.transform.SetParent(spawnedPrefab.transform);
                textObject.transform.SetParent(canvasObject.transform);
                RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                canvasObject.transform.position = new Vector3(x, 1f, z);
            } else if (hasPrefab == 5) {
                GameObject canvasObject = new GameObject("Text_Canvas");
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                GameObject textObject = new GameObject("Text");
                TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = "2";
                textComponent.alignment = TextAlignmentOptions.Center;
                canvas.renderMode = RenderMode.WorldSpace;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.fontSize = 0.4f;
                canvasObject.transform.SetParent(spawnedPrefab.transform);
                textObject.transform.SetParent(canvasObject.transform);
                RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                canvasObject.transform.position = new Vector3(x, 1f, z);
            } else if (hasPrefab == 6) {
                GameObject canvasObject = new GameObject("Text_Canvas");
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                GameObject textObject = new GameObject("Text");
                TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = "3";
                textComponent.alignment = TextAlignmentOptions.Center;
                canvas.renderMode = RenderMode.WorldSpace;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.fontSize = 0.4f;
                canvasObject.transform.SetParent(spawnedPrefab.transform);
                textObject.transform.SetParent(canvasObject.transform);
                RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                canvasObject.transform.position = new Vector3(x, 1f, z);
            } else if (hasPrefab == 7) {
                if (spawnedPrefab.TryGetComponent<LeftMovingTileAnim>(out LeftMovingTileAnim leftMovingTileAnim)) {
                    leftMovingTileAnim.enabled = false;
                    GameObject tileBaseObject = spawnedPrefab.transform.Find("DeceBalus_Normal_Tile_Base").gameObject;
                    tileBaseObject.transform.position = new Vector3(x, 0f, z);
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "<~";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                }
            } else if (hasPrefab == 8) {
                if (spawnedPrefab.TryGetComponent<RightMovingTileAnim>(out RightMovingTileAnim rightMovingTileAnim)) {
                    rightMovingTileAnim.enabled = false;
                    GameObject tileBaseObject = spawnedPrefab.transform.Find("DeceBalus_Normal_Tile_Base").gameObject;
                    tileBaseObject.transform.position = new Vector3(x, 0f, z);
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "~>";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                }
            } else if (hasPrefab == 9 || hasPrefab == 12) {
                GameObject canvasObject = new GameObject("Text_Canvas");
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                GameObject textObject = new GameObject("Text");
                TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = "1";
                textComponent.alignment = TextAlignmentOptions.Center;
                canvas.renderMode = RenderMode.WorldSpace;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.fontSize = 0.4f;
                canvasObject.transform.SetParent(spawnedPrefab.transform);
                textObject.transform.SetParent(canvasObject.transform);
                RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                canvasObject.transform.position = new Vector3(x, 1f, z);
                GameObject moverEdge = hasPrefab == 9 ? levelRenderer.moverEdge : levelRenderer.moverAutoEdge;
                GameObject moverEdge1 = Instantiate(moverEdge, new Vector3(x + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                GameObject moverEdge2 = Instantiate(moverEdge, new Vector3(x - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                GameObject moverEdge3 = Instantiate(moverEdge, new Vector3(x, 0f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                GameObject moverEdge4 = Instantiate(moverEdge, new Vector3(x, 0f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                moverEdge1.transform.parent = spawnedPrefab.transform;
                moverEdge2.transform.parent = spawnedPrefab.transform;
                moverEdge3.transform.parent = spawnedPrefab.transform;
                moverEdge4.transform.parent = spawnedPrefab.transform;
            } else if (hasPrefab == 10 || hasPrefab == 13) {
                GameObject canvasObject = new GameObject("Text_Canvas");
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                GameObject textObject = new GameObject("Text");
                TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = "2";
                textComponent.alignment = TextAlignmentOptions.Center;
                canvas.renderMode = RenderMode.WorldSpace;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.fontSize = 0.4f;
                canvasObject.transform.SetParent(spawnedPrefab.transform);
                textObject.transform.SetParent(canvasObject.transform);
                RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                canvasObject.transform.position = new Vector3(x, 1f, z);
                GameObject moverEdge = hasPrefab == 10 ? levelRenderer.moverEdge : levelRenderer.moverAutoEdge;
                GameObject moverEdge1 = Instantiate(moverEdge, new Vector3(x + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                GameObject moverEdge2 = Instantiate(moverEdge, new Vector3(x - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                GameObject moverEdge3 = Instantiate(moverEdge, new Vector3(x, 0f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                GameObject moverEdge4 = Instantiate(moverEdge, new Vector3(x, 0f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                moverEdge1.transform.parent = spawnedPrefab.transform;
                moverEdge2.transform.parent = spawnedPrefab.transform;
                moverEdge3.transform.parent = spawnedPrefab.transform;
                moverEdge4.transform.parent = spawnedPrefab.transform;
            } else if (hasPrefab == 11 || hasPrefab == 14) {
                GameObject canvasObject = new GameObject("Text_Canvas");
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                GameObject textObject = new GameObject("Text");
                TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = "3";
                textComponent.alignment = TextAlignmentOptions.Center;
                canvas.renderMode = RenderMode.WorldSpace;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.fontSize = 0.4f;
                canvasObject.transform.SetParent(spawnedPrefab.transform);
                textObject.transform.SetParent(canvasObject.transform);
                RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                canvasObject.transform.position = new Vector3(x, 1f, z);
                GameObject moverEdge = hasPrefab == 11 ? levelRenderer.moverEdge : levelRenderer.moverAutoEdge;
                GameObject moverEdge1 = Instantiate(moverEdge, new Vector3(x + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                GameObject moverEdge2 = Instantiate(moverEdge, new Vector3(x - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                GameObject moverEdge3 = Instantiate(moverEdge, new Vector3(x, 0f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                GameObject moverEdge4 = Instantiate(moverEdge, new Vector3(x, 0f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                moverEdge1.transform.parent = spawnedPrefab.transform;
                moverEdge2.transform.parent = spawnedPrefab.transform;
                moverEdge3.transform.parent = spawnedPrefab.transform;
                moverEdge4.transform.parent = spawnedPrefab.transform;
            }
            /*
            if (gdata.positions[i][j] == 1) {
                Instantiate(gre.prefab, new Vector3(x, 0f, z), Quaternion.identity);
            } else if (gdata.positions[i][j] == 2) {
                Instantiate(gre.prefab2, new Vector3(x, 0f, z), Quaternion.identity);
            } else if (gdata.positions[i][j] == 3) {
                Instantiate(gre.prefab3, new Vector3(x, 0f, z), Quaternion.identity);
            } */
        } else if (objectLayer == 1) {
            foreach (GameObject tile in risers) {
                if (tile.transform.position == new Vector3(x, 0.55f, z) || tile.transform.position == new Vector3(x, 0.2f, z) || tile.transform.position == new Vector3(x, 0f, z)) {
                    Destroy(tile);
                    break;
                } else if (tile.transform.position == new Vector3(x + 1f, 0.2f, z) || tile.transform.position == new Vector3(x - 1f, 0.2f, z)) {
                    Destroy(tile);
                    break;
                }
            }
            int hasPrefab = ldata.enemies[i][j];
            Vector3 spawnPosition = new Vector3(x, 0.55f, z);
            if (hasPrefab == 4 || hasPrefab == 5 || (hasPrefab >= 10 && hasPrefab <= 15) || hasPrefab == 27) {
                spawnPosition = new Vector3(x, 0f, z);
            } else if (hasPrefab == 6 || hasPrefab == 7) {
                spawnPosition = new Vector3(x, 0.2f, z);
            } else if (hasPrefab == 8) {
                spawnPosition = new Vector3(x + 1f, 0.2f, z);
            } else if (hasPrefab == 9) {
                spawnPosition = new Vector3(x - 1f, 0.2f, z);
            }
            GameObject spawnedPrefab = Instantiate(levelRenderer.enemySets[hasPrefab], spawnPosition, Quaternion.identity);
            if (hasPrefab == 3) {
                GameObject canvasObject = new GameObject("Text_Canvas");
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                GameObject textObject = new GameObject("Text");
                TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = "SUS";
                textComponent.alignment = TextAlignmentOptions.Center;
                canvas.renderMode = RenderMode.WorldSpace;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.fontSize = 0.4f;
                canvasObject.transform.SetParent(spawnedPrefab.transform);
                textObject.transform.SetParent(canvasObject.transform);
                RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                canvasObject.transform.position = new Vector3(x, 2f, z);
            } else if (hasPrefab == 6 || hasPrefab == 7 || hasPrefab == 15) {
                List<List<int>> gpositions = ldata.tiles;
                if (gpositions[i][j] == 3
                || gpositions[i][j] == 4
                || gpositions[i][j] == 5
                || gpositions[i][j] == 6) {
                    GameObject rotor = Instantiate(levelRenderer.glassRotor, new Vector3(x, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                } else {
                    GameObject rotor = Instantiate(levelRenderer.groundRotor, new Vector3(x, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                }
            } else if (hasPrefab == 8) {
                List<List<int>> gpositions = ldata.tiles;
                if (gpositions[i][j] == 3
                || gpositions[i][j] == 4
                || gpositions[i][j] == 5
                || gpositions[i][j] == 6) {
                    GameObject rotor = Instantiate(levelRenderer.glassRotor, new Vector3(x + 1f, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                } else {
                    GameObject rotor = Instantiate(levelRenderer.groundRotor, new Vector3(x + 1f, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                }
            } else if (hasPrefab == 9) {
                List<List<int>> gpositions = ldata.tiles;
                if (gpositions[i][j] == 3
                || gpositions[i][j] == 4
                || gpositions[i][j] == 5
                || gpositions[i][j] == 6) {
                    GameObject rotor = Instantiate(levelRenderer.glassRotor, new Vector3(x - 1f, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                } else {
                    GameObject rotor = Instantiate(levelRenderer.groundRotor, new Vector3(x - 1f, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                }
            } else if (hasPrefab == 16) {
                if (spawnedPrefab.TryGetComponent<LeftRollerAnim>(out LeftRollerAnim leftRollerAnim)) {
                    leftRollerAnim.enabled = false;
                    GameObject tileBaseObject = spawnedPrefab.transform.Find("DeceBalus_Roller_Base").gameObject;
                    tileBaseObject.transform.position = new Vector3(x, 0.55f, z);
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "<~";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                }
            } else if (hasPrefab == 17) {
                if (spawnedPrefab.TryGetComponent<RightRollerAnim>(out RightRollerAnim rightRollerAnim)) {
                    rightRollerAnim.enabled = false;
                    GameObject tileBaseObject = spawnedPrefab.transform.Find("DeceBalus_Roller_Base").gameObject;
                    tileBaseObject.transform.position = new Vector3(x, 0.55f, z);
                    GameObject canvasObject = new GameObject("Text_Canvas");
                    Canvas canvas = canvasObject.AddComponent<Canvas>();
                    GameObject textObject = new GameObject("Text");
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = "~>";
                    textComponent.alignment = TextAlignmentOptions.Center;
                    canvas.renderMode = RenderMode.WorldSpace;
                    textComponent.fontStyle = FontStyles.Bold;
                    textComponent.fontSize = 0.4f;
                    canvasObject.transform.SetParent(spawnedPrefab.transform);
                    textObject.transform.SetParent(canvasObject.transform);
                    RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                    canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                    canvasObject.transform.position = new Vector3(x, 1f, z);
                }
            } else if (hasPrefab == 33) {
                GameObject canvasObject = new GameObject("Text_Canvas");
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                GameObject textObject = new GameObject("Text");
                TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = "1";
                textComponent.alignment = TextAlignmentOptions.Center;
                canvas.renderMode = RenderMode.WorldSpace;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.fontSize = 0.4f;
                canvasObject.transform.SetParent(spawnedPrefab.transform);
                textObject.transform.SetParent(canvasObject.transform);
                RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                canvasObject.transform.position = new Vector3(x, 2f, z);
            } else if (hasPrefab == 34) {
                GameObject canvasObject = new GameObject("Text_Canvas");
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                GameObject textObject = new GameObject("Text");
                TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = "2";
                textComponent.alignment = TextAlignmentOptions.Center;
                canvas.renderMode = RenderMode.WorldSpace;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.fontSize = 0.4f;
                canvasObject.transform.SetParent(spawnedPrefab.transform);
                textObject.transform.SetParent(canvasObject.transform);
                RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
                canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                canvasRectTransform.sizeDelta = new Vector2(1.2f, 1.2f);
                canvasObject.transform.position = new Vector3(x, 2f, z);
            }
        }
    }

    public void SetPopupOpen(bool open) {
        isPopupOpen = open;
    }

    public void ClearThemes() {
        GameObject[] m_themes = GameObject.FindGameObjectsWithTag("Theme");
        Vector3 m_themePos = new Vector3(-37f, 0f, 0f);
        foreach (GameObject m_theme in m_themes) {
            if (m_theme.transform.position == m_themePos) {
                continue;
            }
            Destroy(m_theme);
        }
        GameObject first_theme = Instantiate(themeText, new Vector3(-1.4f, 0.1f, -0.4f), Quaternion.Euler(90f, 0f, 0f));
    }

    public void ClearLevel() {
        List<List<int>> groundNewPositions = new List<List<int>>();
        for (int i = 0; i < 10; i++) {
            groundNewPositions.Add(new List<int>(){ 0, 0, 0, 0, 0 });
        }
        List<List<int>> enemyNewPositions = new List<List<int>>();
        for (int i = 0; i < 10; i++) {
            enemyNewPositions.Add(new List<int>(){ 0, 0, 0, 0, 0 });
        }
        gdata.positions = groundNewPositions;
        edata.positions = enemyNewPositions;
        ClearEverything();
        ClearThemes();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_camera.transform.position.z + 6.9f >= gridSize && isInEditor) {
            for (int i = 0; i < 5; i++) {
                float x = i - 2;
                float z = gridSize;
                Vector3 spawnPosition = new Vector3(x, 0f, z);
                GameObject spawnedPrefab = Instantiate(grid, spawnPosition, Quaternion.identity);
            }
            gridSize++;
        } else if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame) {
            Vector3 mousePos = Pointer.current.position.ReadValue();
            Vector3 worldPos = m_camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 9.5f));
            //Debug.Log(worldPos);
            worldPos.z = Mathf.Round(worldPos.z);
            worldPos.x = Mathf.Round(worldPos.x);
            int i = (int)worldPos.z;
            int j = (int)worldPos.x + 2;
            //Debug.Log(i + " " + j);
            if (objectLayer == 0) {
                if (i >= ldata.tiles.Count) {
                    for (int k = 0; k <= i - ldata.tiles.Count; k++) {
                        ldata.tiles.Add(new List<int>(){ 0, 0, 0, 0, 0 });
                        ldata.enemies.Add(new List<int>(){ 0, 0, 0, 0, 0 });
                    }
                }
                if (i >= 0 && i < ldata.tiles.Count && j >= 0 && j < 5) {
                    ldata.tiles[i][j] = objectID;
                    UpdateLevelData(i, j);
                }
            } else if (objectLayer == 1) {
                if (i >= ldata.enemies.Count) {
                    for (int k = 0; k <= i - ldata.enemies.Count; k++) {
                        ldata.tiles.Add(new List<int>(){ 0, 0, 0, 0, 0 });
                        ldata.enemies.Add(new List<int>(){ 0, 0, 0, 0, 0 });
                    }
                }
                if (i >= 0 && i < ldata.enemies.Count && j >= 0 && j < 5) {
                    ldata.enemies[i][j] = objectID;
                    UpdateLevelData(i, j);
                }
            } else if (objectLayer == 2) {
                if (objectID == 0 && i >= 0 && j >= 0 && j < 5) {
                    int k = 0;
                    bool themeAlreadyRemoved = false;
                    GameObject[] m_themes = GameObject.FindGameObjectsWithTag("Theme");
                    Array.Sort(m_themes, new GameObjectComparer());
                    foreach (GameObject m_theme in m_themes) {
                        if (m_theme.transform.position == new Vector3(-1.4f, 0.1f, i - 0.4f)) {
                            Destroy(m_theme);
                            themeAlreadyRemoved = false;
                            break;
                        } else {
                            themeAlreadyRemoved = true;
                        }
                        k++;
                    }
                    //Debug.Log(ltdata.themeZPositions.Count);
                    //Debug.Log(k);
                    if (!themeAlreadyRemoved) {
                        ledata.level_events.RemoveAt(k-1);
                    }
                }
                if (objectID == 1 && i >= 0 && j >= 0 && j < 5) {
                    bool themeAlreadyExists = false;
                    GameObject[] m_themes = GameObject.FindGameObjectsWithTag("Theme");
                    Array.Sort(m_themes, new GameObjectComparer());
                    GameObject currentTheme = null;
                    foreach (GameObject m_theme in m_themes) {
                        if (m_theme.transform.position == new Vector3(-1.4f, 0.1f, i - 0.4f)) {
                            Debug.Log(m_theme.transform.position);
                            themeAlreadyExists = true;
                            currentTheme = m_theme;
                            break;
                        }
                    }
                    if (!themeAlreadyExists) {
                        ledata.level_events.Add(new BaseEvent() { event_type = "theme_change", z_position = i, event_fields = new Dictionary<string, object>() { { "theme_id", 0 } } });
                        currentTheme = Instantiate(themeText, new Vector3(-1.4f, 0.1f, i - 0.4f), Quaternion.Euler(90f, 0f, 0f));
                        ThemeEditor te = currentTheme.GetComponent<ThemeEditor>();
                        te.themeID = 0;
                    } else {
                        ThemeEditor te = currentTheme.GetComponent<ThemeEditor>();
                        te.OpenEditThemePanel(i - 0.4f);
                    }
                }
            }
        } else if (objectLayer == 0) {
            tilesPanel.SetActive(true);
            obstaclesPanel.SetActive(false);
            themesPanel.SetActive(false);
        } else if (objectLayer == 1) {
            tilesPanel.SetActive(false);
            obstaclesPanel.SetActive(true);
            themesPanel.SetActive(false);
        } else if (objectLayer == 2) {
            tilesPanel.SetActive(false);
            obstaclesPanel.SetActive(false);
            themesPanel.SetActive(true);
        }
        //Debug.Log(isInEditor);
        if (isInEditor) {
            GameObject[] mhn_themes = GameObject.FindGameObjectsWithTag("Theme");
            Array.Sort(mhn_themes, new GameObjectComparer());
            for (int i = 1; i < mhn_themes.Length; i++) {
                if (mhn_themes[i].transform.position.z < m_camera.transform.position.z && m_camera.transform.position.z > mhn_themes[i-1].transform.position.z) {
                    ThemeEditor te = mhn_themes[i].GetComponent<ThemeEditor>();
                    if (te.event_type == "theme_change") {
                        if (themeChanger2.themeID != te.themeID) {
                            themeChanger2.UpdateTheme(te.themeID);
                        }
                    }
                }
            }
        }
    }
}

public class GameObjectComparer : IComparer {

    public int Compare(object x, object y) {
        return (x as GameObject).transform.position.z.CompareTo((y as GameObject).transform.position.z);
    }
}

public class BaseEventComparer : IComparer<BaseEvent> {

    public int Compare(BaseEvent x, BaseEvent y) {
        return (x as BaseEvent).z_position.CompareTo((y as BaseEvent).z_position);
    }
}
