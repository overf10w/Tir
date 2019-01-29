using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceLoader : MonoBehaviour
{
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";

    public Stats stats;

    //[SerializeField]
    public PlayerData playerData;

    void Awake()
    {
        // TODO: dafuq?!
        #if UNITY_EDITOR
        stats = ReadSelf();
        playerData.InitStats(stats);
        playerData.InitPlayer();
        StartCoroutine(InitUI());
        #endif
        #if UNITY_STANDALONE
        stats = ReadSelf();
        playerData.InitStats(stats);
        playerData.InitPlayer();
        StartCoroutine(InitUI());
        #endif
    }

    public IEnumerator InitUI()
    {
        yield return null;
        playerData.InvokeWeaponChanged();
        playerData.InvokeLevelChanged();
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
        Stats save = playerData.GetData();
        string dataAsJson = JsonUtility.ToJson(save);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }

    public void Reset()
    {
        // Write resetted stats
        Stats resettedStats = new Stats();
        string dataAsJson = JsonUtility.ToJson(resettedStats);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

        playerData.ResetPlayerStats();
    }
}
