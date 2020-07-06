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
    }

    // TODO: WeaponStat => WeaponStats - [done]
    // WeaponStats to have these props:
    // Value
    // Level
    // Price
    // (each of the props can be of type CharacterStat (https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/)) - but that's not obligatory: 
    public class WeaponStat : INotifyPropertyChanged
    {
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

        private WeaponStatsAlgorithm algorithm;

        public WeaponStat()
        {

        }

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

        public WeaponStat(int level, WeaponStatsAlgorithm algorithm)
        {
            Level = level;
            this.algorithm = algorithm;
        }

        private int _level;
        public int Level { get => _level; set { SetField(ref _level, value); } }

        // TODO: make all getters arrow getters '=>' like this (where possible throughout the project)
        // TODO: check for cases when this.algorithm == null !!!!
        public float Price { get => algorithm.GetPrice(Level); set { } }
        
        public float NextPrice { get => algorithm.GetNextPrice(Level); set { } }

        private float _value;
        public float Value { get => algorithm.GetValue(_value, Level); set { _value = value; } }

        public float NextValue { get => algorithm.GetNextValue(Level); set { } }
    }

    public class Weapon : MessageHandler
    {
        // TODO (LP): (?) private set
        // TODO: this stat to become SPS - Shots Per Second
        public WeaponStat DPS { get; set; }

        // TODO (LP): (?) private set
        public WeaponStat DMG { get; set; }

        private Wave wave;

        public float nextShotTime;
        public float msBetweenShots = 200;

        private WeaponStatsAlgorithmsHolder algorithmHolder;

        public void Init(WeaponStatsAlgorithmsHolder algorithm, WeaponStatData data)
        {
            InitMessageHandler();

            this.algorithmHolder = algorithm;
            
            DPS = new WeaponStat(data.dpsLevel, algorithm.DPS);
            DMG = new WeaponStat(data.dmgLevel, algorithm.DMG);
        }

        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.WaveChanged };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.WaveChanged)
            {
                Debug.Log("Weapon.cs: On Wave Changed!");
                this.wave = (Wave)message.objectValue;
            }
        }

        public void Fire(Wave wave)
        {
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                IDestroyable cube = wave.Cubes.ElementAtOrDefault(new System.Random().Next(wave.Cubes.Count));
                if ((MonoBehaviour)cube != null)
                {
                    // TODO uncomment this!!
                    cube.TakeDamage(DPS.Value);
                }
                else
                {
                    Debug.Log("Weapon: " + ": There's no cube there!");
                }
            }
        }

        private void Update()
        {
            if (wave != null)
            {
                Fire(wave);
            }
        }
    }
}