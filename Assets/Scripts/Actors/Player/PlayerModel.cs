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

        public event EventHandler<GenericEventArgs<string>> OnGlobalStatChanged;

        // Player stats
        public PlayerStats PlayerStats { get; private set; }

        // Player stats
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

        private long _timeLastPlayed;
        private long _gameStartDateTime = -1;
        public long IdleTimeSpan
        {
            get
            {
                if (_gameStartDateTime == -1)
                {
                    _gameStartDateTime = DateTime.Now.Ticks;
                }
                return _gameStartDateTime - _timeLastPlayed;
            }
        }

        public Dictionary<string, Weapon> TeamWeapons { get; private set; }

        // Click Gun Stats
        public WeaponStat DPS { get; private set; }
        public WeaponStat DMG { get; private set; }
        // endof Click Gun Stats

        public WeaponStatData GunData { get; private set; }
        public WeaponStatsAlgorithmsHolder GunAlgorithmHolder { get; private set; }
        private WeaponStatsStrategies _weaponStatsStrategies;

        public PlayerModel()
        {
            _weaponStatsStrategies = Resources.Load<WeaponStatsStrategies>("SO/Weapons/TeamWeapons/WeaponStatsStrategies");
            InitPlayerStats();
            InitTeamWeapons(PlayerStats);
            InitClickGun();
        }

        // TODO (LP): instantiating of TeamWeapons should be moved to PlayerView
        private void InitTeamWeapons(PlayerStats playerStats)
        {
            TeamWeapons = new Dictionary<string, Weapon>();

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
                foreach(var algo in _weaponStatsStrategies.algorithms)
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
                        TeamWeapons.Add(weapon.weaponName, weaponScript);
                        //Debug.Log("weapon.weaponName: [" + weapon.weaponName + "] == algo.name: [" + algo.name + "]");
                        break;
                    }
                }
            }
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
            foreach (var weapon in teamWeapons)
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

        public void SavePlayerStats()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            PlayerStats playerStats = new PlayerStats();

            playerStats.gold = Gold;
            playerStats.level = Level;
            playerStats.timeLastPlayed = DateTime.Now.Ticks;
            playerStats.dpsMultiplier = this.PlayerStats.dpsMultiplier;

            ResourceLoader.Save<PlayerStats>(path, playerStats);
        }

        public void InitStats(PlayerStats playerStats)
        {
            _gold = playerStats.gold;
            _level = playerStats.level;
            _timeLastPlayed = playerStats.timeLastPlayed;
        }

        private void InitClickGun()
        {
            string path = Path.Combine(Application.persistentDataPath, "clickGun.dat");

            GunData = ResourceLoader.Load<WeaponStatData>(path);
            if (GunData == null)
            {
                GunData = new WeaponStatData();
                GunData.dmgLevel = 0;
                GunData.dpsLevel = 0;
                GunData.upgradeLevel = 0;
                GunData.weaponName = "Gun";
            }

            GunAlgorithmHolder = Resources.Load<GunStatsStrategy>("SO/Weapons/ClickGun/GunStatsStrategy").algorithm;

            DPS = new WeaponStat(GunData.dpsLevel, GunData, PlayerStats, GunAlgorithmHolder.DPS);
            DMG = new WeaponStat(GunData.dmgLevel, GunData, PlayerStats, GunAlgorithmHolder.DMG);

            DPS.PropertyChanged += HandleClickGunChanged;
            DMG.PropertyChanged += HandleClickGunChanged;
        }

        private void InitPlayerStats()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            this.PlayerStats = ResourceLoader.Load<PlayerStats>(path);

            if (this.PlayerStats == null)
            {
                this.PlayerStats = new PlayerStats();
                this.PlayerStats.gold = 0;
                this.PlayerStats.level = 0;
                this.PlayerStats.timeLastPlayed = DateTime.Now.Ticks;
                this.PlayerStats.dpsMultiplier = 1.1f;
            }

            Gold = this.PlayerStats.gold;
            Level = this.PlayerStats.level;
            _timeLastPlayed = this.PlayerStats.timeLastPlayed;

            this.PlayerStats.PropertyChanged += HandlePlayerStatChanged;
        }

        private void HandlePlayerStatChanged(object sender, PropertyChangedEventArgs args)
        {
            OnGlobalStatChanged?.Invoke(sender, new GenericEventArgs<string>(args.PropertyName));
        }

        private void HandleClickGunChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
            SaveClickGun(DPS, DMG);
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