using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Networking;

[System.Serializable]
public class MyColor {
    public int r;
    public int g;
    public int b;
    public int a;
    public MyColor(int r, int g, int b, int a) {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color ToUnityColor() {
        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }
}

[System.Serializable]
public class ThemeJson {
    public string background_path;
    public string enemy_path;
    public string general_path;
    public MyColor ball_color;
    public MyColor gem_color;
}

public class ThemeChanger : MonoBehaviour
{
    public class Theme {
        public Texture2D background;
        public Texture2D enemy;
        public Texture2D general;
        public Color ballColor;
        public Color gemColor;

        public Theme(Texture2D background, Texture2D enemy, Texture2D general, Color ballColor, Color gemColor) {
            this.background = background;
            this.enemy = enemy;
            this.general = general;
            this.ballColor = ballColor;
            this.gemColor = gemColor;
        }
    }
    public List<Theme> themes = new List<Theme>();
    public string jsonFilePath;
    public int themeID;
    private GameManager gameManager;
    private string jsonString;
    private GameObject balus;
    private bool shouldUpdateTheme = true;
    
    private void Start()
    {
        balus = GameObject.Find("Balus");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager.isDataDownloaded) {
            jsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, jsonFilePath + ".json"));
            List<ThemeJson> data = JsonConvert.DeserializeObject<List<ThemeJson>>(jsonString);
            foreach(ThemeJson theme in data) {
                string backgroundPath = Path.Combine(Application.persistentDataPath, theme.background_path + ".png");
                string enemyPath = Path.Combine(Application.persistentDataPath, theme.enemy_path + ".png");
                string generalPath = Path.Combine(Application.persistentDataPath, theme.general_path + ".png");
                Color ballColor = theme.ball_color.ToUnityColor();
                Color gemColor = theme.gem_color.ToUnityColor();
                byte[] backgroundData = File.ReadAllBytes(backgroundPath);
                byte[] enemyData = File.ReadAllBytes(enemyPath);
                byte[] generalData = File.ReadAllBytes(generalPath);
                Texture2D background = new Texture2D(1024, 1024);
                Texture2D enemy = new Texture2D(1024, 1024);
                Texture2D general = new Texture2D(1024, 1024);
                background.LoadImage(backgroundData);
                enemy.LoadImage(enemyData);
                general.LoadImage(generalData);
                Theme m_theme = new Theme(background, enemy, general, ballColor, gemColor);
                themes.Add(m_theme);
            }
        } else {
            jsonString = Resources.Load<TextAsset>(jsonFilePath).text;
            List<ThemeJson> data = JsonConvert.DeserializeObject<List<ThemeJson>>(jsonString);
            foreach(ThemeJson theme in data) {
                string backgroundPath = theme.background_path;
                string enemyPath = theme.enemy_path;
                string generalPath = theme.general_path;
                Color ballColor = theme.ball_color.ToUnityColor();
                Color gemColor = theme.gem_color.ToUnityColor();
                Texture2D background = Resources.Load<Texture2D>(backgroundPath);
                Texture2D enemy = Resources.Load<Texture2D>(enemyPath);
                Texture2D general = Resources.Load<Texture2D>(generalPath);
                Theme m_theme = new Theme(background, enemy, general, ballColor, gemColor);
                themes.Add(m_theme);
            }
        }
    }

    public void UpdateTheme(int themeID) {
        shouldUpdateTheme = true;
        this.themeID = themeID;
    }

    private void Update() {
        if (shouldUpdateTheme) {
            Theme currentTheme = themes[themeID];
            Renderer balusRenderer = balus.GetComponent<Renderer>();
            if (balusRenderer != null) {
                balusRenderer.sharedMaterial.color = currentTheme.ballColor;
            }
            Material gemMaterial = GameObject.Find("DeceBalus_Gem_Prefab").transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial;
            gemMaterial.color = currentTheme.gemColor;
            Material backgroundMaterial = GameObject.Find("Background").GetComponent<Renderer>().sharedMaterial;
            backgroundMaterial.mainTexture = currentTheme.background;
            Material enemyMaterial = GameObject.Find("Base_2").GetComponent<Renderer>().sharedMaterial;
            enemyMaterial.mainTexture = currentTheme.enemy;
            Material generalMaterial = GameObject.Find("DeceBalus_Normal_Tile").transform.GetChild(1).gameObject.GetComponent<Renderer>().sharedMaterial;
            generalMaterial.mainTexture = currentTheme.general;
            shouldUpdateTheme = false;
        }
    }
}
