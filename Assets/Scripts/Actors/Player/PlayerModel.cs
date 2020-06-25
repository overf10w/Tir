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

        private void InitPlayerStats()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            playerStats = ResourceLoader.Load<PlayerStats>(path);

            Gold = playerStats.gold;
            Level = playerStats.level;
            _timeLastPlayed = playerStats.timeLastPlayed;

            Debug.Log("PlayerModel: Loaded Gold: " + Gold + ", Level: " + Level);
        }

        public void SavePlayerStats()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            PlayerStats playerStats = new PlayerStats();

            playerStats.gold = Gold;
            playerStats.level = Level;
            playerStats.timeLastPlayed = DateTime.Now.Ticks;

            ResourceLoader.Save<PlayerStats>(path, playerStats);
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

        public PlayerModel()
        {
            weaponStatsStrategies = Resources.Load<WeaponStatsStrategies>("SO/Weapons/TeamWeapons/WeaponStatsStrategies");

            InitTeamWeapons();

            InitClickGun();

            InitPlayerStats();

            //InitStats(playerStats);
        }

        private float _gold;
        public float Gold
        {
            get => _gold;
            set
            {
                SetField(ref _gold, value, "Gold");
                SavePlayerStats();
            }
        }

        private int _level;
        public int Level
        {
            get => _level;
            set
            {
                SetField(ref _level, value, "Level");
            }
        }

        //long elapsedTicks = DateTime.Now.Ticks - gameData._timeLastPlayed;

        private long _timeLastPlayed;
        private long gameStartDateTime = -1;
        public long IdleTimeSpan
        {
            get
            {
                if (gameStartDateTime == -1)
                {
                    gameStartDateTime = DateTime.Now.Ticks;
                }
                return gameStartDateTime - _timeLastPlayed;
            }
            private set { }
        }

        public void InitStats(PlayerStats playerStats)
        {
            _gold = playerStats.gold;
            _level = playerStats.level;
            _timeLastPlayed = playerStats.timeLastPlayed;
        }

        private PlayerStats playerStats;
    }

    [System.Serializable]
    public class PlayerStats
    {
        public float gold;
        public int level;
        public long timeLastPlayed;
    }
}