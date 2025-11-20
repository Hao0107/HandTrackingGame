using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using OpenRSR.Animation;

public class GameManager : MonoBehaviour
{
    public GameObject balus;
    public GameObject gameOverPanel;
    public GameObject gamePlayCanvas;
    public GameObject settingsPanel;
    public GameObject loadingPanel;
    public List<AnimationCurve> curves = new List<AnimationCurve>();
    public GroundRenderer gre;
    public GameFreeze GFreeze;
    public CameraFollow CFollow;
    public string geoBufferJsonFilePath = "LevelData/GeoBuffer1.json";
    private GameObject percentTextLabel;
    private TextMeshProUGUI percentTextMesh;
    private string realPercent;
    public EnemyRenderer ere;
    public LevelRenderer levelRenderer;
    public LevelThemeChanger themeChanger;
    public SphereMovement sphm;
    public NonSphereMovement nsm;
    private SphereDragger sphd;
    public ThemeChanger themeChanger2;
    private GameObject levelRendererObject;
    public LevelConfigurator levelConfig;
    public LevelEditor levelEdit;
    public bool isGameOver = false;
    public bool isDataDownloaded = false;
    private bool isDataDownloadedCache = false;
    public bool isGamePaused = true;
    public bool isInMainMenu = true;
    private Rigidbody rb;
    private AudioPlayer audioPlayer;
    public bool isDeathDisabled = false;
    private IEnumerator updateCacheCoroutine;
    private ObjectPool objectPool;
    private GeoBufferJson geoBufferJson;
    private GameObject mainMenuNormalTile;
    public MainMenuScripts mainMenuScripts;
    public int num_collectedGems = 0;
    public int num_collectedCrowns = 0;
    private int totalGemCount = 0;
    private int totalCrownCount = 0;
    public int downloadCount = 0;
    public int totalDownloadsRequired = 0;
    private List<GameObject> collectedGems = new List<GameObject>();
    private GameObject levelEditButton;
    private GameObject levelEndObject;
    private GameObject mainMenuCanvas;
    public bool hasFallen = false;
    public bool hasHitObstacle = false;
    public bool delayed = false;
    public bool isPlayingAnimation = false;
    public bool isPlayingAnimationGroup = false;
    public bool isPlayingObjectAnimation = false;
    public string currentlyPlayingAnimation = "";
    public string currentlyPlayingAnimationGroup = "";
    public BaseObject currentlyPlayingObjectAnimation = null;
    public int minAnimationCount = 0;
    public static GameManager instance;
    public Scene currentScene;
    public Dictionary<string, FrameAnim> anims = new Dictionary<string, FrameAnim>();
    public Dictionary<string, List<FrameAnim>> animGroups = new Dictionary<string, List<FrameAnim>>();
    private Coroutine downloadCoroutine;
    IEnumerator UpdateCache()
    {
        while (true)
        {
            // Update the cached value
            isDataDownloadedCache = !isDataDownloadedCache;

            // Wait for a few seconds before updating again
            yield return new WaitForSeconds(1f);
        }
    }
    void Awake() {
        instance = this;
    }

    public bool IsDataDownloaded() {
        return File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_valea.json")) 
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_valea.json")) 
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_valea.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_valea.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_aperta.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_aperta.json")) 
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_aperta.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_aperta.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_gardenea_older.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_gardenea_older.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_gardenea_older.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_gardenea_older.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_gardenea_old.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_gardenea_old.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_gardenea_old.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_gardenea_old.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_gardenea_new.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_gardenea_new.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_gardenea_new.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_gardenea_new.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "ThemeData/ThemeData.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background1.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background2.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background3.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background4.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background5.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background6.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Enemy1.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Enemy2.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Enemy3.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Enemy4.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Enemy5.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Enemy6.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "General1.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "General2.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "General3.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "General4.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "General5.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "General6.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Music/Music1.mp3"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Music/Music2.mp3"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "Music/Music3.mp3"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "WorldShow/World_valea.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "WorldShow/World_aperta.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "WorldShow/World_gardenea.png"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "MenuData/MenuData.json"))

        // My data
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_mylevel.json"))  
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_mylevel.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_mylevel.json"))
        && File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_mylevel.json"));

    }
    
    void Start()
    {

        //GenerateMyLongLevel();

        instance = this;
        percentTextLabel = GameObject.Find("Percent");
        percentTextMesh = percentTextLabel.GetComponent<TextMeshProUGUI>();
        balus = GameObject.FindGameObjectWithTag("Balus");
        mainMenuScripts = balus.GetComponent<MainMenuScripts>();
        levelRendererObject = GameObject.Find("LevelRenderer");
        levelRenderer = levelRendererObject.GetComponent<LevelRenderer>();
        GFreeze = levelRendererObject.GetComponent<GameFreeze>();
        GameObject objPoolObj = GameObject.Find("ObjectPool");
        objectPool = objPoolObj.GetComponent<ObjectPool>();
        rb = balus.GetComponent<Rigidbody>();
        sphd = balus.GetComponent<SphereDragger>();
        themeChanger = levelRendererObject.GetComponent<LevelThemeChanger>();
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "DebugScene") {
            nsm = GetComponent<NonSphereMovement>();
        }
        sphm = balus.GetComponent<SphereMovement>();
        themeChanger2 = levelRendererObject.GetComponent<ThemeChanger>();
        levelConfig = levelRendererObject.GetComponent<LevelConfigurator>();
        audioPlayer = balus.GetComponent<AudioPlayer>();
        if (sphm != null) {
        sphm.speed = levelConfig.levelSpeed;
        }
        audioPlayer.audioPath = levelConfig.musicPath;
        audioPlayer.LoadAudioClip();
        levelEdit = balus.GetComponent<LevelEditor>();
        levelEditButton = GameObject.Find("EditButton");
        levelEndObject = GameObject.Find("End");
        mainMenuCanvas = GameObject.Find("MainMenu");

        BaseAnim[] baseAnims = FindObjectsByType<BaseAnim>(FindObjectsSortMode.None);
        foreach (BaseAnim baseAnim in baseAnims) {
            foreach (FrameAnim anim in baseAnim.animators) {
                anims[anim.name.name] = anim;
            }
        }

