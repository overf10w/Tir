using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Game
{
    // TODO (only prior final build): make it singleton
    public class ResourceLoader : MonoBehaviour
    {
        public static ResourceLoader Instance;

        private string playerDataProjectFilePath = "/StreamingAssets/data.json";
        private string gameDataProjectFilePath = "/StreamingAssets/gameData.json";

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
        }

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

        /// NEW BEAUTIFUL CODE GOES HERE
        /// HEHE

        public static T Load<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
                using (Stream stream = File.OpenRead(path))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream) as T;
                }
            }
            Debug.LogError("The file doesn't exist at: " + path);
            return null;
        }

        public static void Save<T>(string filename, T data) where T : class
        {
            using (Stream stream = File.Open(filename, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }
        }
    }
}