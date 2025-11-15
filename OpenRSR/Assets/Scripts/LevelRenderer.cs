using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

[System.Serializable]
public class NewLevelJson {
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
    public List<GameObject> tileSets = new List<GameObject>();
    public List<GameObject> enemySets = new List<GameObject>();
    public string jsonFilePath;
    public string jsonString;
    public NewLevelJson data;
    public List<List<bool>> checkedTiles = new List<List<bool>>();
    public int creationStep = 29;
    public int positionsCount;
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
    public List<string> levelFilePaths = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        levelFilePaths.Add(Application.persistentDataPath);
        string correctFilePath = levelFilePaths.FirstOrDefault(x => File.Exists(x + "/" + jsonFilePath + ".json"));
        if (correctFilePath == null) {
            Debug.LogError("File not found");
            return;
        }
        jsonString = File.ReadAllText(correctFilePath + "/" + jsonFilePath + ".json");
        data = JsonConvert.DeserializeObject<NewLevelJson>(jsonString);
        positionsCount = data.tiles.Count;
        for (int i = 0; i < positionsCount; i++) {
            checkedTiles.Add(new List<bool>() { false, false, false, false, false });
            levelVisuals.Add(new List<Storage>( new Storage[7] { new Storage(), new Storage(), new Storage(), new Storage(), new Storage(), new Storage(), new Storage()} ));
        }
        balus = GameObject.Find("Balus");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGamePaused) {
            UpdateTile();
        }
    }

    private void UpdateTile()
	{
		if ((float)creationStep >= (float)positionsCount) {
            GameObject endObject = GameObject.Find("End");
            if (endObject.transform.position != new Vector3(0f, 0f, positionsCount)) {
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
			//CreateCapsule(creationStep);
		}
	}

    public void Initialize()
	{
		//Debug.Log("Initialize");
        UpdateData();
        for (int i = 0; i <= 29; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if ((float)i < positionsCount)
				{
					GenerateStaticTile(j, i);
                    //Debug.Log("GenerateStaticTile " + j + " " + i);
				}
			}
			//CreateCapsule(i);
		}
	}

    private void GenerateStaticTile(int x, int z) {
        if (!checkedTiles[z][x]) {
            int tileID = data.tiles[z][x];
            int enemyID = data.enemies[z][x];
            GameObject tile = tileSets[tileID];
            GameObject enemy = enemySets[enemyID];
            GameObject spawnTile = Instantiate(tile, new Vector3(x - 2f, 0f, z), Quaternion.identity);
            GameObject spawnEnemy = Instantiate(enemy, new Vector3(x - 2f, 0f, z), Quaternion.identity);
            levelVisuals[z][x+1].Tile = spawnTile;
            levelVisuals[z][x+1].Enemy = spawnEnemy;
            if (tileID == 4 || tileID == 5 || tileID == 6) {
                if (x < 4 && data.tiles[z][x + 1] != tileID) {
                    GameObject spawnedEdge = Instantiate(glassEdge, new Vector3(x - 2f + 0.45f, 0.1f, z), Quaternion.Euler(0f, 90f, 0f));
                    spawnedEdge.transform.parent = spawnTile.transform;
                }
                if (x > 0 && data.tiles[z][x - 1] != tileID) {
                    GameObject spawnedEdge = Instantiate(glassEdge, new Vector3(x - 2f - 0.45f, 0.1f, z), Quaternion.Euler(0f, 90f, 0f));
                    spawnedEdge.transform.parent = spawnTile.transform;
                }
                if (x == 4) {
                    GameObject spawnedEdge = Instantiate(glassEdge, new Vector3(x - 2f + 0.45f, 0.1f, z), Quaternion.Euler(0f, 90f, 0f));
                    spawnedEdge.transform.parent = spawnTile.transform;
                }
                if (x == 0) {
                    GameObject spawnedEdge = Instantiate(glassEdge, new Vector3(x - 2f - 0.45f, 0.1f, z), Quaternion.Euler(0f, 90f, 0f));
                    spawnedEdge.transform.parent = spawnTile.transform;
                }
                if (z < positionsCount - 1 && data.tiles[z + 1][x] != tileID) {
                    GameObject spawnedEdge = Instantiate(glassEdge, new Vector3(x - 2f, 0.1f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    spawnedEdge.transform.parent = spawnTile.transform;
                }
                if (z > 0 && data.tiles[z - 1][x] != tileID) {
                    GameObject spawnedEdge = Instantiate(glassEdge, new Vector3(x - 2f, 0.1f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    spawnedEdge.transform.parent = spawnTile.transform;
                }
                if (z == positionsCount - 1) {
                    GameObject spawnedEdge = Instantiate(glassEdge, new Vector3(x - 2f, 0.1f, z + 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    spawnedEdge.transform.parent = spawnTile.transform;
                }
                if (z == 0) {
                    GameObject spawnedEdge = Instantiate(glassEdge, new Vector3(x - 2f, 0.1f, z - 0.45f), Quaternion.Euler(0f, 0f, 0f));
                    spawnedEdge.transform.parent = spawnTile.transform;
                }
            }
            if (tileID == 7) {
                LeftMovingTileAnim lma = spawnTile.GetComponent<LeftMovingTileAnim>();
                lma.xOffset = (float)x - 2f;
                lma.m_Riser = spawnEnemy;
                if (data.tiles[z].All(x => x == 7)) {
                    lma.leftMostXOffset = -2f;
                }
                if (data.tiles[z][0] == 7) {
                    lma.leftMostXOffset = -2f;
                }
            } else if (tileID == 8) {
                RightMovingTileAnim rma = spawnTile.GetComponent<RightMovingTileAnim>();
                rma.xOffset = (float)x - 2f;
                rma.m_Riser = spawnEnemy;
                if (data.tiles[z].All(x => x == 8)) {
                    rma.rightMostXOffset = -2f;
                }
                if (data.tiles[z][4] == 8) {
                    rma.rightMostXOffset = -2f;
                }
            } else if (tileID >= 9 && tileID <= 14) {
                levelVisuals[z][x+1].Tile = null;
                Destroy(spawnTile);
                levelVisuals[z][x+1].Enemy = null;
                Destroy(spawnEnemy);
                GenerateMoverBase(x, z, tileID);
            }
            UpdateEnemy(x, z, spawnTile, spawnEnemy);
            if (tileID == 0) {
                levelVisuals[z][x+1].Tile = null;
            }
            if (enemyID == 0) {
                levelVisuals[z][x+1].Enemy = null;
            }
        }
    }

    public void UpdateEnemy(int x, int z, GameObject spawnTile, GameObject spawnEnemy) {
        if (!checkedTiles[z][x]) {
            int tileID = data.tiles[z][x];
            int enemyID = data.enemies[z][x];
            DestroyObj TileDestroyer = spawnTile.AddComponent<DestroyObj>();
            TileDestroyer.progressPos = GameManager.instance.balus.transform;
            TileDestroyer.deletePos = -12f;
            DestroyObj EnemyDestroyer = spawnEnemy.AddComponent<DestroyObj>();
            EnemyDestroyer.progressPos = GameManager.instance.balus.transform;
            EnemyDestroyer.deletePos = -12f;
            if (enemyID == 6 && (tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(glassRotor, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
            } else if (enemyID == 6 && !(tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(groundRotor, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
                if (tileID == 7) {
                    LeftMovingTileAnim lma = spawnTile.GetComponent<LeftMovingTileAnim>();
                    lma.rotorObject = instantiatedRotor;
                } else if (tileID == 8) {
                    RightMovingTileAnim rma = spawnTile.GetComponent<RightMovingTileAnim>();
                    rma.rotorObject = instantiatedRotor;
                }
            } else if (enemyID == 7 && (tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(glassRotor, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
            } else if (enemyID == 7 && !(tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(groundRotor, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
                if (tileID == 7) {
                    LeftMovingTileAnim lma = spawnTile.GetComponent<LeftMovingTileAnim>();
                    lma.rotorObject = instantiatedRotor;
                } else if (tileID == 8) {
                    RightMovingTileAnim rma = spawnTile.GetComponent<RightMovingTileAnim>();
                    rma.rotorObject = instantiatedRotor;
                }
            } else if (enemyID == 8 && (tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(glassRotor, new Vector3(x - 1f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
            } else if (enemyID == 8 && !(tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(groundRotor, new Vector3(x - 1f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
                if (tileID == 7) {
                    LeftMovingTileAnim lma = spawnTile.GetComponent<LeftMovingTileAnim>();
                    lma.rotorObject = instantiatedRotor;
                } else if (tileID == 8) {
                    RightMovingTileAnim rma = spawnTile.GetComponent<RightMovingTileAnim>();
                    rma.rotorObject = instantiatedRotor;
                }
            } else if (enemyID == 9 && (tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(glassRotor, new Vector3(x - 3f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
            } else if (enemyID == 9 && !(tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(groundRotor, new Vector3(x - 3f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
                if (tileID == 7) {
                    LeftMovingTileAnim lma = spawnTile.GetComponent<LeftMovingTileAnim>();
                    lma.rotorObject = instantiatedRotor;
                } else if (tileID == 8) {
                    RightMovingTileAnim rma = spawnTile.GetComponent<RightMovingTileAnim>();
                    rma.rotorObject = instantiatedRotor;
                }
            } else if (enemyID == 15 && (tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(glassRotor, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
            } else if (enemyID == 15 && !(tileID == 3 || tileID == 4 || tileID == 5 || tileID == 6)) {
                GameObject instantiatedRotor = Instantiate(groundRotor, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                instantiatedRotor.transform.parent = spawnEnemy.transform;
                if (tileID == 7) {
                    LeftMovingTileAnim lma = spawnTile.GetComponent<LeftMovingTileAnim>();
                    lma.rotorObject = instantiatedRotor;
                } else if (tileID == 8) {
                    RightMovingTileAnim rma = spawnTile.GetComponent<RightMovingTileAnim>();
                    rma.rotorObject = instantiatedRotor;
                }
            }
            if (enemyID == 16) {
                LeftRollerAnim lra = spawnEnemy.GetComponent<LeftRollerAnim>();
                lra.xOffset = x - 2f;
            } else if (enemyID == 17) {
                RightRollerAnim rra = spawnEnemy.GetComponent<RightRollerAnim>();
                rra.xOffset = x - 2f;
            }
            if (enemyID == 23 || enemyID == 24 || enemyID == 25 || enemyID == 26) {
                ManagerDynamicGroups managerDynamicGroups = spawnTile.transform.parent.GetComponent<ManagerDynamicGroups>();
                if (managerDynamicGroups != null && managerDynamicGroups.groupType == ManagerDynamicGroups.GroupType.moverAuto) {
                    if (enemyID == 23) {
                        managerDynamicGroups.autoX = 0;
                        managerDynamicGroups.autoY = 1;
                    } else if (enemyID == 24) {
                        managerDynamicGroups.autoX = 1;
                        managerDynamicGroups.autoY = 0;
                    } else if (enemyID == 25) {
                        managerDynamicGroups.autoX = 0;
                        managerDynamicGroups.autoY = -1;
                    } else if (enemyID == 26) {
                        managerDynamicGroups.autoX = -1;
                        managerDynamicGroups.autoY = 0;
                    }
                    managerDynamicGroups.moverActivator = spawnEnemy.GetComponent<MoverVisual>();
                }
            }
            if (enemyID == 33) {
                GameObject instantiatedCollision = Instantiate(smallCollision, new Vector3(x - 2f, 0.55f, z), Quaternion.identity);
                instantiatedCollision.transform.parent = spawnEnemy.transform;
                GameObject instantiatedBlock = Instantiate(baseBlock, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                instantiatedBlock.transform.parent = spawnEnemy.transform;
                BlockAnim blockAnim = instantiatedBlock.GetComponent<BlockAnim>();
                blockAnim.yOffset = 0f;
                blockAnim.dyOffset = 1f;
            }
            if (enemyID == 34) {
                GameObject instantiatedCollision = Instantiate(smallCollision, new Vector3(x - 2f, 0.55f, z), Quaternion.identity);
                instantiatedCollision.transform.parent = spawnEnemy.transform;
                GameObject instantiatedBlock = Instantiate(baseBlock, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                instantiatedBlock.transform.parent = spawnEnemy.transform;
                BlockAnim blockAnim = instantiatedBlock.GetComponent<BlockAnim>();
                blockAnim.yOffset = -1f;
                blockAnim.dyOffset = 2f;
                GameObject instantiatedBlock2 = Instantiate(baseBlock, new Vector3(x - 2f, 0f, z), Quaternion.identity);
                instantiatedBlock2.transform.parent = spawnEnemy.transform;
                BlockAnim blockAnim2 = instantiatedBlock2.GetComponent<BlockAnim>();
                blockAnim2.yOffset = 0f;
                blockAnim2.dyOffset = 2f;
            }
        }
    }

    public void GenerateMoverBase(int x, int z, int tileID) {
        byte b = 3;
        if (tileID < 12) {
            b = 2;
        }
        GameObject gameObject = Instantiate(reference, new Vector3(0f, 0f, z), Quaternion.identity);
        ManagerDynamicGroups managerDynamicGroups = gameObject.AddComponent<ManagerDynamicGroups>();
        managerDynamicGroups.levelRenderer = this;
        if (b == 3) {
            managerDynamicGroups.groupType = ManagerDynamicGroups.GroupType.moverAuto;
        } else if (b == 2) {
            managerDynamicGroups.groupType = ManagerDynamicGroups.GroupType.mover;
        }
        //checkedTiles[z][x] = true;
        InstantiateMoverPieces(x, z, tileID, gameObject, b);
    }

    public void InstantiateMoverPieces(int x, int z, int tileID, GameObject basePivot, byte b) {
        GameObject groundObject = Instantiate(tileSets[tileID], new Vector3(x - 2f, 0f, z), Quaternion.identity);
        groundObject.transform.parent = basePivot.transform;
        levelVisuals[z][x+1].Tile = groundObject;
        GameObject enemyObject = Instantiate(enemySets[data.enemies[z][x]], new Vector3(x - 2f, 0f, z), Quaternion.identity);
        enemyObject.transform.parent = groundObject.transform;
        levelVisuals[z][x+1].Enemy = enemyObject;
        UpdateEnemy(x, z, groundObject, enemyObject);
        checkedTiles[z][x] = true;
        if (x < 4 && !checkedTiles[z][x+1] && data.tiles[z][x+1] == tileID) {
            InstantiateMoverPieces(x + 1, z, tileID, basePivot, b);
        } else if (x < 4 && !checkedTiles[z][x+1] && data.tiles[z][x+1] != tileID) {
            if (b == 2) {
                GameObject spawnedEdge = Instantiate(moverEdge, new Vector3(x - 2f + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                spawnedEdge.transform.parent = groundObject.transform;
            } else if (b == 3) {
                GameObject spawnedEdge = Instantiate(moverAutoEdge, new Vector3(x - 2f + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                spawnedEdge.transform.parent = groundObject.transform;
            }
        }
        if (x > 0 && !checkedTiles[z][x-1] && data.tiles[z][x-1] == tileID) {
            InstantiateMoverPieces(x - 1, z, tileID, basePivot, b);
        } else if (x > 0 && !checkedTiles[z][x-1] && data.tiles[z][x-1] != tileID) {
            if (b == 2) {
                GameObject spawnedEdge = Instantiate(moverEdge, new Vector3(x - 2f - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                spawnedEdge.transform.parent = groundObject.transform;
            } else if (b == 3) {
                GameObject spawnedEdge = Instantiate(moverAutoEdge, new Vector3(x - 2f - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                spawnedEdge.transform.parent = groundObject.transform;
            }
        }
		if (z < positionsCount - 1 && !checkedTiles[z+1][x] && data.tiles[z+1][x] == tileID)
		{
			InstantiateMoverPieces(x, z + 1, tileID, basePivot, b);
		} else if (z < positionsCount - 1 && !checkedTiles[z+1][x] && data.tiles[z+1][x] != tileID) {
            if (b == 2) {
                GameObject spawnedEdge = Instantiate(moverEdge, new Vector3(x - 2f, 0f, z + 0.45f), Quaternion.identity);
                spawnedEdge.transform.parent = groundObject.transform;
            } else if (b == 3) {
                GameObject spawnedEdge = Instantiate(moverAutoEdge, new Vector3(x - 2f, 0f, z + 0.45f), Quaternion.identity);
                spawnedEdge.transform.parent = groundObject.transform;
            }
        }
		if (z > 0 && !checkedTiles[z-1][x] && data.tiles[z-1][x] == tileID)
		{
			InstantiateMoverPieces(x, z - 1, tileID, basePivot, b);
		} else if (z > 0 && !checkedTiles[z-1][x] && data.tiles[z-1][x] != tileID) {
            if (b == 2) {
                GameObject spawnedEdge = Instantiate(moverEdge, new Vector3(x - 2f, 0f, z - 0.45f), Quaternion.identity);
                spawnedEdge.transform.parent = groundObject.transform;
            } else if (b == 3) {
                GameObject spawnedEdge = Instantiate(moverAutoEdge, new Vector3(x - 2f, 0f, z - 0.45f), Quaternion.identity);
                spawnedEdge.transform.parent = groundObject.transform;
            }
        }
        if (x == 0) {
            if (b == 2) {
                GameObject spawnedEdge = Instantiate(moverEdge, new Vector3(x - 2f - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                spawnedEdge.transform.parent = groundObject.transform;
            } else if (b == 3) {
                GameObject spawnedEdge = Instantiate(moverAutoEdge, new Vector3(x - 2f - 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                spawnedEdge.transform.parent = groundObject.transform;
            }
        } else if (x == 4) {
            if (b == 2) {
                GameObject spawnedEdge = Instantiate(moverEdge, new Vector3(x - 2f + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                spawnedEdge.transform.parent = groundObject.transform;
            } else if (b == 3) {
                GameObject spawnedEdge = Instantiate(moverAutoEdge, new Vector3(x - 2f + 0.45f, 0f, z), Quaternion.Euler(0f, 90f, 0f));
                spawnedEdge.transform.parent = groundObject.transform;
            }
        }
        if (z == positionsCount - 1) {
            if (b == 2) {
                GameObject spawnedEdge = Instantiate(moverEdge, new Vector3(x - 2f, 0f, z - 0.45f), Quaternion.identity);
                spawnedEdge.transform.parent = groundObject.transform;
            } else if (b == 3) {
                GameObject spawnedEdge = Instantiate(moverAutoEdge, new Vector3(x - 2f, 0f, z - 0.45f), Quaternion.identity);
                spawnedEdge.transform.parent = groundObject.transform;
            }
        } else if (z == 0) {
            if (b == 2) {
                GameObject spawnedEdge = Instantiate(moverEdge, new Vector3(x - 2f, 0f, z + 0.45f), Quaternion.identity);
                spawnedEdge.transform.parent = groundObject.transform;
            } else if (b == 3) {
                GameObject spawnedEdge = Instantiate(moverAutoEdge, new Vector3(x - 2f, 0f, z + 0.45f), Quaternion.identity);
                spawnedEdge.transform.parent = groundObject.transform;
            }
        }
    }

    public void UpdateData() {
        if (!levelFilePaths.Contains(Application.persistentDataPath)) {
            levelFilePaths.Add(Application.persistentDataPath);
        }
        string correctFilePath = levelFilePaths.FirstOrDefault(x => File.Exists(x + "/" + jsonFilePath + ".json"));
        if (correctFilePath == null) {
            Debug.LogError("File not found: " + jsonFilePath + ".json");
            return;
        }
        jsonString = File.ReadAllText(correctFilePath + "/" + jsonFilePath + ".json");
        data = JsonConvert.DeserializeObject<NewLevelJson>(jsonString);
        positionsCount = data.tiles.Count;
        checkedTiles.Clear();
        levelVisuals.Clear();
        for (int i = 0; i < positionsCount; i++) {
            checkedTiles.Add(new List<bool>() { false, false, false, false, false });
            levelVisuals.Add(new List<Storage>( new Storage[7] { new Storage(), new Storage(), new Storage(), new Storage(), new Storage(), new Storage(), new Storage()} ));
        }
    }

    public int CountTiles(int id) {
        int count = 0;
        for (int i = 0; i < data.tiles.Count; i++) {
            for (int j = 0; j < data.tiles[i].Count; j++) {
                if (data.tiles[i][j] == id) {
                    count++;
                }
            }
        }
        return count;
    }

    public int CountEnemies(int id) {
        int count = 0;
        for (int i = 0; i < data.enemies.Count; i++) {
            for (int j = 0; j < data.enemies[i].Count; j++) {
                if (data.enemies[i][j] == id) {
                    count++;
                }
            }
        }
        return count;
    }

    public NewLevelJson GetData() {
        return JsonConvert.DeserializeObject<NewLevelJson>(jsonString);
    }
}