        //Application.targetFrameRate = 60;

        if (IsDataDownloaded())
        {
            isDataDownloaded = true;
            isDataDownloadedCache = true;

            //LoadLevel("mylevel");
        }
        else
        {
            isDataDownloaded = false;
            isDataDownloadedCache = true;
        }
        updateCacheCoroutine = UpdateCache();
        StartCoroutine(updateCacheCoroutine);

        isGamePaused = GFreeze.gamePaused;

        if (isDataDownloaded)
        {
            CFollow.enabled = false;
            Camera.main.transform.position = new Vector3(0f, 2.25f, -2f);
            GFreeze.enabled = false;
            //gre.enabled = false;
            //ere.enabled = false;
            levelRenderer.enabled = false;
            themeChanger.enabled = false;
            if (sphm != null) {
            sphm.enabled = false;
            }
            sphd.enabled = false;
            themeChanger2.enabled = true;
            //string jsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, geoBufferJsonFilePath));
            //geoBufferJson = JsonConvert.DeserializeObject<GeoBufferJson>(jsonString);
            //objectPool.InitializePools(gre.prefabs, ere.prefabs, geoBufferJson);
            gamePlayCanvas.SetActive(false);
        }
        else 
        {
            CFollow.enabled = false;
            Camera.main.transform.position = new Vector3(0f, 2.25f, -2f);
            GFreeze.enabled = false;
            rb.useGravity = false;
            //gre.enabled = false;
            //ere.enabled = false;
            levelRenderer.enabled = false;
            themeChanger.enabled = false;
            sphm.enabled = false;
            sphd.enabled = false;
            themeChanger2.enabled = false;
            if (totalDownloadsRequired == 0) {
                totalDownloadsRequired = ConvertBoolToInt(!Directory.Exists(Path.Combine(Application.persistentDataPath, "LevelData")))
                + ConvertBoolToInt(!Directory.Exists(Path.Combine(Application.persistentDataPath, "ThemeData")))
                + ConvertBoolToInt(!Directory.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds")))
                + ConvertBoolToInt(!Directory.Exists(Path.Combine(Application.persistentDataPath, "Music")))
                + ConvertBoolToInt(!Directory.Exists(Path.Combine(Application.persistentDataPath, "WorldShow")))
                + ConvertBoolToInt(!Directory.Exists(Path.Combine(Application.persistentDataPath, "MenuData")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "ThemeData/ThemeData.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_valea.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_valea.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_valea.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_valea.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_aperta.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_aperta.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_aperta.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_aperta.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_gardenea_older.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_gardenea_older.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_gardenea_older.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_gardenea_older.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_gardenea_old.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_gardenea_old.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_gardenea_old.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_gardenea_old.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_gardenea_new.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_gardenea_new.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_gardenea_new.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_gardenea_new.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background1.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Enemy1.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "General1.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background2.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Enemy2.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "General2.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background3.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Enemy3.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "General3.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background4.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Enemy4.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "General4.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background5.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Enemy5.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "General5.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Backgrounds/Background6.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Enemy6.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "General6.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Music/Music1.mp3")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Music/Music2.mp3")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "Music/Music3.mp3")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "WorldShow/World_valea.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "WorldShow/World_aperta.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "WorldShow/World_gardenea.png")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "MenuData/MenuData.json")))

                // My data to Download
                +ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Level_mylevel.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Themes_mylevel.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/Config_mylevel.json")))
                + ConvertBoolToInt(!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData/GeoBuffer_mylevel.json")));
                // End My data to Download
            }
            if (downloadCoroutine == null) {
            downloadCoroutine = StartCoroutine(LoadData());
            }
            gamePlayCanvas.SetActive(false);
            themeChanger.enabled = true;
            themeChanger2.enabled = true;
            themeChanger2.UpdateTheme(0);
        }
        mainMenuNormalTile = Instantiate(levelRenderer.tileSets[1], new Vector3(0f, 0f, rb.position.z), Quaternion.identity);
        //Debug.Log(anims.Count);
    }

    public void ExitToMainMenu() {
        themeChanger.themeID = 0;
        themeChanger2.UpdateTheme(themeChanger.themeID);
        if (levelConfig.startPortal) {
            levelConfig.startPortalObject2.SetActive(false);
        }
        CFollow.enabled = false;
        Camera.main.transform.position = new Vector3(0f, 2.25f, -2f);
        audioPlayer.SeekToZero();
        if (sphm != null) sphm.ClearFallingObstacles();
        objectPool.ClearAllPools();
        //gre.ClearPrefabPositions();
        //ere.ClearPrefabPositions();
        GFreeze.enabled = false;
        //gre.enabled = false;
        //ere.enabled = false;
        levelRenderer.enabled = false;
        themeChanger.enabled = false;
        sphm.enabled = false;
        sphd.enabled = false;
        themeChanger2.enabled = true;
        //string jsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, geoBufferJsonFilePath));
        //geoBufferJson = JsonConvert.DeserializeObject<GeoBufferJson>(jsonString);
        //objectPool.InitializePools(levelRenderer.tileSets, levelRenderer.enemySets, geoBufferJson);
        gamePlayCanvas.SetActive(false);
        levelEdit.ClearEverything();
        balus.transform.position = new Vector3(0f, 0.5f, 0f);
        rb.position = new Vector3(0f, 0.5f, 0f);
        mainMenuNormalTile = Instantiate(levelRenderer.tileSets[1], new Vector3(0f, 0f, rb.position.z), Quaternion.identity);
        MainMenuScripts.instance.ResetMenu();
        mainMenuCanvas.SetActive(true);
        SetIsInMainMenu(true);
    }

    void Update()
    {
        if (!isDataDownloadedCache) {
            if (IsDataDownloaded())
            {
                isDataDownloaded = true;
                StopCoroutine(updateCacheCoroutine);
                isDataDownloadedCache = true;
            }
            else
            {
                isDataDownloaded = false;
                isDataDownloadedCache = true;
            }
        }
        isGamePaused = GFreeze.gamePaused;
        if (anims.Count == minAnimationCount) {
            BaseAnim[] baseAnims = FindObjectsByType<BaseAnim>(FindObjectsSortMode.None);
            foreach (BaseAnim baseAnim in baseAnims) {
                foreach (FrameAnim anim in baseAnim.animators) {
                    anims[anim.name.name] = anim;
                }
            }
        }

        if (isPlayingAnimation) {
            if (anims[currentlyPlayingAnimation].currentFrame >= anims[currentlyPlayingAnimation].frames.Count) {
                isPlayingAnimation = false;
            }
            PlayAnimation(currentlyPlayingAnimation);
        }

        if (isPlayingAnimationGroup) {
            if (animGroups[currentlyPlayingAnimationGroup][0].currentFrame >= animGroups[currentlyPlayingAnimationGroup][0].frames.Count) {
                isPlayingAnimationGroup = false;
            }
            PlayAnimationGroup(currentlyPlayingAnimationGroup);
        }

        if (isPlayingObjectAnimation) {
            if (currentlyPlayingObjectAnimation.animators[0].currentFrame >= currentlyPlayingObjectAnimation.animators[0].frames.Count) {
                isPlayingObjectAnimation = false;
                currentlyPlayingObjectAnimation.ResetAnimation(currentlyPlayingObjectAnimation.transform.position);
            }
            PlayObjectAnimation(currentlyPlayingObjectAnimation.name);
        }

        if (isDataDownloaded && !isGameOver && !isGamePaused)
        {
            //gre.enabled = true;
            //ere.enabled = true;
            levelRenderer.enabled = true;
            themeChanger.enabled = true;
            if (currentScene.name == "DebugScene")
            {
                //sphm.enabled = false;
                sphd.enabled = false;
            }

            if (sphm != null) sphm.enabled = true;
            if (sphd != null) sphd.enabled = true;

            GFreeze.enabled = true;
            isGamePaused = GFreeze.gamePaused;
            themeChanger2.enabled = true;
        }
        else if (isDataDownloaded && isGamePaused && !mainMenuScripts.isInitialized) {
            themeChanger.enabled = true;
            themeChanger2.enabled = true;
            mainMenuScripts.Initialize();
        }
        else
        {
            //gre.enabled = false;
            //ere.enabled = false;
            levelRenderer.enabled = false;
            //themeChanger.enabled = false;
            sphm.enabled = false;
            sphd.enabled = false;
            //themeChanger2.enabled = false;
        }
        if (isGameOver) {
            //gre.enabled = false;
            //ere.enabled = false;
            levelRenderer.enabled = false;
            themeChanger.enabled = false;
            sphm.enabled = false;
            sphd.enabled = false;
            themeChanger2.enabled = false;
            rb.velocity = Vector3.zero;
        }

        if (sphm != null && sphm.speed != levelConfig.levelSpeed) {
            //sphm.speed = levelConfig.levelSpeed;
        }

        float balusPercent = (balus.transform.position.z / (float)levelRenderer.positionsCount) * 100f;
        balusPercent = Mathf.Clamp(balusPercent, 0f, 100f);
        realPercent = Math.Round(balusPercent).ToString() + "%";
        percentTextMesh.SetText(realPercent);

        // Check if Balus falls under Y position 0
        if (balus.transform.position.y < 0f && !isGameOver && !isDeathDisabled && !delayed)
        {
            hasFallen = true;
            //Debug.Log(transform.position.y);
            GameOver(realPercent, true);
        }

        // audio management
        if (audioPlayer != null)
        {
            if (isInMainMenu)
            {
                audioPlayer.StopMusic();
            }
            else if (isGameOver || isGamePaused)
            {
                audioPlayer.PauseMusic();
            }
            else
            {
                audioPlayer.PlayMusic();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") && !isGameOver && !isDeathDisabled)
        {
            hasHitObstacle = true;
            GameOver(realPercent, true);
        } else if (other.gameObject.CompareTag("DiamondCollision") && !isGameOver) {
            GameObject diamondParent = other.gameObject.transform.parent.gameObject;
            SoundPlayer snd = diamondParent.GetComponent<SoundPlayer>();
            snd.PlayAudio();
            GameObject diamond1stChild = diamondParent.transform.GetChild(0).gameObject;
            if (diamond1stChild.activeSelf) {
                num_collectedGems++;
            }
            diamond1stChild.SetActive(false);
        } else if (other.gameObject.CompareTag("CrownCollision") && !isGameOver) {
            GameObject crownParent = other.gameObject.transform.parent.gameObject;
            SoundPlayer snd = crownParent.GetComponent<SoundPlayer>();
            snd.PlayAudio();
            GameObject crown1stChild = crownParent.transform.GetChild(0).gameObject;
            if (crown1stChild.activeSelf) {
                num_collectedCrowns++;
            }
            crown1stChild.SetActive(false);
        } else if (other.gameObject.CompareTag("MoverArrowCollision") && !isGameOver) {
            /*List<GameObject> movers = GameObject.FindGameObjectsWithTag("MoverCollisionGroup1").ToList();
            movers.AddRange(GameObject.FindGameObjectsWithTag("MoverCollisionGroup2"));
            movers.AddRange(GameObject.FindGameObjectsWithTag("MoverCollisionGroup3"));
            foreach (GameObject mover in movers) {
                if (mover.transform.position.x == other.transform.position.x && mover.transform.position.z == other.transform.position.z) {
                    Domino domino = mover.GetComponent<Domino>();
                    if (domino != null) {
                        domino.TriggerManualDomino();
                    }
                }
            }*/

            // TODO: Remove this once the mover dominoes are fixed
            int collisionX = (int)other.transform.position.x + 2;
            int collisionZ = (int)other.transform.position.z;
            int xDirection = 0;
            int zDirection = 0;
            GameObject moverArrowNormal = other.transform.parent.GetChild(1).gameObject;
            switch (moverArrowNormal.transform.rotation.eulerAngles.y) {
                case 0f:
                    xDirection = 0;
                    zDirection = 1;
                    break;
                case 90f:
                    xDirection = 1;
                    zDirection = 0;
                    break;
                case 180f:
                    xDirection = 0;
                    zDirection = -1;
                    break;
                case 270f:
                    xDirection = -1;
                    zDirection = 0;
                    break;
            }
            //Debug.Log("X: " + xDirection + " Z: " + zDirection);
            //Debug.Log(levelRenderer.levelVisuals[collisionZ][collisionX].Tile?.GetComponentInParent<ManagerDynamicGroups>() == null);
            //levelRenderer.levelVisuals[collisionZ][collisionX+1].Tile?.GetComponentInParent<ManagerDynamicGroups>()?.TriggerGroup(xDirection, zDirection, collisionZ, collisionX+1);
            var targetStorage = levelRenderer.levelVisuals[collisionZ][collisionX + 1];

            if (targetStorage != null && targetStorage.Tile != null)
            {
                var dynamicGroup = targetStorage.Tile.GetComponentInParent<ManagerDynamicGroups>();

                if (dynamicGroup != null)
                {
                    dynamicGroup.TriggerGroup(xDirection, zDirection, collisionZ, collisionX + 1);
                }
            }
        } else if (other.gameObject.CompareTag("LevelEnd") && !isGameOver) {
            rb.position = new Vector3(rb.position.x, 0.5f, rb.position.z - 0.1f);
            sphm.isNotFalling = true;
            GameOver(realPercent, false);
        }
    }

    public void SetIsInMainMenu(bool isInMainMenu)
    {
        this.isInMainMenu = isInMainMenu;
    }

    public void LoadLevel(string level) {
        //gre.jsonFilePath = "LevelData/Ground_" + level;
        //ere.jsonFilePath = "LevelData/Enemies_" + level;
        levelRenderer.jsonFilePath = "LevelData/Level_" + level;
        themeChanger.jsonFilePath = "LevelData/Themes_" + level;
        levelConfig.jsonFilePath = "LevelData/Config_" + level;
        levelConfig.LoadLevelConfig();
        geoBufferJsonFilePath = "LevelData/GeoBuffer_" + level + ".json";
        //string geoBufferJsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, geoBufferJsonFilePath));
        //Debug.Log(geoBufferJsonString);
        //geoBufferJson = JsonConvert.DeserializeObject<GeoBufferJson>(geoBufferJsonString);
        //objectPool.InitializePools(levelRenderer.tileSets, levelRenderer.enemySets, geoBufferJson);
        audioPlayer.audioPath = levelConfig.musicPath;
        audioPlayer.UpdateAudioClip();
        themeChanger.UpdateData();
        themeChanger.normalSpeed = levelConfig.levelSpeed;
        CFollow.enabled = true;
        //gre.enabled = true;
        //ere.enabled = true;
        levelRenderer.enabled = true;
        //ere.Initialize();
        //gre.Initialize();
        levelRenderer.Initialize();
        totalGemCount = levelRenderer.CountEnemies(18);
        totalCrownCount = levelRenderer.CountEnemies(28);
        num_collectedGems = 0;
        num_collectedCrowns = 0;
        themeChanger.enabled = true;
        themeChanger.themeID = 0;
        if (sphm != null) {
            sphm.speed = levelConfig.levelSpeed;
            sphm.enabled = false;
        }
        GFreeze.enabled = true;
        sphd.enabled = true;
        themeChanger2.enabled = true;
        themeChanger2.UpdateTheme(themeChanger.themeID);
        isGamePaused = GFreeze.gamePaused;
        gamePlayCanvas.SetActive(true);
        gameOverPanel.SetActive(false);
        Destroy(mainMenuNormalTile);
        if (levelConfig.startPortal) {
            balus.transform.position = new Vector3(0f, 0.5f, levelConfig.startPos);
            rb.position = new Vector3(0f, 0.5f, levelConfig.startPos);
        } else {
            balus.transform.position = new Vector3(0f, 0.5f, levelConfig.startPos);
            rb.position = new Vector3(0f, 0.5f, levelConfig.startPos);
        }
        levelEditButton.GetComponent<Button>().onClick.RemoveAllListeners();
        levelEditButton.GetComponent<Button>().onClick.AddListener(() => LoadLevelInEditor(level));
    }

    public void LoadLevelInEditor(string level) {
        //gre.jsonFilePath = "LevelData/Ground_" + level;
        //ere.jsonFilePath = "LevelData/Enemies_" + level;
        levelRenderer.jsonFilePath = "LevelData/Level_" + level;
        themeChanger.jsonFilePath = "LevelData/Themes_" + level;
        levelConfig.jsonFilePath = "LevelData/Config_" + level;
        levelConfig.LoadLevelConfig();
        geoBufferJsonFilePath = "LevelData/GeoBuffer_" + level + ".json";
        //string geoBufferJsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, geoBufferJsonFilePath));
        //geoBufferJson = JsonConvert.DeserializeObject<GeoBufferJson>(geoBufferJsonString);
        //objectPool.InitializePools(levelRenderer.tileSets, levelRenderer.enemySets, geoBufferJson);
        audioPlayer.audioPath = levelConfig.musicPath;
        audioPlayer.UpdateAudioClip();
        themeChanger.UpdateData();
        themeChanger.normalSpeed = levelConfig.levelSpeed;
        CFollow.enabled = true;
        //gre.enabled = true;
        //ere.enabled = true;
        levelRenderer.enabled = true;
        //ere.Initialize();
        //gre.Initialize();
        levelRenderer.Initialize();
        totalGemCount = levelRenderer.CountEnemies(18);
        totalCrownCount = levelRenderer.CountEnemies(28);
        num_collectedGems = 0;
        num_collectedCrowns = 0;
        themeChanger.enabled = true;
        themeChanger.themeID = 0;
        if (sphm != null) {
            sphm.speed = levelConfig.levelSpeed;
            sphm.enabled = false;
        }
        GFreeze.enabled = true;
        sphd.enabled = true;
        themeChanger2.enabled = true;
        isGamePaused = GFreeze.gamePaused;
        gamePlayCanvas.SetActive(true);
        gameOverPanel.SetActive(false);
        Destroy(mainMenuNormalTile);
        if (levelConfig.startPortal) {
            balus.transform.position = new Vector3(0f, 0.5f, levelConfig.startPos);
        } else {
            balus.transform.position = new Vector3(0f, 0.5f, levelConfig.startPos);
        }
        levelEditButton.GetComponent<Button>().onClick.RemoveAllListeners();
        levelEditButton.GetComponent<Button>().onClick.AddListener(() => LoadLevelInEditor(level));
        levelEndObject.transform.position = new Vector3(-90f, 0f, 0f);
        levelEdit.editorTransition();
    }

    public void CreateNewLevel(string levelID, string modPath = null) {
        if (modPath == null || modPath == "null") {
            StreamWriter configWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "LevelData", $"Config_{levelID}.json"));
            configWriter.Write(@"{""level_name"":""Unnamed Level"",""level_author"":""Anonymous"",""level_speed"":7.55,""start_pos"":0,""music_path"":""Music/Music1"",""worldshow_path"":null,""start_portal"":false}");
            configWriter.Close();
            StreamWriter levelWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "LevelData", $"Level_{levelID}.json"));
            levelWriter.Write(@"{""tiles"":[[0,0,0,0,0]],""enemies"":[[0,0,0,0,0]]}");
            levelWriter.Close();
            StreamWriter themeWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "LevelData", $"Themes_{levelID}.json"));
            themeWriter.Write(@"{""level_events"":[{""z_position"":0.0,""event_type"":""theme_change"",""event_fields"":{""theme_id"":0}}]}");
            themeWriter.Close();
            StreamWriter geoBufferWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "LevelData", $"GeoBuffer_{levelID}.json"));
            geoBufferWriter.Write(@"{""ground"":[210,210,210,210,210,210,210,210,210,210,210,210,210,210,210],""enemies"":[200,210,210,210,210,210,195,195,195,195,210,210,195,210,210,210,210,210,20,125,125,125,125,125,125,125,125,170]}");
            geoBufferWriter.Close();
        } else {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Mods", modPath))) {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Mods", modPath, "LevelData"));
            }
            StreamWriter configWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "Mods", modPath, "LevelData", $"Config_{levelID}.json"));
            configWriter.Write(@"{""level_name"":""Unnamed Level"",""level_author"":""Anonymous"",""level_speed"":7.55,""start_pos"":0,""music_path"":""Music/Music1"",""worldshow_path"":null,""start_portal"":false}");
            configWriter.Close();
            StreamWriter levelWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "Mods", modPath, "LevelData", $"Level_{levelID}.json"));
            levelWriter.Write(@"{""tiles"":[[0,0,0,0,0]],""enemies"":[[0,0,0,0,0]]}");
            levelWriter.Close();
            StreamWriter themeWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "Mods", modPath, "LevelData", $"Themes_{levelID}.json"));
            themeWriter.Write(@"{""level_events"":[{""z_position"":0.0,""event_type"":""theme_change"",""event_fields"":{""theme_id"":0}}]}");
            themeWriter.Close();
            StreamWriter geoBufferWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "Mods", modPath, "LevelData", $"GeoBuffer_{levelID}.json"));
            geoBufferWriter.Write(@"{""ground"":[210,210,210,210,210,210,210,210,210,210,210,210,210,210,210],""enemies"":[200,210,210,210,210,210,195,195,195,195,210,210,195,210,210,210,210,210,20,125,125,125,125,125,125,125,125,170]}");
            geoBufferWriter.Close();
        }
        LoadLevelInEditor(levelID);
    }

    public void SetDeathDisabled(bool isDisabled)
    {
        isDeathDisabled = isDisabled;
    }

    public void ShowSettingsPanel() {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel() {
        settingsPanel.SetActive(false);
    }

    public IEnumerator DelayGameOver(string percent, float delay) {
        delayed = true;
        yield return new WaitForSeconds(delay);
        GameOver(percent, true);
        delayed = false;
    }

    public void DecaySpeed(float multiplier) {
        sphm.speed /= multiplier;
    }

    public void PlayAnimation(string animationName) {
        isPlayingAnimation = true;
        anims[animationName].Play();
        if (currentlyPlayingAnimation != animationName) {
            currentlyPlayingAnimation = animationName;
        }
    }

    public void PlayAnimationGroup(string groupName)  {
        isPlayingAnimationGroup = true;
        if (animGroups.ContainsKey(groupName) == false) {
            return;
        }
        foreach (FrameAnim anim in animGroups[groupName]) {
            anim.Play();
        }
        if (currentlyPlayingAnimationGroup != groupName) {
            currentlyPlayingAnimationGroup = groupName;
        }
    }

    public void PlayObjectAnimation(string objectName) {
        BaseObject[] baseObjects = FindObjectsByType<BaseObject>(FindObjectsSortMode.None);
        foreach (BaseObject baseObject in baseObjects) {
            if (baseObject.name == objectName) {
                isPlayingObjectAnimation = true;
                foreach (FrameAnim anim in baseObject.animators) {
                    anim.Play();
                }
                if (currentlyPlayingObjectAnimation != baseObject) {
                    currentlyPlayingObjectAnimation = baseObject;
                }
                break;
            }
        }
    }

    public void ResetBaseObjectAnimation(string objectName, Vector3 position) {
        BaseObject[] baseObjects = FindObjectsByType<BaseObject>(FindObjectsSortMode.None);
        foreach (BaseObject baseObject in baseObjects) {
            if (baseObject.name == objectName) {
                baseObject.ResetAnimation(position, false);
                break;
            }
        }
    }

    public string GetPercentage() {
        float balusPercent = (balus.transform.position.z / (float)gre.positionsCount) * 100f;
        balusPercent = Mathf.Clamp(balusPercent, 0f, 100f);
        realPercent = Math.Round(balusPercent).ToString() + "%";
        percentTextMesh.SetText(realPercent);
        return realPercent;
    }

    public void GameOver(string percent, bool stopMusic)
    {
        isGameOver = true;
        //Time.timeScale = 0f;
        rb.velocity = Vector3.zero;
        gameOverPanel.SetActive(true);
        GameObject percentTextLabel2 = GameObject.Find("Percent2");
        TextMeshProUGUI percentTextMesh2 = percentTextLabel2.GetComponent<TextMeshProUGUI>();
        percentTextMesh2.SetText(percent);
        GameObject gemCountTextLabel = GameObject.Find("GemCountText");
        TextMeshProUGUI gemCountTextMesh = gemCountTextLabel.GetComponent<TextMeshProUGUI>();
        gemCountTextMesh.SetText($"{num_collectedGems}/{totalGemCount}");
        GameObject levelNameTextLabel = GameObject.Find("LevelName");
        TextMeshProUGUI levelNameTextMesh = levelNameTextLabel.GetComponent<TextMeshProUGUI>();
        levelNameTextMesh.SetText(levelConfig.levelName);
        GameObject levelAuthorTextLabel = GameObject.Find("LevelAuthor");
        TextMeshProUGUI levelAuthorTextMesh = levelAuthorTextLabel.GetComponent<TextMeshProUGUI>();
        levelAuthorTextMesh.SetText(levelConfig.levelAuthor);
        GameObject levelCrownsObject = gameOverPanel.transform.Find("LevelCrowns").gameObject;
        switch (totalCrownCount) {
            case 1:
                GameObject oneCrownObject = levelCrownsObject.transform.Find("OneCrown").gameObject;
                oneCrownObject.SetActive(true);
                break;
            case 2:
                GameObject twoCrownsObject = levelCrownsObject.transform.Find("TwoCrowns").gameObject;
                twoCrownsObject.SetActive(true);
                break;
            case 3:
                GameObject threeCrownsObject = levelCrownsObject.transform.Find("ThreeCrowns").gameObject;
                threeCrownsObject.SetActive(true);
                break;
            default:
                GameObject oneCrownObject2 = levelCrownsObject.transform.Find("OneCrown").gameObject;
                GameObject twoCrownsObject2 = levelCrownsObject.transform.Find("TwoCrowns").gameObject;
                GameObject threeCrownsObject2 = levelCrownsObject.transform.Find("ThreeCrowns").gameObject;
                oneCrownObject2.SetActive(false);
                twoCrownsObject2.SetActive(false);
                threeCrownsObject2.SetActive(false);
                break;
        }
        switch (num_collectedCrowns) {
            case 1:
                GameObject oneCrownObject = levelCrownsObject.transform.Find("OneCrown").gameObject;
                GameObject twoCrownsObject = levelCrownsObject.transform.Find("TwoCrowns").gameObject;
                GameObject threeCrownsObject = levelCrownsObject.transform.Find("ThreeCrowns").gameObject;
                if (oneCrownObject.activeSelf) {
                    GameObject crownFillObject = oneCrownObject.transform.Find("CrownFill").gameObject;
                    crownFillObject.SetActive(true);
                } else if (twoCrownsObject.activeSelf) {
                    GameObject crownFillObject = twoCrownsObject.transform.Find("CrownFill").gameObject;
                    crownFillObject.SetActive(true);
                } else if (threeCrownsObject.activeSelf) {
                    GameObject crownFillObject = threeCrownsObject.transform.Find("CrownFill").gameObject;
                    crownFillObject.SetActive(true);
                }
                break;
            case 2:
                GameObject twoCrownsObject2 = levelCrownsObject.transform.Find("TwoCrowns").gameObject;
                GameObject threeCrownsObject2 = levelCrownsObject.transform.Find("ThreeCrowns").gameObject;
                if (twoCrownsObject2.activeSelf) {
                    GameObject crownFillObject = twoCrownsObject2.transform.Find("CrownFill").gameObject;
                    crownFillObject.SetActive(true);
                    GameObject crownFill1Object = twoCrownsObject2.transform.Find("CrownFill1").gameObject;
                    crownFill1Object.SetActive(true);
                } else if (threeCrownsObject2.activeSelf) {
                    GameObject crownFillObject = threeCrownsObject2.transform.Find("CrownFill").gameObject;
                    crownFillObject.SetActive(true);
                    GameObject crownFill1Object = threeCrownsObject2.transform.Find("CrownFill1").gameObject;
                    crownFill1Object.SetActive(true);
                }
                break;
            case 3:
                GameObject threeCrownsObject3 = levelCrownsObject.transform.Find("ThreeCrowns").gameObject;
                if (threeCrownsObject3.activeSelf) {
                    GameObject crownFillObject = threeCrownsObject3.transform.Find("CrownFill").gameObject;
                    crownFillObject.SetActive(true);
                    GameObject crownFill1Object = threeCrownsObject3.transform.Find("CrownFill1").gameObject;
                    crownFill1Object.SetActive(true);
                    GameObject crownFill2Object = threeCrownsObject3.transform.Find("CrownFill2").gameObject;
                    crownFill2Object.SetActive(true);
                }
                break;
            default:
                if (totalCrownCount == 1) {
                    GameObject oneCrownObject3 = levelCrownsObject.transform.Find("OneCrown").gameObject;
                    GameObject crownFillObject = oneCrownObject3.transform.Find("CrownFill").gameObject;
                    crownFillObject.SetActive(false);
                } else if (totalCrownCount == 2) {
                    GameObject twoCrownsObject3 = levelCrownsObject.transform.Find("TwoCrowns").gameObject;
                    GameObject crownFillObject = twoCrownsObject3.transform.Find("CrownFill").gameObject;
                    crownFillObject.SetActive(false);
                    GameObject crownFill1Object = twoCrownsObject3.transform.Find("CrownFill1").gameObject;
                    crownFill1Object.SetActive(false);
                } else if (totalCrownCount == 3) {
                    GameObject threeCrownsObject4 = levelCrownsObject.transform.Find("ThreeCrowns").gameObject;
                    GameObject crownFillObject = threeCrownsObject4.transform.Find("CrownFill").gameObject;
                    crownFillObject.SetActive(false);
                    GameObject crownFill1Object = threeCrownsObject4.transform.Find("CrownFill1").gameObject;
                    crownFill1Object.SetActive(false);
                    GameObject crownFill2Object = threeCrownsObject4.transform.Find("CrownFill2").gameObject;
                    crownFill2Object.SetActive(false);
                }
                break;
        }
        gamePlayCanvas.SetActive(false);
        sphm.isJumping = true;
        sphm.isNotFalling = true;
        sphd.enabled = false;
        sphm.enabled = false;
        //CFollow.enabled = false;
        GFreeze.PauseGame(stopMusic);
        isGamePaused = GFreeze.gamePaused;
        //GFreeze.enabled = false;
        if (stopMusic) {
            audioPlayer.PauseAudio();
        }
        //rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
    }

    public void RestartGame()
    {
        sphm.isJumping = false;
        hasFallen = false;
        hasHitObstacle = false;
        if (!balus.activeSelf) {
            balus.SetActive(true);
        }
        //objectPool.ClearAllPools();
        num_collectedGems = 0;
        num_collectedCrowns = 0;
        //string jsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, geoBufferJsonFilePath));
        //geoBufferJson = JsonConvert.DeserializeObject<GeoBufferJson>(jsonString);
        //objectPool.InitializePools(levelRenderer.tileSets, levelRenderer.enemySets, geoBufferJson);
        BaseObject[] baseObjects = FindObjectsByType<BaseObject>(FindObjectsSortMode.None);
        foreach (BaseObject baseObject in baseObjects) {
            baseObject.ResetAnimation(baseObject.transform.position);
        }
        isPlayingAnimation = false;
        isPlayingAnimationGroup = false;
        isPlayingObjectAnimation = false;
        //sphm.enabled = false;
        //sphd.enabled = false;
        //CFollow.enabled = false;
        //gre.enabled = false;
        //ere.enabled = false;
        levelRenderer.enabled = false;
        gamePlayCanvas.SetActive(true);
        gameOverPanel.SetActive(false);
        Vector3 balusPos = levelConfig.startPortal ? new Vector3(0f, 0.5f, levelConfig.startPos) : new Vector3(0f, 0.5f, levelConfig.startPos);
        //gre.enabled = true;
        //ere.enabled = true;
        levelRenderer.enabled = true;
        levelRenderer.creationStep = 29;
        themeChanger.enabled = true;
        themeChanger.themeID = 0;
        themeChanger2.enabled = true;
        themeChanger2.UpdateTheme(themeChanger.themeID);
        levelEdit.ClearEverything();
        levelRenderer.Initialize();
        //gre.ClearPrefabPositions();
        //ere.ClearPrefabPositions();
        totalGemCount = levelRenderer.CountEnemies(18);
        totalCrownCount = levelRenderer.CountEnemies(28);
        GFreeze.enabled = true;
        //Time.timeScale = 1f;
        sphd.enabled = true;
        CFollow.enabled = true;
        audioPlayer.SeekToZero();
        //rb.velocity = Vector3.zero;
        rb.position = balusPos;
        balus.transform.position = balusPos;
        sphm.collisionZ = levelConfig.startPos + 0.5f;
        sphm.isJumping = true;
        sphm.isNotFalling = true;
        sphm.speed = levelConfig.levelSpeed;
        GFreeze.PauseGame();
        levelConfig.LoadLevelConfig();
        FilterManager.filterManager.DisableAll();
        GameObject endObject = GameObject.Find("End");
        endObject.transform.position = new Vector3(-90f, 0f, 0f);
        //rb.velocity = Vector3.zero;
        //rb.isKinematic = false;
        //rb.position = new Vector3(0f, 0.5f, 0f);
        //sphm.enabled = true;
        sphm.ClearFallingObstacles();
        isGameOver = false;
    }

    public void EnsureCorrectPosAfterRestart() {
        RestartGame();
        if (levelConfig.startPortal) {
            rb.position = new Vector3(0f, 0.55f, levelConfig.startPos);
            balus.transform.position = new Vector3(0f, 0.55f, levelConfig.startPos);
            sphm.isJumping = true;
            sphm.isNotFalling = true;
            if (!sphm.isNotFalling) {
                Debug.Log("Error");
            }
        } else {
            rb.position = new Vector3(0f, 0.5f, levelConfig.startPos);
            balus.transform.position = new Vector3(0f, 0.5f, levelConfig.startPos);
            if (rb.position != new Vector3(0f, 0.5f, levelConfig.startPos)) {
                Debug.Log("Error");
            }
        }
    }

    public int ConvertBoolToInt(bool b) {
        return b ? 1 : 0;
    }

    private IEnumerator LoadData()
    {
        if (!loadingPanel.activeSelf) loadingPanel.SetActive(true);
        Image progressImage = loadingPanel.transform.GetChild(2).GetComponent<Image>();

        void UpdateProgress()
        {
            if (totalDownloadsRequired > 0)
                progressImage.fillAmount = (float)downloadCount / (float)totalDownloadsRequired;
            else
                progressImage.fillAmount = 1f;
        }

        string[] folders = { "LevelData", "ThemeData", "Backgrounds", "Music", "WorldShow", "MenuData" };
        foreach (string folder in folders)
        {
            string path = Path.Combine(Application.persistentDataPath, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                downloadCount++;
                UpdateProgress();
                yield return null;
            }
        }

        // MY LEVEL 
        if (EnsureFile("LevelData/Config_mylevel", "LevelData/Config_mylevel.json")) downloadCount++;
        if (EnsureFile("LevelData/Level_mylevel", "LevelData/Level_mylevel.json")) downloadCount++;
        if (EnsureFile("LevelData/Themes_mylevel", "LevelData/Themes_mylevel.json")) downloadCount++;
        if (EnsureFile("LevelData/GeoBuffer_mylevel", "LevelData/GeoBuffer_mylevel.json")) downloadCount++;

        // VALEA
        if (EnsureFile("LevelData/Level_valea", "LevelData/Level_valea.json")) { downloadCount++; UpdateProgress(); yield return null; }
        if (EnsureFile("LevelData/Themes_valea", "LevelData/Themes_valea.json")) { downloadCount++; }
        if (EnsureFile("LevelData/Config_valea", "LevelData/Config_valea.json")) { downloadCount++; }
        if (EnsureFile("LevelData/GeoBuffer_valea", "LevelData/GeoBuffer_valea.json")) { downloadCount++; }

        // APERTA
        if (EnsureFile("LevelData/Level_aperta", "LevelData/Level_aperta.json")) { downloadCount++; UpdateProgress(); yield return null; }
        if (EnsureFile("LevelData/Themes_aperta", "LevelData/Themes_aperta.json")) { downloadCount++; }
        if (EnsureFile("LevelData/Config_aperta", "LevelData/Config_aperta.json")) { downloadCount++; }
        if (EnsureFile("LevelData/GeoBuffer_aperta", "LevelData/GeoBuffer_aperta.json")) { downloadCount++; }

        // GARDENEA 
        string[] gardeneaVars = { "older", "old", "new" };
        foreach (var v in gardeneaVars)
        {
            if (EnsureFile($"LevelData/Level_gardenea_{v}", $"LevelData/Level_gardenea_{v}.json")) downloadCount++;
            if (EnsureFile($"LevelData/Themes_gardenea_{v}", $"LevelData/Themes_gardenea_{v}.json")) downloadCount++;
            if (EnsureFile($"LevelData/Config_gardenea_{v}", $"LevelData/Config_gardenea_{v}.json")) downloadCount++;
            if (EnsureFile($"LevelData/GeoBuffer_gardenea_{v}", $"LevelData/GeoBuffer_gardenea_{v}.json")) downloadCount++;
        }
        UpdateProgress(); yield return null;

        // THEME DATA
        if (EnsureFile("ThemeData/ThemeData", "ThemeData/ThemeData.json")) { downloadCount++; UpdateProgress(); yield return null; }

        // BACKGROUNDS & ENEMIES & GENERAL (1-6)
        for (int i = 1; i <= 6; i++)
        {
            if (EnsureBinaryFile($"Backgrounds/Background{i}", $"Backgrounds/Background{i}.png", "png")) downloadCount++;
            if (EnsureBinaryFile($"Enemy{i}", $"Enemy{i}.png", "png")) downloadCount++;
            if (EnsureBinaryFile($"General{i}", $"General{i}.png", "png")) downloadCount++;
        }
        UpdateProgress(); yield return null;

        // MUSIC (1-3 + MyLevel)
        if (EnsureBinaryFile("Music/Music1", "Music/Music1.mp3", "mp3")) downloadCount++;
        if (EnsureBinaryFile("Music/Music2", "Music/Music2.mp3", "mp3")) downloadCount++;
        if (EnsureBinaryFile("Music/Music3", "Music/Music3.mp3", "mp3")) downloadCount++;

        // WORLD SHOW
        if (EnsureBinaryFile("WorldShow/World_valea", "WorldShow/World_valea.png", "png")) downloadCount++;
        if (EnsureBinaryFile("WorldShow/World_aperta", "WorldShow/World_aperta.png", "png")) downloadCount++;
        if (EnsureBinaryFile("WorldShow/World_gardenea", "WorldShow/World_gardenea.png", "png")) downloadCount++;
        if (EnsureBinaryFile("WorldShow/World_mylevel", "WorldShow/World_mylevel.png", "png")) downloadCount++;

        string menuJson = Resources.Load<TextAsset>("MenuData/MenuData").text;
        string menuPath = Path.Combine(Application.persistentDataPath, "MenuData/MenuData.json");

        File.WriteAllText(menuPath, menuJson);

        UpdateProgress();
        yield return null;

        isDataDownloaded = true;
        loadingPanel.SetActive(false);
    }

    /// <summary>
    /// Copy file tu Resources ra PersistentDataPath 
    /// Ham EnsureFile va EnsureBinaryFile giup rut gon ham LoadData lap di lap lai phan !File.Exists(destPath)
    /// </summary>
    /// <param name="resourcePath"></param>
    /// <param name="destRelativePath"></param>
    /// <param name="isBinary"></param>
    /// <returns></returns>
    private bool EnsureFile(string resourcePath, string destRelativePath, bool isBinary = false)
    {
        string destPath = Path.Combine(Application.persistentDataPath, destRelativePath);

        string dir = Path.GetDirectoryName(destPath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        if (!File.Exists(destPath))
        {
            if (isBinary)
            {
                return false;
            }
            else
            {
                TextAsset asset = Resources.Load<TextAsset>(resourcePath);
                if (asset != null)
                {
                    File.WriteAllText(destPath, asset.text);
                    return true;
                }
                else
                {
                    Debug.LogError($"[LoadData] Resource not found: {resourcePath}");
                }
            }
        }
        return false;
    }

    private bool EnsureBinaryFile(string resourcePath, string destRelativePath, string type)
    {
        string destPath = Path.Combine(Application.persistentDataPath, destRelativePath);
        string dir = Path.GetDirectoryName(destPath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        if (!File.Exists(destPath))
        {
            if (type == "png")
            {
                Texture2D tex = Resources.Load<Texture2D>(resourcePath);
                if (tex != null)
                {
                    File.WriteAllBytes(destPath, tex.EncodeToPNG());
                    return true;
                }
            }
            else if (type == "mp3")
            {
                AudioClip clip = Resources.Load<AudioClip>(resourcePath);
                if (clip != null)
                {
                    byte[] mp3Data = WavToMp3.ConvertWavToMp3(clip, 128);
                    File.WriteAllBytes(destPath, mp3Data);
                    return true;
                }
            }
        }
        return false;
    }

    private void EnsureFileExists(string resourcePath, string destFileName)
    {
        string destPath = Path.Combine(Application.persistentDataPath, destFileName);

        if (!File.Exists(destPath))
        {
            TextAsset asset = Resources.Load<TextAsset>(resourcePath);
            if (asset != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                File.WriteAllText(destPath, asset.text);
                Debug.Log($"file copied: {destFileName}");
            }
            else
            {
                Debug.LogError($"ERROR: can't find file '{resourcePath}' in Resources!");
            }
        }
    }
    //public void GenerateMyLongLevel()
    //{
    //    int targetSeconds = 120;
    //    float speed = 8.0f;
    //    int totalRows = (int)(targetSeconds * speed);

    //    NewLevelJson newLevel = new NewLevelJson();

    //    for (int i = 0; i < totalRows; i++)
    //    {
    //        newLevel.tiles.Add(new List<int> { 1, 1, 1, 1, 1 });

    //        newLevel.enemies.Add(new List<int> { 0, 0, 0, 0, 0 });
    //    }

    //    string json = JsonConvert.SerializeObject(newLevel, Formatting.Indented);

    //    string path = Path.Combine(Application.persistentDataPath, "LevelData/Level_mylevel.json");

    //    File.WriteAllText(path, json);
    //    Debug.Log($"da tao xong level dai {totalRows} tai: {path}");
    //}
}
