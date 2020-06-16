using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Game
{
    // TODO: probably don't need this
    public class WeaponArgs : EventArgs
    {
        public WeaponModel weaponModel;
        public Weapon sender;

        public WeaponArgs(Weapon sender, WeaponModel weaponModel)
        {
            this.sender = sender;
            this.weaponModel = weaponModel;
        }
    }

    public delegate void WeaponChanged(WeaponArgs e);

    // TODO: value updated
    public delegate void AutoFireUpdated(float seconds);
    public delegate void AttackUpdated(float value);
    public delegate void LevelChanged(int level);

    [System.Serializable]
    public class PlayerModel
    {
        // boiler-plate
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

        // TODO: probably don't need
        public event WeaponChanged OnWeaponChanged;
        public event AutoFireUpdated OnAutoFireUpdated;
        public event AttackUpdated OnAttackUpdated;

        [Header("Skills")]
        [SerializeField]
        public float currentDamage;
        public float currentAutoFire;

        private int damageLvl;
        private int autoFireLvl;

        private Weapon pistol;
        private int pistolLvl;

        private Weapon doublePistol;
        private int doublePistolLvl;

        [SerializeField] private float autoFireDuration;

        public Dictionary<string, Weapon> teamWeapons;

        private void InitTeamWeapons()
        {
            teamWeapons = new Dictionary<string, Weapon>();

            GameObject standardPistol = new GameObject();
            Weapon weaponScript = standardPistol.AddComponent<Weapon>();
            weaponScript.WeaponModel = new WeaponModel(10, 10, 10);
            
            weaponScript.WeaponModel.DPS = new Cost();


            Debug.Log("PlayerModel.weaponScript.WeaponModel.DPS == null: " + (weaponScript.WeaponModel.DPS == null));
            weaponScript.WeaponModel.DPS.CurrCost = 8;
            weaponScript.WeaponModel.DPS.NextCost = 12;

            teamWeapons.Add("StandardPistol", weaponScript);
        }

        public PlayerModel(PlayerStats playerStats)
        {
            InitTeamWeapons();

            InitStats(playerStats);

            currentDamage = 2.0f;
            currentAutoFire = 0.0f;

            var pistolPrefab = Resources.Load<Weapon>("Prefabs/WeaponPistolPrefab");
            pistol = UnityEngine.Object.Instantiate(pistolPrefab, GameObject.FindObjectOfType<PlayerView>().transform) as Weapon;
            pistol.weaponType = WeaponType.PISTOL;
            pistol.WeaponModel = new WeaponModel(12, 2, 0);
            pistol.WeaponModel.Init(pistolLvl);

            var doublePistolPrefab = Resources.Load<Weapon>("Prefabs/WeaponPistolDoublePrefab");
            doublePistol = UnityEngine.Object.Instantiate(doublePistolPrefab, GameObject.FindObjectOfType<PlayerView>().transform) as Weapon;
            doublePistol.weaponType = WeaponType.DOUBLE_PISTOL;
            doublePistol.WeaponModel = new WeaponModel(14, 4, 0);
            doublePistol.WeaponModel.Init(doublePistolLvl);
        }

        [Header("Player Stats")]
        [SerializeField]
        private float gold;
        public float Gold
        {
            get { return gold; }
            set
            {
                SetField(ref gold, value, "Gold");
            }
        }

        public void UpdatePistol()
        {
            if (gold >= pistol.WeaponModel.Cost)
            {
                gold -= pistol.WeaponModel.Cost;
                pistol.WeaponModel.UpdateSelf();
                OnWeaponChanged?.Invoke(new WeaponArgs(pistol, pistol.WeaponModel));
                pistolLvl = pistol.WeaponModel.level;
            }
            else
            {
                Debug.LogWarning("Sorry bro no money =((");
            }
        }

        public void UpdateDoublePistol()
        {
            if (gold >= doublePistol.WeaponModel.Cost)
            {
                gold -= doublePistol.WeaponModel.Cost;
                doublePistol.WeaponModel.UpdateSelf();
                OnWeaponChanged?.Invoke(new WeaponArgs(doublePistol, doublePistol.WeaponModel));
                doublePistolLvl = doublePistol.WeaponModel.level;
            }
            else
            {
                Debug.LogWarning("Sorry bro no money =((");
            }
        }

        public void ResetPlayerStats()
        {
            gold = 0;
            damageLvl = 0;
            pistolLvl = 0;
            doublePistolLvl = 0;
            autoFireLvl = 0;
            autoFireDuration = 0.0f;
        }

        public void InitStats(PlayerStats playerStats)
        {
            gold = playerStats._gold;
            damageLvl = playerStats._damageLvl;
            pistolLvl = playerStats._pistolLvl;
            doublePistolLvl = playerStats._doublePistolLvl;
            autoFireLvl = playerStats._autoFireLvl;
            autoFireDuration = playerStats._autoFireDuration;
        }

        public PlayerStats GetStats()
        {
            return new PlayerStats
            {
                _gold = gold,
                _damageLvl = damageLvl,
                _pistolLvl = pistolLvl,
                _doublePistolLvl = doublePistolLvl,
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