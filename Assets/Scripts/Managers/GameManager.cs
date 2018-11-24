using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";

    public Stats stats;

    //[SerializeField]
    public PlayerDB playerDb;

    void Awake()
    {
        // TODO: dafuq?!
        #if UNITY_EDITOR
        stats = ReadSelf();
        playerDb.InitStats(stats);
        playerDb.InitPlayer();
        #endif
        #if UNITY_STANDALONE
        stats = ReadSelf();
        playerDb.InitStats(stats);
        playerDb.InitPlayer();
        #endif
    }

    void OnDisable()
    { 
        #if UNITY_STANDALONE
            WriteSelf();
        #endif
    }

    // TODO: try catch stats == null;
    public Stats ReadSelf()
    {
        string dataAsJson = File.ReadAllText(Application.dataPath + gameDataProjectFilePath);
        stats = JsonUtility.FromJson<Stats>(dataAsJson);
        if (stats != null)
        {
            return stats;
        }
        else
        {
            return null;
        }
    }

    public void WriteSelf()
    {
        Stats save = playerDb.ReturnStats();
        string dataAsJson = JsonUtility.ToJson(save);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        Debug.Log("DataAsJson: " + dataAsJson);
        File.WriteAllText(filePath, dataAsJson);
    }

    public void Reset()
    {
        // Write resetted stats
        Stats resettedStats = new Stats();
        string dataAsJson = JsonUtility.ToJson(resettedStats);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

        playerDb.ResetPlayerStats();
    }
}
