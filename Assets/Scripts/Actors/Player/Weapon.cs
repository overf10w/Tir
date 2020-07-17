using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class WeaponData
    {

        [SerializeField] private string _weaponName;
        public string WeaponName { get => _weaponName; set { _weaponName = value; } }

        [SerializeField] private StatData _dps;
        // TODO: remove setter
        public StatData DPS { get => _dps; set { _dps = value; } }

        [SerializeField] private StatData _dmg;
        // TODO: remove setter
        public StatData DMG { get => _dmg; set { _dmg = value; } }

        public WeaponAlgorithms algorithms;
    }

    [System.Serializable]
    public class StatData
    {
        [SerializeField] private int _level;
        public int Level { get => _level; set { _level = value; } }

        [SerializeField] private int _upgradeLevel;
        public int UpgradeLevel { get => _upgradeLevel; set { _upgradeLevel = value; } }
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

        private WeaponAlgorithm _algorithm;

        private PlayerStats _playerStats;

        private int _level;
        public int Level { get => _level; set { SetField(ref _level, value); } }

        // TODO: check for cases when _algorithm == null !!!!
        public float Price { get => _algorithm.GetPrice(Level); private set { } }
        
        public float NextPrice { get => _algorithm.GetNextPrice(Level);}

        private float _value;
        public float Value { get => _algorithm.GetValue(Level, _upgradeLevel, _playerStats.DPSMultiplier); set { _value = value; } }

        public float NextValue { get => _algorithm.GetNextValue(Level); }

        private int _upgradeLevel;
        public int UpgradeLevel { get => _upgradeLevel; set { SetField(ref _upgradeLevel, value); } }

        public float UpgradePrice { get => _algorithm.GetUpgradePrice(_upgradeLevel); }
        public float NextUpgradePrice { get => _algorithm.GetNextUpgradePrice(_upgradeLevel);}

        public WeaponStat(int level, WeaponData weaponStats, PlayerStats playerStats, WeaponAlgorithm algorithm)
        {
            // Init this WeaponStat's stats
            Level = level;
            //_upgradeLevel = weaponStats.;

            _algorithm = algorithm;
            _playerStats = playerStats;
        }

        public WeaponStat(StatData statData, PlayerStats playerStats, WeaponAlgorithm algorithm)
        {
            // Init this WeaponStat's stats
            Level = statData.Level;
            _upgradeLevel = statData.UpgradeLevel;

            _algorithm = algorithm;
            _playerStats = playerStats;
        }

        public WeaponStat(int level, int upgradeLevel, WeaponAlgorithm algorithm)
        {
            Level = level;
            _upgradeLevel = upgradeLevel;
            this._algorithm = algorithm;
        }

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
        public WeaponAlgorithms Algorithms { get; private set; }

        private Wave _wave;

        private float _nextShotTime;
        private float _msBetweenShots = 200;

        public void Init(WeaponAlgorithms algorithmHolder, WeaponData data, PlayerStats playerStats)
        {
            InitMessageHandler();

            Algorithms = algorithmHolder;
            _playerStats = playerStats;

            DPS = new WeaponStat(data.DPS, playerStats, algorithmHolder.DPS);
            DMG = new WeaponStat(data.DMG, playerStats, algorithmHolder.DMG);
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
    }
}