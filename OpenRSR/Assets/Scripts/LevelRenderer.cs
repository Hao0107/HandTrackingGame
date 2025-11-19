using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

[System.Serializable]
public class NewLevelJson
{
    public List<List<int>> tiles = new List<List<int>>();
    public List<List<int>> enemies = new List<List<int>>();
}

public class LevelRenderer : MonoBehaviour
{
    [System.Serializable]
    public class Storage
    {
        public GameObject Tile;
        public GameObject Enemy;

        public Storage Copy()
        {
            return new Storage
            {
                Tile = Tile,
                Enemy = Enemy
            };
        }
    }

    public List<List<Storage>> levelVisuals = new List<List<Storage>>();

    [Header("Assets Configuration")]
    public List<GameObject> tileSets = new List<GameObject>();
    public List<GameObject> enemySets = new List<GameObject>();

    [Header("Level Settings")]
    public string jsonFilePath;
    public string jsonString;
    public NewLevelJson data;
    public List<List<bool>> checkedTiles = new List<List<bool>>();
    public int creationStep = 29;
    public int positionsCount;

    [Header("References")]
    private GameObject balus;
    public GameObject groundRotor;
    public GameObject glassRotor;
    public GameObject reference;
    public GameObject glassEdge;
    public GameObject moverEdge;
    public GameObject moverAutoEdge;
    public GameObject smallCollision;
    public GameObject tallCollision;
    public GameObject airCollision;
    public GameObject baseBlock;

