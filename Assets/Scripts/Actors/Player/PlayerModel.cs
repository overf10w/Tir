using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class PlayerModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public float currentDamage;

        public Dictionary<string, Weapon> teamWeapons;
        private WeaponStatsStrategies weaponStatsStrategies;

        public WeaponStatData gunData;
        public WeaponStatsAlgorithmsHolder gunAlgorithmHolder;
        public WeaponStat DPS { get; set; }
        public WeaponStat DMG { get; set; }

        // TODO (LP): instantiating of TeamWeapons should be moved to PlayerView
        private void InitTeamWeapons()
        {
            teamWeapons = new Dictionary<string, Weapon>();

            string path = Path.Combine(Application.persistentDataPath, "weapons.dat");

            WeaponStatData[] loadedWeapons = ResourceLoader.Load<WeaponStatData[]>(path);

            foreach (var weapon in loadedWeapons)
            {
                foreach(var algo in weaponStatsStrategies.algorithms)
                {
                    if (weapon.weaponName == algo.name)
                    {
                        GameObject obj = new GameObject(weapon.weaponName);
                        Weapon weaponScript = obj.AddComponent<Weapon>();
                        weaponScript.Init(algo, weapon);
                        teamWeapons.Add(weapon.weaponName, weaponScript);
                        Debug.Log("weapon.weaponName: [" + weapon.weaponName + "] == algo.name: [" + algo.name + "]");
                        break;
                    }
                }
            }
        }

        private void InitClickGun()
        {
            string path = Path.Combine(Application.persistentDataPath, "clickGun.dat");

            gunData = ResourceLoader.Load<WeaponStatData>(path);
            gunAlgorithmHolder = Resources.Load<GunStatsStrategy>("SO/Weapons/ClickGun/GunStatsStrategy").algorithm;

            DPS = new WeaponStat(gunData.dpsLevel, gunAlgorithmHolder.DPS);
            DMG = new WeaponStat(gunData.dmgLevel, gunAlgorithmHolder.DMG);

            DPS.PropertyChanged += HandleClickGunChanged;
            DMG.PropertyChanged += HandleClickGunChanged;

            Debug.Log("PlayerModel: Read gunData: dpsLevel: " + gunData.dpsLevel + ", dmgLevel: " + gunData.dmgLevel);
        }

        public void HandleClickGunChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
            SaveClickGun(DPS, DMG);
        }

        public void SaveClickGun(WeaponStat DPS, WeaponStat DMG) 
        {
            WeaponStatData data = new WeaponStatData();

            data.weaponName = "Gun";
            data.dpsLevel = DPS.Level;
            data.dmgLevel = DMG.Level;

            string path = Path.Combine(Application.persistentDataPath, "clickGun.dat");
            ResourceLoader.Save<WeaponStatData>(path, data);
        }

        public void SaveTeamWeapons(Dictionary<string, Weapon> teamWeapons)
        {
            WeaponStatData[] teamWeaponsToSave = new WeaponStatData[teamWeapons.Count];
            int i = 0;
            foreach(var weapon in teamWeapons)
            {
                WeaponStatData data = new WeaponStatData();
                data.weaponName = weapon.Key;
                data.dpsLevel = weapon.Value.DPS.Level;
                data.dmgLevel = weapon.Value.DMG.Level;

                teamWeaponsToSave[i++] = data;
            }

            string path = Path.Combine(Application.persistentDataPath, "weapons.dat");
            ResourceLoader.Save<WeaponStatData[]>(path, teamWeaponsToSave);
        }

        public PlayerModel(PlayerStats playerStats)
        {
            weaponStatsStrategies = Resources.Load<WeaponStatsStrategies>("SO/Weapons/TeamWeapons/WeaponStatsStrategies");

            InitTeamWeapons();

            InitClickGun();

            InitStats(playerStats);

            currentDamage = 2.0f;
        }

        [Header("Player Stats")]
        [SerializeField]
        private float gold;
        public float Gold
        {
            get => gold;
            set
            {
                SetField(ref gold, value, "Gold");
            }
        }

        public void ResetPlayerStats()
        {
            gold = 0;
        }

        public void InitStats(PlayerStats playerStats)
        {
            gold = playerStats._gold;
        }

        public PlayerStats GetStats()
        {
            return new PlayerStats
            {
                _gold = gold,
                _timeLastPlayed = DateTime.Now.Ticks
            };
        }
    }

    [System.Serializable]
    public class PlayerStats
    {
        public float _gold;
        public int _level;
        public int _currentWave;
        public int _damageLvl;
        public int _pistolLvl;
        public int _doublePistolLvl;
        public int _autoFireLvl;
        public float _autoFireDuration;
        public long _timeLastPlayed;
    }
}