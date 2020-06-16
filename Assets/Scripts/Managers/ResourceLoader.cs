using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game
{
    // TODO (only prior final build): make it singleton
    public class ResourceLoader : MonoBehaviour
    {
        public static ResourceLoader Instance;

        private string playerDataProjectFilePath = "/StreamingAssets/data.json";
        private string gameDataProjectFilePath = "/StreamingAssets/gameData.json";

        //public PlayerStats playerStats;

        //public PlayerModel playerData;

        //public GameStats gameStats;

        //public GameData gameData;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("More than one ResourceLoader is in the scene");
            }
            else
            {
                Instance = this;
            }

            //// TODO: dafuq?!
            //#if UNITY_EDITOR
            //    playerStats = ReadPlayerStats();
            //    playerData.Init(playerStats);

            //    gameStats = ReadGameStats();
            //    gameData.Init(gameStats);

            //    //StartCoroutine(InitUI());
            //#endif

            //#if UNITY_STANDALONE
            //    playerStats = ReadPlayerStats();
            //    playerData.Init(playerStats);

            //    gameStats = ReadGameStats();
            //    gameData.Init(gameStats);

            //    //StartCoroutine(InitUI());
            //#endif
        }

        ////////public IEnumerator InitUI()
        ////////{
        ////////    yield return null;
        ////////    playerData.InvokeWeaponChanged();
        ////////    gameData.InvokeLevelChanged();
        ////////}

        //void OnDisable()
        //{
        //    playerStats = playerData.GetStats();
        //    Debug.Log("SHO");
        //    #if UNITY_STANDALONE
        //        WritePlayerStats(playerStats);
        //        WriteGameStats(gameStats);
        //    #endif        
        //    #if UNITY_EDITOR
        //        WritePlayerStats(playerStats);
        //        WriteGameStats(gameStats);
        //    #endif
        //}

        // TODO: try catch playerStats == null;
        public PlayerStats ReadPlayerStats()
        {
            string dataAsJson = File.ReadAllText(Application.dataPath + playerDataProjectFilePath);
            PlayerStats playerStats = JsonUtility.FromJson<PlayerStats>(dataAsJson);
            if (playerStats != null)
            {
                return playerStats;
            }
            else
            {
                return null;
            }
        }

        public GameStats ReadGameStats()
        {
            string dataAsJson = File.ReadAllText(Application.dataPath + gameDataProjectFilePath);
            GameStats gameStats = JsonUtility.FromJson<GameStats>(dataAsJson);
            if (gameStats != null)
            {
                return gameStats;
            }
            return null;
        }

        public void Write(object stats)
        {
            string dataAsJson = JsonUtility.ToJson(stats);
            string filePath = Application.dataPath + playerDataProjectFilePath;
            File.WriteAllText(filePath, dataAsJson);
        }

        public void WriteGameStats(GameStats gameStats)
        {
            string dataAsJson = JsonUtility.ToJson(gameStats);
            string filePath = Application.dataPath + gameDataProjectFilePath;
            File.WriteAllText(filePath, dataAsJson);
        }

        //public void Reset()
        //{
        //    // Write resetted playerStats
        //    PlayerStats resettedPlayerStats = new PlayerStats();
        //    string dataAsJson = JsonUtility.ToJson(resettedPlayerStats);
        //    string filePath = Application.dataPath + playerDataProjectFilePath;
        //    File.WriteAllText(filePath, dataAsJson);

        //    playerData.ResetPlayerStats();
        //}
    }
}