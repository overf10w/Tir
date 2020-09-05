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
        [SerializeField] private Upgrade _upgrade;

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
                StatsList statsList = (StatsList)PlayerStats[StatsList];
                PlayerStat skill = statsList.List.Find(sk => sk.Name == _stat);

                if (_upgrade != null)
                {
                    if (_upgrade.IsActive)
                    {
                        return false;
                    }
                }

                switch (_thresholdComparison)
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
}