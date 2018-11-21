using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private string gameDataProjectFilePath = "/StreamingAssets/data.json";
	
	// Update is called once per frame
	void Update () {
		
	}

    public int kek;

    //[SerializeField]
    public PlayerDB playerDb;

    void Start()
    {
        #if UNITY_EDITOR
        ReadSelf();
        playerDb.UpdateCurrentSkills();
        #endif
        #if UNITY_STANDALONE
        ReadSelf();
        playerDb.UpdateCurrentSkills();
        #endif
    }

    // TODO: you'd better write this logic somewhere in monobehaviour,
    // as SO's OnDisable() not always called(?)
    void OnDisable()
    { 
        #if UNITY_STANDALONE
            WriteSelf();
        #endif
    }

    public void ReadSelf()
    {
        string dataAsJson = File.ReadAllText(Application.dataPath + gameDataProjectFilePath);
        Stats stats = JsonUtility.FromJson<Stats>(dataAsJson);
        if (stats != null)
        {
            playerDb.InitStats(stats);
        }
        else
        {
            Debug.Log("NULLLLLLLL =((((((((((((((((");
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
