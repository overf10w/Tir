using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Game
{
    public class ResourceLoader : MonoBehaviour
    {
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

        public UpgradeData[] LoadUpgradesData(string path)
        {
            UpgradeData[] upgrades = Load<UpgradeData[]>(path);
            if (upgrades == null)
            {
                int length = Resources.Load<UpgradesSO>("SO/Researches/Upgrades").Upgrades.Length;
                upgrades = new UpgradeData[length];
                for(int i = 0; i < upgrades.Length; i++)
                {
                    upgrades[i] = new UpgradeData();
                    upgrades[i].Id = i;
                    upgrades[i].IsActive = true;
                    Debug.Log("HELLLOOOO");
                }
            }
            return upgrades;
        }

        public static PlayerStats LoadPlayerStats()
        {
            return Resources.Load<PlayerData>("SO/Player/PlayerData").playerStats;
        }

        public static WeaponData LoadClickGun()
        {
            string path = Path.Combine(Application.persistentDataPath, "clickGun.dat");
            WeaponData data = ResourceLoader.Load<WeaponData>(path);

            if (data == null)
            {
                data = Resources.Load<GunData>("SO/Weapons/ClickGun/GunData").gunStats;
            }
            return data;
        }

        public static WeaponData[] LoadTeamWeapons()
        {
            string path = Path.Combine(Application.persistentDataPath, "weapons.dat");

            WeaponData[] weaponStats = ResourceLoader.Load<WeaponData[]>(path);

            if (weaponStats == null)
            {
                weaponStats = Resources.Load<WeaponsData>("SO/Weapons/TeamWeapons/WeaponsData").Weapons;
            }

            return weaponStats;
        }

        public static void SaveClickGun(WeaponStat dps, WeaponStat dmg, WeaponData gunData)
        {
            WeaponData data = new WeaponData();

            data.DPS = new StatData();
            data.DMG = new StatData();

            data.WeaponName = "Gun";
            data.DPS.Level = dps.Level;
            data.DMG.Level = dmg.Level;
            data.DPS.UpgradeLevel = dps.UpgradeLevel;
            data.DMG.UpgradeLevel = dmg.UpgradeLevel;

            data.algorithms = gunData.algorithms;

            string path = Path.Combine(Application.persistentDataPath, "clickGun.dat");
            Save<WeaponData>(path, data);
        }

        public static void SaveTeamWeapons(Dictionary<string, Weapon> teamWeapons)
        {
            WeaponData[] weapons = new WeaponData[teamWeapons.Count];
            int i = 0;
            foreach (var weapon in teamWeapons)
            {
                WeaponData data = new WeaponData();

                data.DPS = new StatData();
                data.DMG = new StatData();

                data.WeaponName = weapon.Key;
                data.ShootInterval = weapon.Value.ShootInterval;
                data.StartTimeout = weapon.Value.StartTimeout;

                data.DPS.Level = weapon.Value.DPS.Level;
                data.DMG.Level = weapon.Value.DMG.Level;
                data.DPS.UpgradeLevel = weapon.Value.DPS.UpgradeLevel;
                data.DMG.UpgradeLevel = weapon.Value.DMG.UpgradeLevel;
                data.algorithms = weapon.Value.Algorithms;

                weapons[i++] = data;
            }

            string path = Path.Combine(Application.persistentDataPath, "weapons.dat");
            Save<WeaponData[]>(path, weapons);
        }

        public static void SavePlayerStatsData(PlayerStats playerStats)
        {
            string _playerStatsDataPath = Path.Combine(Application.persistentDataPath, "playerStatsData.dat");

            playerStats.LastPlayTimestamp = DateTime.Now.Ticks;

            PlayerStatsData playerStatsData = new PlayerStatsData(playerStats);

            Save<PlayerStatsData>(_playerStatsDataPath, playerStatsData);
        }
    }
}