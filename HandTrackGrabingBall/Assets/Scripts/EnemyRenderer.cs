using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

[System.Serializable]
public class EnemyPositionsData
{
    public List<List<int>> positions;

    public EnemyPositionsData(List<List<int>> positions) {
        this.positions = positions;
    }
}

public class EnemyRenderer : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    public string jsonFilePath;
    public float prefabSpacing = 1f;
    public float destroyDistance = 12f;

    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    private List<int> spawnedPrefabIDs = new List<int>();
    private GameObject balus;
    private Dictionary<Vector3, GameObject> prefabPositions = new Dictionary<Vector3, GameObject>();
    private GameManager gameManager;
    public string jsonString;
    private EnemyPositionsData data;
    public GameObject groundRotor;
    public GameObject glassRotor;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    private bool shouldSpawnPrefabCache = true;
    private ObjectPool objectPool;
    private SphereMovement sphm;
    private Scene currentScene;

    IEnumerator UpdateCache() {
        while (true)
        {
            // Update the cached value
            shouldSpawnPrefabCache = !shouldSpawnPrefabCache;

            // Wait for a few seconds before updating again
            yield return new WaitForSeconds(0.4f);
        }
    }

    private void Start()
    {
        Initialize();
        //StartCoroutine(UpdateCache());
    }

    private void Update()
    {
        //if (lastPosition.z < balus.transform.position.z) {
            //lastPosition = balus.transform.position;
        //}
        if (ShouldSpawnPrefab(lastPosition)) {
            UpdateEnemies();
        }
    }

    public void Initialize()
    {
        currentScene = SceneManager.GetActiveScene();
        balus = GameObject.FindGameObjectWithTag("Balus");
        sphm = balus.GetComponent<SphereMovement>();
        GameObject objPoolObject = GameObject.Find("ObjectPool");
        objectPool = objPoolObject.GetComponent<ObjectPool>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //Debug.Log(gameManager.isDataDownloaded);
        jsonString = gameManager.isDataDownloaded ? File.ReadAllText(Application.persistentDataPath + "/" + jsonFilePath + ".json") : Resources.Load<TextAsset>(jsonFilePath).text;
        data = JsonConvert.DeserializeObject<EnemyPositionsData>(jsonString);
        ProcessEnemyPositions(data.positions);
    }

    private void UpdateEnemies()
    {
        ProcessEnemyPositions(data.positions);
        CleanupSpawnedPrefabs();
    }

    public int CountEnemies(int id) {
        int count = 0;
        if (data != null) {
            for (int i = 0; i < data.positions.Count; i++) {
                for (int j = 0; j < data.positions[i].Count; j++) {
                    if (data.positions[i][j] == id) {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    private void ProcessEnemyPositions(List<List<int>> positions)
    {
        if (positions != null) {
            try {
            for (int i = (int)lastPosition.z; i < (int)lastPosition.z + 30; i++)
            {
                List<int> row = positions[i];
                float z = i * prefabSpacing;
                for (int j = 0; j < row.Count; j++)
                {
                    int hasPrefab = row[j];
                    float x = j - 2;
                    Vector3 spawnPosition = CalculateSpawnPosition(hasPrefab, x, z);
                    if (ShouldSpawnPrefab(spawnPosition))
                    {
                        SpawnPrefab(hasPrefab, spawnPosition, i, j, x, z);
                    } else {
                        lastPosition = balus.transform.position;
                    }
                }
            }
            } catch (System.ArgumentOutOfRangeException e) {
                for (int i = (int)lastPosition.z; i < positions.Count; i++)
                {
                    List<int> row = positions[i];
                    float z = i * prefabSpacing;
                    for (int j = 0; j < row.Count; j++)
                    {
                        int hasPrefab = row[j];
                        float x = j - 2;
                        Vector3 spawnPosition = CalculateSpawnPosition(hasPrefab, x, z);
                        if (ShouldSpawnPrefab(spawnPosition))
                        {
                            SpawnPrefab(hasPrefab, spawnPosition, i, j, x, z);
                        } else {
                            lastPosition = balus.transform.position;
                        }
                    }
                }
            }
        }
    }

    private Vector3 CalculateSpawnPosition(int hasPrefab, float x, float z)
    {
        Vector3 spawnPosition = new Vector3(x, 0.55f, z);
        if (hasPrefab == 4 || hasPrefab == 5 || (hasPrefab >= 10 && hasPrefab <= 15) || hasPrefab == 27)
        {
            spawnPosition = new Vector3(x, 0f, z);
        }
        else if (hasPrefab == 6 || hasPrefab == 7)
        {
            spawnPosition = new Vector3(x, 0.2f, z);
        }
        else if (hasPrefab == 8)
        {
            spawnPosition = new Vector3(x + 1f, 0.2f, z);
        }
        else if (hasPrefab == 9)
        {
            spawnPosition = new Vector3(x - 1f, 0.2f, z);
        } else if (hasPrefab == 19 || hasPrefab == 20 || hasPrefab == 21 || hasPrefab == 22 
        || hasPrefab == 23 || hasPrefab == 24 || hasPrefab == 25 || hasPrefab == 26) {
            spawnPosition = new Vector3(x, 0f, z);
        }
        return spawnPosition;
    }

    private bool ShouldSpawnPrefab(Vector3 spawnPosition)
    {
        return spawnPosition.z - balus.transform.position.z < 26 && !prefabPositions.ContainsKey(spawnPosition);
    }

    private void SpawnPrefab(int hasPrefab, Vector3 spawnPosition, int i, int j, float x, float z)
    {
        if (hasPrefab != 0) {
            GameObject spawnedPrefab = objectPool.GetPrefab(prefabs[hasPrefab], spawnPosition);
            GlassObject obstacleGlassObject = spawnedPrefab.GetComponent<GlassObject>();
            if (obstacleGlassObject != null) {
                obstacleGlassObject.fallCoefficient = 0.125f;
            }
            if (sphm != null) {
            if (sphm.fallingObstaclesGroup1.Contains(spawnedPrefab)) {
                sphm.fallingObstaclesGroup1.Remove(spawnedPrefab);
                sphm.fallingObstaclesGroup1.Clear();
            } else if (sphm.fallingObstaclesGroup2.Contains(spawnedPrefab)) {
                sphm.fallingObstaclesGroup2.Remove(spawnedPrefab);
                sphm.fallingObstaclesGroup2.Clear();
            } else if (sphm.fallingObstaclesGroup3.Contains(spawnedPrefab)) {
                sphm.fallingObstaclesGroup3.Remove(spawnedPrefab);
                sphm.fallingObstaclesGroup3.Clear();
            }
            if (sphm.fallingObstacles.Contains(spawnedPrefab)) {
                sphm.fallingObstacles.Remove(spawnedPrefab);
                while (sphm.fallingObstacles.Contains(spawnedPrefab)) {
                    sphm.fallingObstacles.Remove(spawnedPrefab);
                }
            }
            }
            if (spawnedPrefab.TryGetComponent<BaseAnim>(out BaseAnim baseAnim)) {
                //if (baseAnim == null) {
                    //goto SkipRiserAnim;
                //}
                baseAnim.ResetAnimation(spawnPosition);
            } else if (spawnedPrefab.TryGetComponent<SoundPlayer>(out SoundPlayer soundPlayer)) {
                GameObject gemBaseObject = spawnedPrefab.transform.GetChild(0).gameObject;
                gemBaseObject.SetActive(true);
                if (gemBaseObject.transform.position.x - spawnedPrefab.transform.position.x != 0) {
                    gemBaseObject.transform.position = new Vector3(spawnedPrefab.transform.position.x, gemBaseObject.transform.position.y, gemBaseObject.transform.position.z);
                }
                soundPlayer.SeekToZero();
            }
            SkipRiserAnim:
            spawnedPrefab.transform.position = spawnPosition;
            spawnedPrefabs.Add(spawnedPrefab);
            spawnedPrefabIDs.Add(hasPrefab);
            prefabPositions.Add(spawnPosition, spawnedPrefab);
            if (hasPrefab == 6 || hasPrefab == 7 || hasPrefab == 15) {
                GroundRenderer gre = GetComponent<GroundRenderer>();
                PositionsData gdata = gre.GetData();
                if (gdata.positions[i][j] == 3 
                || gdata.positions[i][j] == 4 
                || gdata.positions[i][j] == 5
                || gdata.positions[i][j] == 6) {
                    GameObject rotor = Instantiate(glassRotor, new Vector3(x, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                } else {
                    GameObject rotor = Instantiate(groundRotor, new Vector3(x, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                }
            } else if (hasPrefab == 8) {
                GroundRenderer gre = GetComponent<GroundRenderer>();
                PositionsData gdata = gre.GetData();
                if (gdata.positions[i][j] == 3 
                || gdata.positions[i][j] == 4 
                || gdata.positions[i][j] == 5
                || gdata.positions[i][j] == 6) {
                    GameObject rotor = Instantiate(glassRotor, new Vector3(x + 1f, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                } else {
                    GameObject rotor = Instantiate(groundRotor, new Vector3(x + 1f, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                }
            } else if (hasPrefab == 9) {
                GroundRenderer gre = GetComponent<GroundRenderer>();
                PositionsData gdata = gre.GetData();
                if (gdata.positions[i][j] == 3 
                || gdata.positions[i][j] == 4 
                || gdata.positions[i][j] == 5
                || gdata.positions[i][j] == 6) {
                    GameObject rotor = Instantiate(glassRotor, new Vector3(x - 1f, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                } else {
                    GameObject rotor = Instantiate(groundRotor, new Vector3(x - 1f, 0f, z), Quaternion.identity);
                    rotor.transform.SetParent(spawnedPrefab.transform);
                }
            } else if (hasPrefab == 16) {
                if (spawnedPrefab.TryGetComponent<LeftRollerAnim>(out LeftRollerAnim lr)) {
                    lr.xOffset = x;
                }
            } else if (hasPrefab == 17) {
                if (spawnedPrefab.TryGetComponent<RightRollerAnim>(out RightRollerAnim rr)) {
                    rr.xOffset = x;
                }
            } else if (hasPrefab == 19 || hasPrefab == 20 || hasPrefab == 21 || hasPrefab == 22
            || hasPrefab == 23 || hasPrefab == 24 || hasPrefab == 25 || hasPrefab == 26) {
                GameObject activeMover = spawnedPrefab.transform.GetChild(0).gameObject;
                GameObject normalMover = spawnedPrefab.transform.GetChild(1).gameObject;
                GameObject moverCollision = spawnedPrefab.transform.GetChild(2).gameObject;
                normalMover.SetActive(true);
                activeMover.SetActive(false);
                moverCollision.SetActive(true);
                if (hasPrefab == 19 || hasPrefab == 20 || hasPrefab == 21 || hasPrefab == 22) {
                    List<GameObject> movers = GameObject.FindGameObjectsWithTag("MoverCollisionGroup1").ToList();
                    movers.AddRange(GameObject.FindGameObjectsWithTag("MoverCollisionGroup2").ToList());
                    movers.AddRange(GameObject.FindGameObjectsWithTag("MoverCollisionGroup3").ToList());
                    foreach (GameObject mover in movers) {
                        if (mover.transform.position.x == spawnedPrefab.transform.position.x && mover.transform.position.z == spawnedPrefab.transform.position.z) {
                            //spawnedPrefab.transform.parent = mover.transform;
                            Domino domino = mover.GetComponent<Domino>();
                            domino.isGroupLeader = true;
                            break;
                        }
                    }
                } else if (hasPrefab == 23 || hasPrefab == 24 || hasPrefab == 25 || hasPrefab == 26) {
                    List<GameObject> movers = GameObject.FindGameObjectsWithTag("MoverAutoCollisionGroup1").ToList();
                    movers.AddRange(GameObject.FindGameObjectsWithTag("MoverAutoCollisionGroup2").ToList());
                    movers.AddRange(GameObject.FindGameObjectsWithTag("MoverAutoCollisionGroup3").ToList());
                    foreach (GameObject mover in movers) {
                        if (mover.transform.position.x == spawnedPrefab.transform.position.x && mover.transform.position.z == spawnedPrefab.transform.position.z) {
                            //spawnedPrefab.transform.parent = mover.transform;
                            Domino domino = mover.GetComponent<Domino>();
                            domino.isGroupLeader = true;
                            break;
                        }
                    }
                }
            }
            lastPosition = balus.transform.position;
        }
    }

    private void CleanupSpawnedPrefabs()
    {
        for (int i = 0; i < spawnedPrefabs.Count; i++)
        {
            GameObject spawnedPrefab = spawnedPrefabs[i];
            if (spawnedPrefab == null || balus.transform.position.z - spawnedPrefab.transform.position.z >= destroyDistance)
            {
                if (spawnedPrefab != null)
                {
                    objectPool.ReturnPrefab(prefabs[spawnedPrefabIDs[i]], spawnedPrefab);
                }
                spawnedPrefabs.RemoveAt(i);
                spawnedPrefabIDs.RemoveAt(i);
            }
        }
    }

    public void UpdateData()
    {
        jsonString = File.ReadAllText(Application.persistentDataPath + "/" + jsonFilePath + ".json");
        data = JsonConvert.DeserializeObject<EnemyPositionsData>(jsonString);
    }

    public EnemyPositionsData GetData()
    {
        return JsonConvert.DeserializeObject<EnemyPositionsData>(jsonString);
    }

    public void ClearPrefabPositions()
    {
        prefabPositions.Clear();
        spawnedPrefabs.Clear();
        spawnedPrefabIDs.Clear();
        lastPosition = new Vector3(0f, 0f, 0f);
    }
}