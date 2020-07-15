using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class WeaponStatData
    {
        public string weaponName;
        public int dpsLevel;
        public int dmgLevel;
        public int upgradeLevel;
    }

    // TODO: 
    // PROP UpgradePrice
    // PROP NextUpgradePrice
    // PROP BaseUpgradePrice
    // Method UpgradeSkill();

    // TODO: WeaponStat => WeaponStats - [done]
    // WeaponStats to have these props:
    // Value
    // Level
    // Price
    // (each of the props can be of type CharacterStat (https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/)) - but that's not obligatory: 
    public class WeaponStat : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        private WeaponStatsAlgorithm _algorithm;

        public WeaponStat(int level, int price, int value)
        {
            Level = level;
            Price = price;
            Value = value;
        }

        public WeaponStat(int level)
        {
            Level = level;
        }

        private float dpsMultiplier; // TODO: remove, and use playerStats.dpsMultiplier instead

        private PlayerStats _playerStats;

        public void UpdateSelf()
        {
            dpsMultiplier = _playerStats.dpsMultiplier;
        }

        public WeaponStat(int level, WeaponStatData weaponStats, PlayerStats playerStats, WeaponStatsAlgorithm algorithm)
        {
            // Init this WeaponStat's stats
            Level = level;
            _upgradeLevel = weaponStats.upgradeLevel;

            // Init global teamWeaponsStats
            dpsMultiplier = playerStats.dpsMultiplier;

            Debug.LogWarning("Weapon.dpsMultiplier: " + dpsMultiplier);

            this._algorithm = algorithm;
            this._playerStats = playerStats;
        }

        public WeaponStat(int level, int upgradeLevel, WeaponStatsAlgorithm algorithm)
        {
            Level = level;
            _upgradeLevel = upgradeLevel;
            this._algorithm = algorithm;
        }

        private int _level;
        public int Level { get => _level; set { SetField(ref _level, value); } }

        // TODO: make all getters arrow getters '=>' like this (where possible throughout the project)
        // TODO: check for cases when this.algorithm == null !!!!
        public float Price { get => _algorithm.GetPrice(Level); set { } }
        
        public float NextPrice { get => _algorithm.GetNextPrice(Level); set { } }

        private float _value;
        public float Value { get => _algorithm.GetValue(Level, _upgradeLevel, dpsMultiplier); set { _value = value; } }

        public float NextValue { get => _algorithm.GetNextValue(Level); set { } }

        private int _upgradeLevel;

        public int UpgradeLevel { get => _upgradeLevel; set { SetField(ref _upgradeLevel, value); } }

        public float UpgradePrice { get => _algorithm.GetUpgradePrice(_upgradeLevel); set { } }
        public float NextUpgradePrice { get => _algorithm.GetNextUpgradePrice(_upgradeLevel); set { }}

        // TODO: setter
        public void Upgrade()
        {
            _upgradeLevel++;
        }
    }

    public class Weapon : MessageHandler
    {
        #region MessageHandler
        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.WAVE_CHANGED };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.WAVE_CHANGED)
            {
                //Debug.Log("Weapon.cs: On Wave Changed!");
                this._wave = (Wave)message.objectValue;
            }
        }
        #endregion

        public WeaponStat DPS { get; private set; }

        public WeaponStat DMG { get; private set; }

        private PlayerStats _playerStats;
        private WeaponStatsAlgorithmsHolder _algorithmHolder;

        private Wave _wave;

        private float _nextShotTime;
        private float _msBetweenShots = 200;

        public void Init(WeaponStatsAlgorithmsHolder algorithmHolder, WeaponStatData data, PlayerStats playerStats)
        {
            InitMessageHandler();

            _algorithmHolder = algorithmHolder;
            _playerStats = playerStats;

            DPS = new WeaponStat(data.dpsLevel, data, playerStats, algorithmHolder.DPS);
            DMG = new WeaponStat(data.dmgLevel, data, playerStats, algorithmHolder.DMG);
        }

        private void Update()
        {
            if (_wave != null)
            {
                Fire(_wave);
            }
        }

        public void Fire(Wave wave)
        {
            if (Time.time > _nextShotTime)
            {
                _nextShotTime = Time.time + _msBetweenShots / 1000;
                IDestroyable cube = wave.Cubes.ElementAtOrDefault(new System.Random().Next(wave.Cubes.Count));
                if ((MonoBehaviour)cube != null)
                {
                    cube.TakeDamage(DPS.Value);
                }
                else
                {
                    Debug.Log("Weapon: " + ": There's no cube there!");
                }
            }
        }

        public void UpdateSelf()
        {
            DPS.UpdateSelf();
            DMG.UpdateSelf();
        }
    }
}