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

        // TODO: if (upgrades == null) INIT upgrades with the backup SO
        public Upgrades.Upgrade[] LoadUpgrades(string path)
        {
            Upgrades.Upgrade[] upgrades = Load<Upgrades.Upgrade[]>(path);
            if (upgrades == null)
            {
                upgrades = Resources.Load<Upgrades>("SO/Researches/Upgrades").upgrades;
            }
            return upgrades;
        }

        public static PlayerStats LoadPlayerStats()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            string scriptableObjectPath = "SO/Player/PlayerData";

            PlayerStats playerStats = Load<PlayerStats>(path);

            if (playerStats == null)
            {
                //playerStats = new PlayerStats();
                //playerStats.Gold = 0;
                //playerStats.Level = 0;
                //playerStats.LastPlayTimestamp = DateTime.Now.Ticks;

                playerStats = Resources.Load<PlayerData>("SO/Player/PlayerData").playerStats;

                // TODO: playerStats.Skills = new Skill[] { new Skill() { Name="", Value = 1.1f}, new Skill{Name = "", Value = } }
            }

            return playerStats;
        }

        public static WeaponData LoadClickGun()
        {
            string path = Path.Combine(Application.persistentDataPath, "clickGun.dat");
            WeaponData data = ResourceLoader.Load<WeaponData>(path);

            if (data == null)
            {
                data = Resources.Load<GunData>("SO/Weapons/ClickGun/GunData").gunStats;
                //GunData.WeaponName = "Gun";
                // TODO: 
                // GunData.algorithms = ...
            }
            return data;
        }

        public static WeaponData[] LoadTeamWeapons()
        {
            string path = Path.Combine(Application.persistentDataPath, "weapons.dat");

            WeaponData[] weaponStats = ResourceLoader.Load<WeaponData[]>(path);

            // TODO: init this with special default backup files...
            if (weaponStats == null)
            {
                weaponStats = Resources.Load<WeaponsData>("SO/Weapons/TeamWeapons/WeaponsData").weapons;
                //Debug.Log("This is not the case");
                //weaponStats = new WeaponData[2];

                //weaponStats[0] = new WeaponData();
                //weaponStats[0].DMG.Level = 0;
                //weaponStats[0].DPS.Level = 0;
                //weaponStats[0].DPS.UpgradeLevel = 0;
                //weaponStats[0].DMG.UpgradeLevel = 0;
                //weaponStats[0].WeaponName = "StandardPistol";

                //weaponStats[1] = new WeaponData();
                //weaponStats[1].DMG.Level = 0;
                //weaponStats[1].DPS.Level = 0;
                //weaponStats[1].DPS.UpgradeLevel = 0;
                //weaponStats[1].DMG.UpgradeLevel = 0;
                //weaponStats[1].WeaponName = "MachineGun";
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
            WeaponData[] teamWeaponsToSave = new WeaponData[teamWeapons.Count];
            int i = 0;
            foreach (var weapon in teamWeapons)
            {
                WeaponData data = new WeaponData();

                data.DPS = new StatData();
                data.DMG = new StatData();

                data.WeaponName = weapon.Key;

                data.DPS.Level = weapon.Value.DPS.Level;
                data.DMG.Level = weapon.Value.DMG.Level;
                data.DPS.UpgradeLevel = weapon.Value.DPS.UpgradeLevel;
                data.DMG.UpgradeLevel = weapon.Value.DMG.UpgradeLevel;
                data.algorithms = weapon.Value.Algorithms;

                teamWeaponsToSave[i++] = data;
            }

            string path = Path.Combine(Application.persistentDataPath, "weapons.dat");
            Save<WeaponData[]>(path, teamWeaponsToSave);
        }

        public static void SavePlayerStats(PlayerStats playerStats)
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            if (playerStats == null)
            {
                playerStats = new PlayerStats();
                playerStats.Gold = 0;
                playerStats.Level = 0;
                // TODO: playerStats.Skills = new Skill[] { new Skill() { Name="", Value = 1.1f}, new Skill{Name = "", Value = } }
            }

            playerStats.LastPlayTimestamp = DateTime.Now.Ticks;

            Save<PlayerStats>(path, playerStats);
        }
    }
}