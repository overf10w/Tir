using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class PlayerModel
    {
        #region NotifyPropertyChanged
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
        #endregion

        public Dictionary<string, Weapon> teamWeapons;
        private WeaponStatsStrategies weaponStatsStrategies;

        public WeaponStatData gunData;
        public WeaponStatsAlgorithmsHolder gunAlgorithmHolder;

        [HideInInspector]
        public PlayerStats playerStats;
        
        // Click Gun Stats
        public WeaponStat DPS { get; set; }
        public WeaponStat DMG { get; set; }

        // TODO (LP): instantiating of TeamWeapons should be moved to PlayerView
        private void InitTeamWeapons(PlayerStats playerStats)
        {
            teamWeapons = new Dictionary<string, Weapon>();

            string path = Path.Combine(Application.persistentDataPath, "weapons.dat");

            WeaponStatData[] weaponStats = ResourceLoader.Load<WeaponStatData[]>(path);

            // TODO: init with special scriptable object injected here (through constructor chain) all the way down through GameManager
            if (weaponStats == null)
            {
                weaponStats = new WeaponStatData[2];

                weaponStats[0] = new WeaponStatData();
                weaponStats[0].dmgLevel = 0;
                weaponStats[0].dpsLevel = 0;
                weaponStats[0].upgradeLevel = 0;
                weaponStats[0].weaponName = "StandardPistol";

                weaponStats[1] = new WeaponStatData();
                weaponStats[1].dmgLevel = 0;
                weaponStats[1].dpsLevel = 0;
                weaponStats[1].upgradeLevel = 0;
                weaponStats[1].weaponName = "MachineGun";
            }

            foreach (var weapon in weaponStats)
            {
                foreach(var algo in weaponStatsStrategies.algorithms)
                {
                    if (weapon.weaponName == algo.name)
                    {
                        GameObject obj = new GameObject(weapon.weaponName);
                        Weapon weaponScript = obj.AddComponent<Weapon>();
                        // TODO: 
                        // 1. Subscribe to weaponScript.OnPropertyChanged
                        // 2. Raise the event when notified OnPropertyChanged
                        // 3. PlayerController subscribes to this event and changes view accordingly (it just updates the views with the ref to teamWeapons dictionary;

                        weaponScript.Init(algo, weapon, playerStats);
                        teamWeapons.Add(weapon.weaponName, weaponScript);
                        //Debug.Log("weapon.weaponName: [" + weapon.weaponName + "] == algo.name: [" + algo.name + "]");
                        break;
                    }
                }
            }
        }

        public void UpdateTeamWeapons()
        {
            foreach(var wpn in teamWeapons)
            {
                wpn.Value.UpdateSelf();
            }
        }

        private void InitClickGun()
        {
            string path = Path.Combine(Application.persistentDataPath, "clickGun.dat");

            gunData = ResourceLoader.Load<WeaponStatData>(path);
            if (gunData == null)
            {
                gunData = new WeaponStatData();
                gunData.dmgLevel = 0;
                gunData.dpsLevel = 0;
                gunData.upgradeLevel = 0;
                gunData.weaponName = "Gun";
            }
            gunAlgorithmHolder = Resources.Load<GunStatsStrategy>("SO/Weapons/ClickGun/GunStatsStrategy").algorithm;

            DPS = new WeaponStat(gunData.dpsLevel, gunData.upgradeLevel, gunAlgorithmHolder.DPS);
            DMG = new WeaponStat(gunData.dmgLevel, gunData.upgradeLevel, gunAlgorithmHolder.DMG);

            DPS.PropertyChanged += HandleClickGunChanged;
            DMG.PropertyChanged += HandleClickGunChanged;

            //Debug.Log("PlayerModel: Read gunData: dpsLevel: " + gunData.dpsLevel + ", dmgLevel: " + gunData.dmgLevel);
        }

        private void InitPlayerStats()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            this.playerStats = ResourceLoader.Load<PlayerStats>(path);

            if (this.playerStats == null)
            {
                this.playerStats = new PlayerStats();
                this.playerStats.gold = 0;
                this.playerStats.level = 0;
                this.playerStats.timeLastPlayed = DateTime.Now.Ticks;
                this.playerStats.dpsMultiplier = 1.1f;
            }

            Gold = this.playerStats.gold;
            Level = this.playerStats.level;
            _timeLastPlayed = this.playerStats.timeLastPlayed;

            this.playerStats.PropertyChanged += HandlePlayerStatChanged;
            //Debug.Log("PlayerModel: InitPlayerStats: playerStats == null : " + (this.playerStats == null).ToString());
        }

        public event EventHandler<GenericEventArgs<string>> OnGlobalStatChanged;

        public void HandlePlayerStatChanged(object sender, PropertyChangedEventArgs args)
        {
            OnGlobalStatChanged?.Invoke(sender, new GenericEventArgs<string>(args.PropertyName));
        }

        public void SavePlayerStats()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            PlayerStats playerStats = new PlayerStats();

            playerStats.gold = Gold;
            playerStats.level = Level;
            playerStats.timeLastPlayed = DateTime.Now.Ticks;
            playerStats.dpsMultiplier = this.playerStats.dpsMultiplier;

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
            data.upgradeLevel = DPS.UpgradeLevel;

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
                data.upgradeLevel = weapon.Value.DPS.UpgradeLevel;

                teamWeaponsToSave[i++] = data;
            }

            string path = Path.Combine(Application.persistentDataPath, "weapons.dat");
            ResourceLoader.Save<WeaponStatData[]>(path, teamWeaponsToSave);
        }

        public PlayerModel()
        {
            weaponStatsStrategies = Resources.Load<WeaponStatsStrategies>("SO/Weapons/TeamWeapons/WeaponStatsStrategies");

            InitPlayerStats();

            InitTeamWeapons(playerStats);

            InitClickGun();


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

    }

    [System.Serializable]
    public class PlayerStats : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            //Debug.Log("PlayerStats: SetField<T>(): Invoked");
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        [Header("Player")]
        public float gold;
        public int level;
        public long timeLastPlayed;

        [Header("Team Stats")]
        [Tooltip("1.0 - 7.0")]
        public float dpsMultiplier;

        public float DPSMultiplier { get => dpsMultiplier; set { SetField(ref dpsMultiplier, value); } }

        // Indexer (will be used by Upgrade system a lot)
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
    }
}