    void Start()
    {
        balus = GameObject.Find("Balus");

        // If GameManager hasn't initialized data, load it manually (useful for testing)
        if (data == null || data.tiles.Count == 0)
        {
            UpdateData();
             //Initialize(); // Uncomment if you need immediate rendering in Start
        }
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.isGamePaused)
        {
            UpdateTile();
        }
    }

    // --- FIXED LOAD DATA FUNCTION ---
    public void UpdateData()
    {
        string persistentPath = Path.Combine(Application.persistentDataPath, jsonFilePath + ".json");

        // 1. Priority: Read from PersistentDataPath (Device Storage)
        if (File.Exists(persistentPath))
        {
            jsonString = File.ReadAllText(persistentPath);
        }
        // 2. Fallback: Read from Resources (Editor or first run)
        else
        {
            TextAsset asset = Resources.Load<TextAsset>(jsonFilePath);
            if (asset != null)
            {
                jsonString = asset.text;
            }
            else
            {
                Debug.LogError($"[LevelRenderer] File not found: {jsonFilePath}");
                return;
            }
        }

        try
        {
            data = JsonConvert.DeserializeObject<NewLevelJson>(jsonString);
            positionsCount = data.tiles.Count;
            checkedTiles.Clear();
            levelVisuals.Clear();

            for (int i = 0; i < positionsCount; i++)
            {
                checkedTiles.Add(new List<bool>() { false, false, false, false, false });
                levelVisuals.Add(new List<Storage>(new Storage[7] { new Storage(), new Storage(), new Storage(), new Storage(), new Storage(), new Storage(), new Storage() }));
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[LevelRenderer] JSON Parse Error: {e.Message}");
        }
    }
    // ---------------------------------------

    private void UpdateTile()
    {
        if (balus == null) return;

        if ((float)creationStep >= (float)positionsCount)
        {
            GameObject endObject = GameObject.Find("End");
            if (endObject != null && endObject.transform.position != new Vector3(0f, 0f, positionsCount))
            {
                endObject.transform.position = new Vector3(0f, 0f, positionsCount);
            }
            return;
        }
        if ((float)creationStep >= (float)positionsCount || !(balus.transform.position.z + 29f >= (float)creationStep))
        {
            return;
        }
        creationStep++;
        if (creationStep < positionsCount)
        {
            for (int i = 0; i < 5; i++)
            {
                GenerateStaticTile(i, creationStep);
            }
        }
    }

    public void Initialize()
    {
        UpdateData();
        if (data == null) return;

        int startPosition = 0;
        if (GameManager.instance != null && GameManager.instance.levelConfig != null)
        {
            startPosition = GameManager.instance.levelConfig.startPos;
        }
        int initialRenderLimit = startPosition + 40;
        if (initialRenderLimit > positionsCount) initialRenderLimit = positionsCount;

        creationStep = Mathf.Max(creationStep, initialRenderLimit - 1);
        for (int i = 0; i < initialRenderLimit; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if ((float)i < positionsCount)
                {
                    GenerateStaticTile(j, i);
                }
            }
        }
    }

    private void GenerateStaticTile(int x, int z)
    {
        if (!checkedTiles[z][x])
        {
            int tileID = data.tiles[z][x];
            int enemyID = data.enemies[z][x];

            // --- SAFETY CHECKS ---
            if (tileID < 0 || tileID >= tileSets.Count)
            {
                // ID out of range, skip to prevent crash
                return;
            }
            if (enemyID < 0 || enemyID >= enemySets.Count)
            {
                return;
            }
            // ---------------------

            GameObject tile = tileSets[tileID];
            GameObject enemy = enemySets[enemyID];

            // Instantiate Objects
            GameObject spawnTile = Instantiate(tile, new Vector3(x - 2f, 0f, z), Quaternion.identity);
            GameObject spawnEnemy = Instantiate(enemy, new Vector3(x - 2f, 0f, z), Quaternion.identity);

            levelVisuals[z][x + 1].Tile = spawnTile;
            levelVisuals[z][x + 1].Enemy = spawnEnemy;

            // --- GLASS EDGE LOGIC ---
            if (tileID == 4 || tileID == 5 || tileID == 6)
            {
                if (x < 4 && data.tiles[z][x + 1] != tileID)
                {
                    SpawnEdge(spawnTile, new Vector3(x - 2f + 0.45f, 0.1f, z), Quaternion.Euler(0f, 90f, 0f));
                }
                if (x > 0 && data.tiles[z][x - 1] != tileID)
                {
                    SpawnEdge(spawnTile, new Vector3(x - 2f - 0.45f, 0.1f, z), Quaternion.Euler(0f, 90f, 0f));
                }
                if (x == 4)
                {
                    SpawnEdge(spawnTile, new Vector3(x - 2f + 0.45f, 0.1f, z), Quaternion.Euler(0f, 90f, 0f));
                }
                if (x == 0)
                {
                    SpawnEdge(spawnTile, new Vector3(x - 2f - 0.45f, 0.1f, z), Quaternion.Euler(0f, 90f, 0f));
                }
                if (z < positionsCount - 1 && data.tiles[z + 1][x] != tileID)
                {
                    SpawnEdge(spawnTile, new Vector3(x - 2f, 0.1f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                }
                if (z > 0 && data.tiles[z - 1][x] != tileID)
                {
                    SpawnEdge(spawnTile, new Vector3(x - 2f, 0.1f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                }
                if (z == positionsCount - 1)
                {
                    SpawnEdge(spawnTile, new Vector3(x - 2f, 0.1f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                }
                if (z == 0)
                {
                    SpawnEdge(spawnTile, new Vector3(x - 2f, 0.1f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                }
            }

            // --- MOVING TILE LOGIC ---
            if (tileID == 7)
            {
                LeftMovingTileAnim lma = spawnTile.GetComponent<LeftMovingTileAnim>();
                if (lma)
                {
                    lma.xOffset = (float)x - 2f;
                    lma.m_Riser = spawnEnemy;
                    if (data.tiles[z].All(v => v == 7) || data.tiles[z][0] == 7) lma.leftMostXOffset = -2f;
                }
            }
            else if (tileID == 8)
            {
                RightMovingTileAnim rma = spawnTile.GetComponent<RightMovingTileAnim>();
                if (rma)
                {
                    rma.xOffset = (float)x - 2f;
                    rma.m_Riser = spawnEnemy;
                    if (data.tiles[z].All(v => v == 8) || data.tiles[z][4] == 8) rma.rightMostXOffset = -2f;
                }
            }
            else if (tileID >= 9 && tileID <= 14)
            {
                // Mover Logic
                levelVisuals[z][x + 1].Tile = null;
                Destroy(spawnTile);
                levelVisuals[z][x + 1].Enemy = null;
                Destroy(spawnEnemy);
                GenerateMoverBase(x, z, tileID);
            }

            UpdateEnemy(x, z, spawnTile, spawnEnemy);

            // Handle empty ID (0)
            if (tileID == 0) levelVisuals[z][x + 1].Tile = null;
            if (enemyID == 0) levelVisuals[z][x + 1].Enemy = null;
        }
    }

    // Helper function to spawn edge
    private void SpawnEdge(GameObject parent, Vector3 pos, Quaternion rot)
    {
        if (glassEdge == null) return;
        GameObject edge = Instantiate(glassEdge, pos, rot);
        edge.transform.parent = parent.transform;
    }

    public void UpdateEnemy(int x, int z, GameObject spawnTile, GameObject spawnEnemy)
    {
        if (!checkedTiles[z][x])
        {
            int tileID = data.tiles[z][x];
            int enemyID = data.enemies[z][x];

            // Add Destroy script
            if (spawnTile)
            {
                DestroyObj TileDestroyer = spawnTile.AddComponent<DestroyObj>();
                if (GameManager.instance != null && GameManager.instance.balus != null)
                    TileDestroyer.progressPos = GameManager.instance.balus.transform;
                TileDestroyer.deletePos = -12f;
            }
            if (spawnEnemy)
            {
                DestroyObj EnemyDestroyer = spawnEnemy.AddComponent<DestroyObj>();
                if (GameManager.instance != null && GameManager.instance.balus != null)
                    EnemyDestroyer.progressPos = GameManager.instance.balus.transform;
                EnemyDestroyer.deletePos = -12f;
            }

            // Logic for Rotors (6, 7, 8, 9, 15)
            bool isGlassTile = (tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6);
            GameObject rotorPrefab = isGlassTile ? glassRotor : groundRotor;

            if ((enemyID >= 6 && enemyID <= 9) || enemyID == 15)
            {
                float xOffset = 0;
                if (enemyID == 8) xOffset = 1f;
                if (enemyID == 9) xOffset = -1f;

                GameObject rotor = Instantiate(rotorPrefab, new Vector3(x - 2f + xOffset, 0f, z), Quaternion.identity);
                rotor.transform.parent = spawnEnemy.transform;

                // Assign rotor to moving tile
                if (!isGlassTile)
                {
                    if (tileID == 7)
                    {
                        LeftMovingTileAnim lma = spawnTile.GetComponent<LeftMovingTileAnim>();
                        if (lma) lma.rotorObject = rotor;
                    }
                    else if (tileID == 8)
                    {
                        RightMovingTileAnim rma = spawnTile.GetComponent<RightMovingTileAnim>();
                        if (rma) rma.rotorObject = rotor;
                    }
                }
            }

            // Logic for Roller
            if (enemyID == 16)
            {
                LeftRollerAnim lra = spawnEnemy.GetComponent<LeftRollerAnim>();
                if (lra) lra.xOffset = x - 2f;
            }
            else if (enemyID == 17)
            {
                RightRollerAnim rra = spawnEnemy.GetComponent<RightRollerAnim>();
                if (rra) rra.xOffset = x - 2f;
            }

            // Logic for Mover Auto (23-26)
            if (enemyID >= 23 && enemyID <= 26 && spawnTile != null)
            {
                Transform parentTransform = spawnTile.transform.parent;
                if (parentTransform != null)
                {
                    ManagerDynamicGroups manager = parentTransform.GetComponent<ManagerDynamicGroups>();
                    if (manager != null && manager.groupType == ManagerDynamicGroups.GroupType.moverAuto)
                    {
                        if (enemyID == 23) { manager.autoX = 0; manager.autoY = 1; }
                        else if (enemyID == 24) { manager.autoX = 1; manager.autoY = 0; }
                        else if (enemyID == 25) { manager.autoX = 0; manager.autoY = -1; }
                        else if (enemyID == 26) { manager.autoX = -1; manager.autoY = 0; }

                        manager.moverActivator = spawnEnemy.GetComponent<MoverVisual>();
                    }
                }
            }

            // Logic for Blocks (33, 34)
            if (enemyID == 33 || enemyID == 34)
            {
                GameObject col = Instantiate(smallCollision, new Vector3(x - 2f, 0.55f, z), Quaternion.identity);
                col.transform.parent = spawnEnemy.transform;

                GameObject block1 = Instantiate(baseBlock, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                block1.transform.parent = spawnEnemy.transform;
                BlockAnim anim1 = block1.GetComponent<BlockAnim>();

                if (enemyID == 33)
                {
                    if (anim1) { anim1.yOffset = 0f; anim1.dyOffset = 1f; }
                }
                else
                {
                    if (anim1) { anim1.yOffset = -1f; anim1.dyOffset = 2f; }

                    GameObject block2 = Instantiate(baseBlock, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                    block2.transform.parent = spawnEnemy.transform;
                    BlockAnim anim2 = block2.GetComponent<BlockAnim>();
                    if (anim2) { anim2.yOffset = 0f; anim2.dyOffset = 2f; }
                }
            }
        }
    }

    public void GenerateMoverBase(int x, int z, int tileID)
    {
        byte b = 3;
        if (tileID < 12) b = 2;

        GameObject gameObject = Instantiate(reference, new Vector3(0f, 0f, z), Quaternion.identity);
        ManagerDynamicGroups manager = gameObject.AddComponent<ManagerDynamicGroups>();
        manager.levelRenderer = this;
        manager.groupType = (b == 3) ? ManagerDynamicGroups.GroupType.moverAuto : ManagerDynamicGroups.GroupType.mover;

        InstantiateMoverPieces(x, z, tileID, gameObject, b);
    }

    public void InstantiateMoverPieces(int x, int z, int tileID, GameObject basePivot, byte b)
    {
        // Safety Check
        if (tileID < 0 || tileID >= tileSets.Count) return;
        int enemyID = data.enemies[z][x];
        if (enemyID < 0 || enemyID >= enemySets.Count) return;

        GameObject groundObject = Instantiate(tileSets[tileID], new Vector3(x - 2f, 0f, z), Quaternion.identity);
        groundObject.transform.parent = basePivot.transform;
        levelVisuals[z][x + 1].Tile = groundObject;

        GameObject enemyObject = Instantiate(enemySets[enemyID], new Vector3(x - 2f, 0f, z), Quaternion.identity);
        enemyObject.transform.parent = groundObject.transform;
        levelVisuals[z][x + 1].Enemy = enemyObject;

        UpdateEnemy(x, z, groundObject, enemyObject);
        checkedTiles[z][x] = true;

        // Recursive check for neighbors
        CheckAndSpawnMoverNeighbor(x + 1, z, tileID, basePivot, b, 1, 0); // Right
        CheckAndSpawnMoverNeighbor(x - 1, z, tileID, basePivot, b, -1, 0); // Left
        CheckAndSpawnMoverNeighbor(x, z + 1, tileID, basePivot, b, 0, 1); // Up
        CheckAndSpawnMoverNeighbor(x, z - 1, tileID, basePivot, b, 0, -1); // Down

        // Spawn Mover Edge
        SpawnMoverEdge(x, z, tileID, groundObject, b);
    }

    // Recursive helper for Mover
    private void CheckAndSpawnMoverNeighbor(int targetX, int targetZ, int tileID, GameObject basePivot, byte b, int dx, int dz)
    {
        if (targetX >= 0 && targetX < 5 && targetZ >= 0 && targetZ < positionsCount)
        {
            if (!checkedTiles[targetZ][targetX] && data.tiles[targetZ][targetX] == tileID)
            {
                InstantiateMoverPieces(targetX, targetZ, tileID, basePivot, b);
            }
        }
    }

    private void SpawnMoverEdge(int x, int z, int tileID, GameObject parent, byte b)
    {
        GameObject edgePrefab = (b == 3) ? moverAutoEdge : moverEdge;
        if (edgePrefab == null) return;

        // Right
        if (x == 4 || (x < 4 && !checkedTiles[z][x + 1] && data.tiles[z][x + 1] != tileID))
        {
            Instantiate(edgePrefab, new Vector3(x - 2f + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f)).transform.parent = parent.transform;
        }
        // Left
        if (x == 0 || (x > 0 && !checkedTiles[z][x - 1] && data.tiles[z][x - 1] != tileID))
        {
            Instantiate(edgePrefab, new Vector3(x - 2f - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f)).transform.parent = parent.transform;
        }
        // Up
        if (z == positionsCount - 1 || (z < positionsCount - 1 && !checkedTiles[z + 1][x] && data.tiles[z + 1][x] != tileID))
        {
            Instantiate(edgePrefab, new Vector3(x - 2f, 0f, z + 0.45f), Quaternion.identity).transform.parent = parent.transform;
        }
        // Down
        if (z == 0 || (z > 0 && !checkedTiles[z - 1][x] && data.tiles[z - 1][x] != tileID))
        {
            Instantiate(edgePrefab, new Vector3(x - 2f, 0f, z - 0.45f), Quaternion.identity).transform.parent = parent.transform;
        }
    }

    public int CountTiles(int id)
    {
        if (data == null) return 0;
        int count = 0;
        foreach (var row in data.tiles)
        {
            count += row.Count(t => t == id);
        }
        return count;
    }

    public int CountEnemies(int id)
    {
        if (data == null) return 0;
        int count = 0;
        foreach (var row in data.enemies)
        {
            count += row.Count(e => e == id);
        }
        return count;
    }

    public NewLevelJson GetData()
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
                Debug.LogWarning($"[LevelThemeChanger] Can't find '{jsonFilePath}'. Create Empty data.");
                NewLevelJson emptyData = new NewLevelJson();
                for (int i = 0; i < 10; i++)
                {
                    emptyData.tiles.Add(new List<int> { 0, 0, 0, 0, 0 });
                    emptyData.enemies.Add(new List<int> { 0, 0, 0, 0, 0 });
                }
                return emptyData;
            }
        }

        try
        {
            return JsonConvert.DeserializeObject<NewLevelJson>(jsonString);
        }
        catch
        {
            return new NewLevelJson();
        }
    }
}