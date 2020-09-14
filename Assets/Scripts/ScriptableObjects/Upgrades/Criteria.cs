using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class Criteria
    {
        [field: NonSerialized]
        public PlayerStats PlayerStats { get; set; }

        [SerializeField] private StatsLists _statsList;
        public string StatsList => _statsList.ToString();
        [SerializeField, HideInInspector] private string _stat; // SerializeField: accessible through FindPropertyRelative("_stat"); isn't shown in the inspector
        [SerializeField] private float _threshold;
        [SerializeField] private Upgrade[] _upgrades;

        private enum Comparison
        {
            LESS,
            EQUAL,
            GREATER,
            [InspectorName("<=")]
            LESS_EQUAL,
            [InspectorName(">=")]
            GREATER_EQUAL
        }

        [SerializeField] private Comparison _thresholdComparison;

        public bool Satisfied
        {
            get
            {
                StatsList statsList = (StatsList)PlayerStats[StatsList];
                PlayerStat playerStat = statsList.List.Find(sk => sk.Name == _stat);

                if (_upgrades != null)
                {
                    foreach (var upgrade in _upgrades)
                    {
                        if (upgrade.IsActive)
                        {
                            return false;
                        }
                    }
                }

                switch (_thresholdComparison)
                {
                    case Comparison.LESS:
                        return playerStat.Value < _threshold;
                    case Comparison.LESS_EQUAL:
                        return playerStat.Value <= _threshold;
                    case Comparison.EQUAL:
                        return playerStat.Value == _threshold;
                    case Comparison.GREATER_EQUAL:
                        return playerStat.Value >= _threshold;
                    case Comparison.GREATER:
                        return playerStat.Value > _threshold;
                    default:
                        return playerStat.Value >= _threshold;
                }
            }
        }

        public PlayerStat TargetStat
        {
            get
            {
                StatsList statsList = (StatsList)PlayerStats[StatsList];
                PlayerStat playerStat = statsList.List.Find(sk => sk.Name == _stat);
                return playerStat;
            }
            private set { }
        }
    }
}