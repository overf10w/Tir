using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class UpgradeData
    {
        public int id;
        public bool isActive;
    }

    public class UpgradeBtnClickEventArgs : EventArgs
    {
        public Upgrade Upgrade { get; }
        public UpgradeBtnClickEventArgs(Upgrade upgrade)
        {
            Upgrade = upgrade;
        }
    }

    [System.Serializable]
    public class Criteria
    {
        private PlayerStats playerStats;
        public string indexer;
        public float threshold;
        public bool IsSatisfied { get => (float)playerStats[indexer] >= threshold; }
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

        [SerializeField] private string _skillContainer;
        public string SkillContainer => _skillContainer;

        [SerializeField] private string _skill;
        public string Skill => _skill;

        [SerializeField] private string _name;
        public string Name => _name; 

        [SerializeField] private string _description;
        public string Description => _description;

        [SerializeField] private float _price;
        public float Price => _price;

        [SerializeField] private float _amount;
        public float Amount => _amount;

        public Criteria[] criterias;

        [SerializeField] private bool _isActive;
        public bool IsActive { get => _isActive; set { SetField(ref _isActive, value); } }
    }

    // TODO: 
    // 0. Create an Upgrades Research Center View ((with canvas and all), which listens to when playerModel.playerStats/playerModel.teamWeapons has changed) - [done]
    // 1. Add such playerStats: SilverCookieArtifact, GoldCookieArtifact, BronzeCookieArtifact, 
    //    HolyCookieGraal, PhilosophyCookie, MeteorCookie, CookieAntiMatterMult
    [CreateAssetMenu(fileName = "Upgrades", menuName = "ScriptableObjects/Ugprades", order = 6)]
    public class Upgrades : ScriptableObject
    {
        private PlayerStats _playerStats;
        // private Dictionary<string, Weapon> teamWeapons; // to keep an eye on weapons
        public Upgrade[] upgrades;

        public UpgradeData[] GetUpgradesData()
        {
            UpgradeData[] ret = new UpgradeData[upgrades.Length];
            for (int i = 0; i < upgrades.Length; i++)
            {
                ret[i] = new UpgradeData();
                ret[i].id = i;
                ret[i].isActive = upgrades[i].IsActive;
            }
            return ret;
        }

        public void SetUpgradesData(UpgradeData[] upgradeDatas)
        {
            if (upgradeDatas.Length != upgrades.Length)
            {
                string warning =
                    "Upgrades.cs: Upgrades Save File not in sync with Upgrades Scriptable Object\n" +
                    "Setting all Upgrades: IsActive = true";
                Debug.LogWarning(warning);
                foreach (var upgrade in upgrades)
                {
                    upgrade.IsActive = true;
                }
            }
            else
            {
                for (int i = 0; i < upgrades.Length; i++)
                {
                    upgrades[i].IsActive = upgradeDatas[i].isActive;
                }
            }
        }
    }
}