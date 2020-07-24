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

    // STAGE_0 - pure data-driven approach (inspired by this: https://developer.valvesoftware.com/wiki/Dota_2_Workshop_Tools/Scripting/Abilities_Data_Driven)
    // TO get the Value, 
    // 1) We just pass the PlayerModel.PlayerStats.Skills to the algorithm
    //      1.1) PlayerModel.PlayerStats.Skills being an array of [Serializable] Skill class with fields { skillName: string, skillValue: float } 
    //          1.2) PlayerModel.PlayerStats.Skills[] emits when each skill changed, so the WeaponStat can be notified
    // 3) Algorithm traverses each skillValue and multiplies the Value by it
    // 4) In Upgrades.cs: nothing really changes on the editor interface -- the "indexer" field stays the same
    //      4.1) What changes now is the way we access the Skills: instead of object indexer, we access the skills array (and look for string match: indexer == skill.Name) <-- note skill.Name should be unique
    // STAGE_0 - optional (implement in later game): In Upgrades.cs: each Upgrade has an array of impacted indexers instead of one 


    // STAGE_1 - Combined approach, more friendly to game designer (with unity SO) 
    // 1) Actually, make a Skill a Scriptable Object
    //      1.1) This implies the gamedesigner should create as many Skill objects as he pleases and assign them to the array in the Resources/SO/PlayerData/Player/PlayerData inspector
    // 2) In Upgrade class, now instead of field Indexer, there should be Skill field : Skill, which references the skill the Upgrade operates on



    // (LP)(each of the props can be of type CharacterStat (https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/)) - but that's not obligatory: 
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

        private int _level;
        public int Level { get => _level; set { SetField(ref _level, value); } }

        // TODO: check for cases when _algorithm == null !!!!
        public float Price { get => _algorithm.GetPrice(Level); private set { } }
        public float NextPrice { get => _algorithm.GetNextPrice(Level);}

        private float _value;
        public float Value { get => _algorithm.GetValue(_playerStats, Level, _upgradeLevel); set { _value = value; } }
        //public float Value { get => _algorithm.GetValue(Level, _upgradeLevel, _playerStats.DPSMultiplier); set { _value = value; } }
        public float NextValue { get => _algorithm.GetNextValue(Level); }

        private int _upgradeLevel;
        public int UpgradeLevel { get => _upgradeLevel; set { SetField(ref _upgradeLevel, value); } }

        public float UpgradePrice { get => _algorithm.GetUpgradePrice(_upgradeLevel); }
        public float NextUpgradePrice { get => _algorithm.GetNextUpgradePrice(_upgradeLevel);}

        private WeaponAlgorithm _algorithm;

        private PlayerStats _playerStats;

        public WeaponStat(StatData statData, PlayerStats playerStats, WeaponAlgorithm algorithm)
        {
            Level = statData.Level;
            _upgradeLevel = statData.UpgradeLevel;

            _algorithm = algorithm;
            _playerStats = playerStats;

            //_playerStats.PropertyChanged += HandlePlayerStatsChange;
            //_playerStats.Skills.Changed += Skills_Changed;
            _playerStats.TeamSkills.StatChanged += SkillsContainer_SkillChanged;
        }

        private void SkillsContainer_SkillChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is PlayerStat)
            {
                PlayerStat skill = (PlayerStat)sender;
                Debug.Log("Weapon.cs: Notified of Skill change: " + skill.Name);
            }

        }

        //private void Skills_Changed(int ind)
        //{
        //    Debug.Log("Weapon: Notified of PlayerSkills[" + ind + "] Change");
        //    throw new System.NotImplementedException();
        //}

        // TODO: setter
        public void Upgrade()
        {
            _upgradeLevel++;
        }

        //private void HandlePlayerStatsChange(object sender, PropertyChangedEventArgs args)
        //{
        //    Debug.Log("Weapon: Notified of PlayerStatsChange");
        //}
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

        public void Init(WeaponData data, PlayerStats playerStats)
        {
            InitMessageHandler();

            Algorithms = data.algorithms;
            _playerStats = playerStats;

            DPS = new WeaponStat(data.DPS, playerStats, data.algorithms.DPS);
            DMG = new WeaponStat(data.DMG, playerStats, data.algorithms.DMG);
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
                // TODO: wave.Cubes.PickRandom()
                IDestroyable cube = wave.Cubes.ElementAtOrDefault(new System.Random().Next(wave.Cubes.Count));
                //IDestroyable cube = wave.Cubes.PickRandom();
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