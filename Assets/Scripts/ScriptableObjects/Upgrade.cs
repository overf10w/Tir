using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 7)]
    [System.Serializable]
    public class Upgrade : ScriptableObject, INotifyPropertyChanged
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
            foreach (var crit in criterias)
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
        [SerializeField] private StatsLists _statsList;
        public string StatsList => _statsList.ToString();

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
                foreach (var crit in criterias)
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
}