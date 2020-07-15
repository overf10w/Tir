using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    public class UpgradeBtnClickEventArgs : EventArgs
    {
        public Upgrades.Upgrade Upgrade { get; }
        public UpgradeBtnClickEventArgs(Upgrades.Upgrade upgrade)
        {
            Upgrade = upgrade;
        }
    }

    // TODO: 
    // 0. Create an Upgrades Research Center View ((with canvas and all), which listens to when playerModel.playerStats/playerModel.teamWeapons has changed)
    // 1. Don't forget to create such playerStats: SilverCookieArtifact, GoldCookieArtifact, BronzeCookieArtifact, HolyCookieGraal, PhilosophyCookie, MeteorCookie
    // 2. Don't forget to create such playerStats: CookieAntiMatterMult
    [CreateAssetMenu(fileName = "Upgrades", menuName = "ScriptableObjects/Ugprades", order = 6)]
    public class Upgrades : ScriptableObject
    {
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
            [SerializeField] private string _indexer;
            public string Indexer { get => _indexer; set { } }

            [SerializeField] private string _name;
            public string Name { get => _name; set { } }

            [SerializeField] private string _description;
            public string Description { get => _description; set { } }

            [SerializeField] private float _price;
            public float Price { get => _price; set { } }

            [SerializeField] private float _amount;
            public float Amount { get => _amount; set { } }

            public Criteria[] criterias;

            [SerializeField] private bool _isActive;
            public bool IsActive { get => _isActive; set { SetField(ref _isActive, value); } }
            //public bool IsActive()
            //{
            //    foreach (var criteria in criterias)
            //    {
            //        if (criteria.IsSatisfied)
            //        {
            //            return true;
            //        }
            //    }
            //    return false;
            //}

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
                Debug.Log("PlayerStats: SetField<T>(): Invoked");
                if (EqualityComparer<T>.Default.Equals(field, value))
                {
                    return false;
                }
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            #endregion
        }

        private PlayerStats _playerStats;
        // private Dictionary<string, Weapon> teamWeapons; // to keep an eye on weapons
        public Upgrade[] upgrades;
    }
}