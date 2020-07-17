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
    // TODO: 
    // 1. All disk init/saving/reading should be done in PlayerController through ResourceLoader:
    //        1.1 PlayerCont.cs: model.HandleTeamWeaponsChanged(() => { ResourceLoader.SaveTeamWeapons(model.TeamWeapons) });
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

        public event EventHandler<GenericEventArgs<string>> OnPlayerStatsChanged;

        public PlayerStats PlayerStats { get; private set; }

        public Dictionary<string, Weapon> TeamWeapons { get; private set; }

        // Click Gun Data
        public WeaponData GunData { get; private set; }
        public WeaponStat DPS { get; private set; }
        public WeaponStat DMG { get; private set; }
        // endof Click Gun Data

        public PlayerModel()
        {
            InitPlayerStats();
            InitTeamWeapons(PlayerStats);
            InitClickGun();
        }

        // TODO (LP): instantiating of TeamWeapons should be moved to PlayerView
        private void InitTeamWeapons(PlayerStats playerStats)
        {
            TeamWeapons = new Dictionary<string, Weapon>();

            string path = Path.Combine(Application.persistentDataPath, "weapons.dat");

            WeaponData[] weaponStats = ResourceLoader.Load<WeaponData[]>(path);

            // TODO: init this with special default backup files...
            if (weaponStats == null)
            {
                Debug.Log("This is not the case");
                weaponStats = new WeaponData[2];

                weaponStats[0] = new WeaponData();
                weaponStats[0].DMG.Level = 0;
                weaponStats[0].DPS.Level = 0;
                weaponStats[0].DPS.UpgradeLevel = 0;
                weaponStats[0].DMG.UpgradeLevel = 0;
                weaponStats[0].WeaponName = "StandardPistol";

                weaponStats[1] = new WeaponData();
                weaponStats[1].DMG.Level = 0;
                weaponStats[1].DPS.Level = 0;
                weaponStats[1].DPS.UpgradeLevel = 0;
                weaponStats[1].DMG.UpgradeLevel = 0;
                weaponStats[1].WeaponName = "MachineGun";
            }

            foreach (var weapon in weaponStats)
            {
                GameObject obj = new GameObject(weapon.WeaponName);
                Weapon weaponScript = obj.AddComponent<Weapon>();
                // TODO:
                // 1. Subscribe to weaponScript.OnPropertyChanged
                // 2. Raise the event when notified OnPropertyChanged
                // 3. PlayerController subscribes to this event and changes view accordingly (it just updates the views with the ref to teamWeapons dictionary;
                weaponScript.Init(weapon.algorithms, weapon, playerStats);
                TeamWeapons.Add(weapon.WeaponName, weaponScript);
            }
        }

        public void SaveClickGun(WeaponStat dps, WeaponStat dmg, WeaponData gunData)
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
            ResourceLoader.Save<WeaponData>(path, data);
        }

        public void SaveTeamWeapons(Dictionary<string, Weapon> teamWeapons)
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
            ResourceLoader.Save<WeaponData[]>(path, teamWeaponsToSave);
        }

        public void SavePlayerStats()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            PlayerStats playerStats = new PlayerStats();

            playerStats.Gold = PlayerStats.Gold;
            playerStats.Level = PlayerStats.Level;
            playerStats.LastPlayTimestamp = DateTime.Now.Ticks;
            playerStats.DPSMultiplier = this.PlayerStats.DPSMultiplier;

            ResourceLoader.Save<PlayerStats>(path, playerStats);
        }

        private void InitClickGun()
        {
            string path = Path.Combine(Application.persistentDataPath, "clickGun.dat");

            GunData = ResourceLoader.Load<WeaponData>(path);
            // TODO: init this with special default backup files...
            if (GunData == null)
            {
                GunData = new WeaponData();
                GunData.DMG.Level = 0;
                GunData.DPS.Level = 0;
                GunData.DPS.UpgradeLevel = 0;
                GunData.DMG.UpgradeLevel = 0;
                //GunData.WeaponName = "Gun";
                // TODO: 
                // GunData.algorithms = ...
            }

            DPS = new WeaponStat(GunData.DPS, PlayerStats, GunData.algorithms.DPS);
            DMG = new WeaponStat(GunData.DMG, PlayerStats, GunData.algorithms.DMG);

            DPS.PropertyChanged += HandleClickGunChanged;
            DMG.PropertyChanged += HandleClickGunChanged;
        }

        private void InitPlayerStats()
        {
            string path = Path.Combine(Application.persistentDataPath, "playerStats.dat");

            PlayerStats = ResourceLoader.Load<PlayerStats>(path);

            if (PlayerStats == null)
            {
                this.PlayerStats = new PlayerStats();
                this.PlayerStats.Gold = 0;
                this.PlayerStats.Level = 0;
                this.PlayerStats.LastPlayTimestamp = DateTime.Now.Ticks;
                this.PlayerStats.DPSMultiplier = 1.1f;
            }

            PlayerStats.PropertyChanged += HandlePlayerStatChanged;
        }

        private void HandlePlayerStatChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPlayerStatsChanged?.Invoke(sender, new GenericEventArgs<string>(args.PropertyName));
        }

        private void HandleClickGunChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
            SaveClickGun(DPS, DMG, GunData);
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
        [SerializeField] private float _gold;
        public float Gold { get => _gold; set { SetField(ref _gold, value); } }

        [SerializeField] private int _level;
        public int Level { get => _level; set { SetField(ref _level, value); } }

        [SerializeField] private long _lastPlayTimestamp;
        public long LastPlayTimestamp { get => _lastPlayTimestamp; set { SetField(ref _lastPlayTimestamp, value); } }

        [Header("Team Stats")]
        [Tooltip("1.0 - 7.0")]
        [SerializeField] private float _dpsMultiplier;
        public float DPSMultiplier { get => _dpsMultiplier; set { SetField(ref _dpsMultiplier, value); } }

        public long IdleTimeSpan => _lastPlayTimestamp == 0 ? 0 : DateTime.Now.Ticks - _lastPlayTimestamp;

        // Indexer (will be used by Upgrade system a lot)
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
    }
}