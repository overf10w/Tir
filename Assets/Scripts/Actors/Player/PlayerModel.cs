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

        // TODO: 
        // public WeaponStat DMG;
        public float currentDamage;
        public float currentAutoFire;

        private int damageLvl;
        // AutoFire level is basically DPS level !
        private int autoFireLvl;

        [SerializeField] 
        private float autoFireDuration;

        public Dictionary<string, Weapon> teamWeapons;

        private WeaponStatsStrategies weaponStatsStrategies;

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
            weaponStatsStrategies = Resources.Load<WeaponStatsStrategies>("SO/WeaponStatsStrategies");

            InitTeamWeapons();

            InitStats(playerStats);

            currentDamage = 2.0f;
            currentAutoFire = 0.0f;
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
            damageLvl = 0;
            autoFireLvl = 0;
            autoFireDuration = 0.0f;
        }

        public void InitStats(PlayerStats playerStats)
        {
            gold = playerStats._gold;
            damageLvl = playerStats._damageLvl;
            autoFireLvl = playerStats._autoFireLvl;
            autoFireDuration = playerStats._autoFireDuration;
        }

        public PlayerStats GetStats()
        {
            return new PlayerStats
            {
                _gold = gold,
                _damageLvl = damageLvl,
                _autoFireLvl = autoFireLvl,
                _autoFireDuration = autoFireDuration,
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