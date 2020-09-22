using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using System.Linq;


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

        public event EventHandler<EventArgs<string>> OnPlayerStatsChanged;

        public PlayerStats PlayerStats { get; private set; }

        public Dictionary<string, Weapon> TeamWeapons { get; private set; }

        // ClickGunData
        public WeaponData GunData { get; private set; }
        public WeaponStat DPS { get; private set; }
        public WeaponStat DMG { get; private set; }
        // endof ClickGunData

        public PlayerModel(string _playerStatsDataPath)
        {
            LoadPlayerStats(_playerStatsDataPath);
            InitTeamWeapons(PlayerStats);
            InitClickGun();
            InitPlayerStats(); 
        }

        private void LoadPlayerStats(string _playerStatsDataPath)
        {
            PlayerStats = ResourceLoader.LoadPlayerStats();
            PlayerStatsData playerStatsData = ResourceLoader.Load<PlayerStatsData>(_playerStatsDataPath);
            if (playerStatsData != null)
            {
                PlayerStats.SetPlayerStats(ResourceLoader.Load<PlayerStatsData>(_playerStatsDataPath));
            }
            PlayerStats.PropertyChanged += PlayerStatChangedHandler;
        }

        private void InitPlayerStats()
        {
            PlayerStats.IdleProfit = CalculateIdleProfit(TeamWeapons, PlayerStats);
            PlayerStats.Gold += PlayerStats.IdleProfit;
            
            Debug.Log("PlayerStats.IdleProfit: " + PlayerStats.IdleProfit);
        }

        // TODO (?): move this to PlayerStats
        private float CalculateIdleProfit(Dictionary<string, Weapon> teamWeapons, PlayerStats playerStats)
        {
            float teamDPS = 0.0f;
            foreach (var weapon in teamWeapons)
            {
                teamDPS += weapon.Value.DPS.Value;    
            }

            teamDPS /= 2.0f;

            double timePassed = TimeSpan.FromTicks(playerStats.IdleTimeSpan).TotalMinutes;
            Debug.Log("timePassed: " + timePassed);
            //float levelGold = WaveSpawnerAlgorithm.GetLevelGold(PlayerStats.Level);

            // TODO: calculation to be dependent on timePassed, levelGold

            return teamDPS * (float)timePassed;
        }

        // TODO (LP): instantiating of TeamWeapons should be moved to PlayerView
        private void InitTeamWeapons(PlayerStats playerStats)
        {
            TeamWeapons = new Dictionary<string, Weapon>();

            WeaponData[] weaponDataArray = ResourceLoader.LoadTeamWeapons();

            foreach (var weaponData in weaponDataArray)
            {
                string name = weaponData.WeaponName;

                GameObject weaponObj = new GameObject(name);
                Weapon weapon = weaponObj.AddComponent<Weapon>();
                // TODO (OLD):
                // 1. Subscribe to weaponScript.OnPropertyChanged
                // 2. Raise the event when notified OnPropertyChanged
                // 3. PlayerController subscribes to this event and changes view accordingly (it just updates the views with the ref to teamWeapons dictionary;

                int index = PlayerStats.WeaponsMultipliers.List.FindIndex(item => item.Name == name);
                float multiplier = PlayerStats.WeaponsMultipliers.List[index].Value;
                weapon.Init(multiplier, weaponData, playerStats);
                TeamWeapons.Add(name, weapon);

                // Set PlayerStats.WeaponsStats:
                index = PlayerStats.WeaponsLevels.List.FindIndex(item => item.Name == name);
                PlayerStats.WeaponsLevels.List[index].Value = weapon.DPS.Level;
            }
        }

        private void InitClickGun()
        {
            GunData = ResourceLoader.LoadClickGun();

            DPS = new WeaponStat(GunData.DPS, PlayerStats, GunData.algorithms.DPS);
            DMG = new WeaponStat(GunData.DMG, PlayerStats, GunData.algorithms.DMG);

            DPS.PropertyChanged += ClickGunChangedHandler;
            DMG.PropertyChanged += ClickGunChangedHandler;
        }

        private void PlayerStatChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            OnPlayerStatsChanged?.Invoke(sender, new EventArgs<string>(args.PropertyName));
        }

        private void ClickGunChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
            ResourceLoader.SaveClickGun(DPS, DMG, GunData);
        }
    }
}