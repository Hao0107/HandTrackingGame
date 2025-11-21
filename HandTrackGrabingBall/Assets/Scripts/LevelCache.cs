using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelCache : MonoBehaviour
{
    string cacheDirectory = Path.Combine(Application.persistentDataPath, "level_cache");
    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(cacheDirectory))
        {
            Directory.CreateDirectory(cacheDirectory);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
