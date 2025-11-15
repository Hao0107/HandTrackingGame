using System;
using Microsoft.CSharp;
using UnityEngine;
using UnityEngine.UI;
using OpenRSR;
using OpenRSR.Modding;
using OpenRSR.Animation;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Dummiesman;
using NLua;
using Newtonsoft.Json;

[Serializable]
public class Vector3Json {
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class SpawnProperties {
    public Vector3Json position;
    public Vector3Json rotation;
    public Vector3Json scale;
}

[Serializable]
public class ObjectMetadata {
    public string name;
    public SpawnProperties spawn_properties;
    public string material;
}

public class ScriptLoader : MonoBehaviour
{
    string modFolderPath;
    public List<string> installedModPaths = new List<string>();
    //Lua state = new Lua();
    List<Lua> scriptStates = new List<Lua>();
    public GameManager gameManager;
    private int num_iterations = 0;
    //private CSharpScriptCompiler scriptCompiler = new CSharpScriptCompiler();
    // Start is called before the first frame update
    Func<IEnumerator, Coroutine> startCoroutine;
    void Start()
    {
        startCoroutine = (coroutine) => StartCoroutine(coroutine); 
        gameManager = GameManager.instance;
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Mods"))) {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Mods"));
        }
        modFolderPath = Path.Combine(Application.persistentDataPath, "Mods");
        installedModPaths = Directory.GetDirectories(modFolderPath).ToList();
        for (int i = 0; i < installedModPaths.Count; i++) {
            installedModPaths[i] = installedModPaths[i].Replace("\\", "/");
        }

        foreach (string modPath in installedModPaths) {
            string ScriptsFolderPath = Path.Combine(modPath, "Scripts");
            string MeshFolderPath = Path.Combine(modPath, "Meshes");
            string LevelDataFolderPath = Path.Combine(modPath, "LevelData");
            string _AppendFolderPath = Path.Combine(modPath, "_Append");
            if (Directory.Exists(MeshFolderPath)) {
                string[] files = Directory.GetFiles(MeshFolderPath);
                foreach (string file in files) {
                    if (file.EndsWith(".obj")) {
                        GameObject loadedObject = new OBJLoader().Load(file);
                        string fileWithoutExtension = Path.GetFileNameWithoutExtension(file);
                        if (File.Exists(Path.Combine(Application.persistentDataPath, modPath, "Meshes", fileWithoutExtension + "_Metadata.json"))) {
                            string objectJsonString = File.ReadAllText(Path.Combine(Application.persistentDataPath, modPath, "Meshes", fileWithoutExtension + "_Metadata.json"));
                            ObjectMetadata objectMetadata = JsonConvert.DeserializeObject<ObjectMetadata>(objectJsonString);
                            loadedObject.name = objectMetadata.name;
                            loadedObject.transform.position = new Vector3(objectMetadata.spawn_properties.position.x, objectMetadata.spawn_properties.position.y, objectMetadata.spawn_properties.position.z);
                            loadedObject.transform.rotation = Quaternion.Euler(objectMetadata.spawn_properties.rotation.x, objectMetadata.spawn_properties.rotation.y, objectMetadata.spawn_properties.rotation.z);
                            loadedObject.transform.localScale = new Vector3(objectMetadata.spawn_properties.scale.x, objectMetadata.spawn_properties.scale.y, objectMetadata.spawn_properties.scale.z);

                            MeshRenderer[] meshRenderers = loadedObject.GetComponentsInChildren<MeshRenderer>();
                            foreach (MeshRenderer meshRenderer in meshRenderers) {
                                if (meshRenderer != null) {
                                    meshRenderer.sharedMaterial = Resources.Load<Material>("Materials/" + objectMetadata.material);
                                }
                            }
                        }
                        if (File.Exists(Path.Combine(Application.persistentDataPath, modPath, "Animations", fileWithoutExtension + "_Animation.lua"))) {
                            loadedObject.AddComponent<BaseObject>();
                            BaseObject baseObject = loadedObject.GetComponent<BaseObject>();
                            Lua animationState = baseObject.objState;
                            animationState.LoadCLRPackage();
                            animationState["gameObject"] = loadedObject;
                            animationState.DoString("package.path = package.path .. ';" + Path.Combine(Application.persistentDataPath, modPath, "Scripts").Replace("\\", "/") + "/?.lua';");
                            animationState.DoFile(Path.Combine(Application.persistentDataPath, modPath, "Animations", fileWithoutExtension + "_Animation.lua"));
                            var initializeFunction = animationState["InitializeAnimation"] as LuaFunction;
                            if (initializeFunction != null) {
                                initializeFunction.Call();
                            }
                            var resetFunction = animationState["ResetAnimation"] as LuaFunction;
                            if (resetFunction != null) {
                                loadedObject.GetComponent<BaseObject>().resetAnimationFunction = resetFunction;
                                resetFunction.Call(loadedObject.transform.position);
                            }
                            LuaTable animTable = animationState["animators"] as LuaTable;
                            List<FrameAnim> animators = new List<FrameAnim>();
                            foreach (var animator in animTable.Values) {
                                LuaTable animatorTable = animator as LuaTable;
                                LuaTable framesTable = animatorTable["frames"] as LuaTable;
                                List<Frame> frames = new List<Frame>();
                                foreach (var frame in framesTable.Values) {
                                    LuaTable frameTable = frame as LuaTable;
                                    Vector3 position = (Vector3)frameTable["position"];
                                    Quaternion rotation = (Quaternion)frameTable["rotation"];
                                    Vector3 scale = (Vector3)frameTable["scale"];
                                    GameObject name = frameTable["name"] as GameObject;
                                    frames.Add(new Frame(position, rotation, scale, name));
                                }
                                FrameAnim frameAnim = new FrameAnim(frames);
                                animators.Add(frameAnim);
                            }
                            Debug.Log("Anims: " + animators.Count);
                            foreach (FrameAnim animator in animators) {
                                gameManager.anims[animator.name.name] = animator;
                            }
                            gameManager.minAnimationCount = animators.Count;
                            gameManager.animGroups[loadedObject.name] = animators;
                            loadedObject.GetComponent<BaseObject>().animators = animators;
                        }
                    }
                }
            }
            if (Directory.Exists(ScriptsFolderPath)) {
                string[] files = Directory.GetFiles(ScriptsFolderPath);
                foreach (string file in files) {
                    if (file.EndsWith(".lua")) {
                        Lua newState = new Lua();
                        newState["gameManager"] = gameManager;
                        newState["ball"] = gameManager.balus;
                        newState.LoadCLRPackage();
                        newState["StartCoroutine"] = startCoroutine;
                        newState.DoFile(file);
                        var initializeFunction = newState["Initialize"] as LuaFunction;
                        if (initializeFunction != null) {
                            initializeFunction.Call();
                        }
                        scriptStates.Add(newState);
                    }
                }
            }
            if (Directory.Exists(LevelDataFolderPath)) {
                gameManager.levelRenderer.levelFilePaths.Add(modPath);
                gameManager.levelConfig.levelFilePaths.Add(modPath);
                gameManager.themeChanger.levelFilePaths.Add(modPath);
            }
            if (Directory.Exists(_AppendFolderPath)) {
                string AppendMenuDataFolderPath = Path.Combine(_AppendFolderPath, "MenuData");
                if (Directory.Exists(AppendMenuDataFolderPath)) {
                    string[] appendFiles = Directory.GetFiles(AppendMenuDataFolderPath);
                    string menuDataJsonFile = appendFiles.FirstOrDefault(x => x.EndsWith("MenuData.json"));
                    if (menuDataJsonFile != null) {
                        string jsonString = File.ReadAllText(menuDataJsonFile);
                        AppendMenuDataJson menuData = JsonConvert.DeserializeObject<AppendMenuDataJson>(jsonString);
                        MainMenuScripts.instance.menuData.level_list.AddRange(menuData.level_list);
                    }
                }
            }
        }
    }

    public void AppendMenuData() {
        foreach (string modPath in installedModPaths) {
            string _AppendFolderPath = Path.Combine(modPath, "_Append");
            if (Directory.Exists(_AppendFolderPath)) {
                string AppendMenuDataFolderPath = Path.Combine(_AppendFolderPath, "MenuData");
                if (Directory.Exists(AppendMenuDataFolderPath)) {
                    string[] appendFiles = Directory.GetFiles(AppendMenuDataFolderPath);
                    string menuDataJsonFile = appendFiles.FirstOrDefault(x => x.EndsWith("MenuData.json"));
                    if (menuDataJsonFile != null) {
                        string jsonString = File.ReadAllText(menuDataJsonFile);
                        AppendMenuDataJson menuData = JsonConvert.DeserializeObject<AppendMenuDataJson>(jsonString);
                        MainMenuScripts.instance.menuData.level_list.AddRange(menuData.level_list);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager == null) {
            gameManager = GameManager.instance;
            foreach (Lua state in scriptStates) {
                state["gameManager"] = gameManager;
                state["ball"] = gameManager.balus;
                state["StartCoroutine"] = startCoroutine;
            }
        }

        foreach (Lua state in scriptStates) {
            if (gameManager.hasFallen) {
                var onFallenFunction = state["OnFallen"] as LuaFunction;
                if (onFallenFunction != null) {
                    if (state["number_iterations"] != null) {
                        double number_iterations = (double)state["number_iterations"];
                        if ((int)number_iterations == num_iterations) {
                            gameManager.hasFallen = false;
                            num_iterations = 0;
                        }
                        onFallenFunction.Call(number_iterations);
                    } else {
                        double number_iterations = 1;
                        if ((int)number_iterations == num_iterations) {
                            gameManager.hasFallen = false;
                            num_iterations = 0;
                        }
                        onFallenFunction.Call(number_iterations);
                    }
                    num_iterations++;
                    //gameManager.hasFallen = false;
                }
            }

            if (gameManager.hasHitObstacle) {
                var onObstacleHitFunction = state["OnObstacleHit"] as LuaFunction;
                if (onObstacleHitFunction != null) {
                    if (state["number_iterations"] != null) {
                        double number_iterations = (double)state["number_iterations"];
                        if ((int)number_iterations == num_iterations) {
                            gameManager.hasHitObstacle = false;
                            num_iterations = 0;
                        }
                        onObstacleHitFunction.Call(number_iterations);
                    } else {
                        double number_iterations = 1;
                        if ((int)number_iterations == num_iterations) {
                            gameManager.hasHitObstacle = false;
                            num_iterations = 0;
                        }
                        onObstacleHitFunction.Call(number_iterations);
                    }
                    num_iterations++;
                }
            }

            if (gameManager.isGameOver) {
                var onGameOverFunction = state["OnGameOver"] as LuaFunction;
                if (onGameOverFunction != null) {
                    onGameOverFunction.Call();
                }
            }

            var onUpdateFunction = state["OnUpdate"] as LuaFunction;
            if (onUpdateFunction != null) {
                onUpdateFunction.Call();
            }
        }
    }
}
