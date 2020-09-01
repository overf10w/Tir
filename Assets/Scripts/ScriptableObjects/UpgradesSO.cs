using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class UpgradeData // Won't be shown directly anywhere in the editor
    {
        [SerializeField] private int id;
        public int Id { get => id; set { id = value; } }

        [SerializeField] private bool isActive;
        public bool IsActive { get => isActive; set { isActive = value; } }
    }

    [System.Serializable]
    public class Criteria
    {
        [field: NonSerialized]
        public PlayerStats PlayerStats { get; set; }

        [SerializeField] private string _statsList;
        public string SkillContainer => _statsList;
        [SerializeField] private string _stat;
        [SerializeField] private float _threshold;

        private enum Comparison
        {
            LESS,
            EQUAL,
            GREATER
        }

        [SerializeField] private Comparison _thresholdComparison;

        public bool Satisfied
        {
            get
            {
                StatsList statsList = (StatsList)PlayerStats[_statsList];
                PlayerStat skill = statsList.List.Find(sk => sk.Name == _stat);
                switch(_thresholdComparison)
                {
                    case Comparison.EQUAL:
                        return skill.Value == _threshold;
                    case Comparison.GREATER:
                        return skill.Value > _threshold;
                    case Comparison.LESS:
                        return skill.Value < _threshold;
                    default:
                        return skill.Value > _threshold;
                }
            }
        }
    }

    [System.Serializable]
    public class Upgrade : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        // In our case the events shouldn't be serialized, because:
        // 1. Had we try to serialize this event, all the MonoBehaviours subscribers would get serialized also (and MB cannot be serialized)
        [field: NonSerialized]
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

        public void Init(PlayerStats playerStats)
        {
            foreach (var crit in criterias)
            {
                crit.PlayerStats = playerStats;
            }
        }

        public void Init(PlayerStats playerStats, UpgradeData upgradeData)
        {
            IsActive = upgradeData.IsActive;
            foreach(var crit in criterias)
            {
                crit.PlayerStats = playerStats;
            }
        }

        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;

        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private string _description;
        public string Description => _description;

        [Header("Target Stat")]
        [SerializeField] private string _statsList;
        public string StatsList => _statsList;

        [SerializeField] private string _stat;
        public string Stat => _stat;

        [Header("Characteristics")]
        [SerializeField] private float _price;
        public float Price => _price;

        [SerializeField] private float _amount;
        public float Amount => _amount;

        [Header("Criterias")]
        public Criteria[] criterias;

        public bool CriteriasFulfilled
        {
            get
            {
                foreach(var crit in criterias)
                {
                    if (!crit.Satisfied)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        [SerializeField] private bool _isActive;
        public bool IsActive { get => _isActive; set { SetField(ref _isActive, value); } }
    }

    [CreateAssetMenu(fileName = "Upgrades", menuName = "ScriptableObjects/Ugprades", order = 6)]
    public class UpgradesSO : ScriptableObject
    {
        public PlayerStats PlayerStats { get; set; }
        // private Dictionary<string, Weapon> teamWeapons; // to keep an eye on weapons

        [SerializeField] private Upgrade[] _upgrades;
        public Upgrade[] Upgrades => _upgrades;

        public UpgradeData[] GetUpgradesData()
        {
            UpgradeData[] ret = new UpgradeData[Upgrades.Length];
            for (int i = 0; i < Upgrades.Length; i++)
            {
                ret[i] = new UpgradeData();
                ret[i].Id = i;
                ret[i].IsActive = Upgrades[i].IsActive;
            }
            return ret;
        }

        public void SetUpgrades(UpgradeData[] upgradeDatas)
        {
            if (upgradeDatas.Length != Upgrades.Length)
            {
                string warning =
                    "Upgrades.cs: Upgrades Save File not in sync with Upgrades Scriptable Object\n" +
                    "Setting all Upgrades: IsActive = true";
                Debug.LogWarning(warning);
                foreach (var upgrade in Upgrades)
                {
                    upgrade.IsActive = true;
                    upgrade.Init(PlayerStats);
                }
            }
            else
            {
                for (int i = 0; i < Upgrades.Length; i++)
                {
                    Upgrades[i].IsActive = upgradeDatas[i].IsActive;
                    Upgrades[i].Init(PlayerStats, upgradeDatas[i]);
                }
            }
        }
    }
}