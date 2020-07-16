using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Game
{
    public class ResourceLoader : MonoBehaviour
    {
        public static ResourceLoader Instance;

        private const string _playerDataProjectFilePath = "/StreamingAssets/data.json";
        private const string _gameDataProjectFilePath = "/StreamingAssets/gameData.json";

        private void Awake()
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
            string dataAsJson = File.ReadAllText(Application.dataPath + _playerDataProjectFilePath);
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

        // TODO: if (upgrades == null) INIT upgrades with the backup SO
        public Upgrades.Upgrade[] LoadUpgrades(string path)
        {
            Upgrades.Upgrade[] upgrades = Load<Upgrades.Upgrade[]>(path);
            if (upgrades == null)
            {
                upgrades = new Upgrades.Upgrade[1];
                upgrades[0] = new Upgrades.Upgrade();
                upgrades[0].Name = "DPS++";
                upgrades[0].Description = "Increase DPS by Kek%";
                upgrades[0].Price = 10000;
                upgrades[0].Amount = 100;

                upgrades[0].criterias = new Upgrades.Criteria[1];
                upgrades[0].criterias[0] = new Upgrades.Criteria();
                upgrades[0].criterias[0].indexer = "DPSMultiplier";
                upgrades[0].criterias[0].threshold = 0;
            }
            return upgrades;
        }
    }
}