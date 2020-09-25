using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class IterationMultiplier
    {
        [field: NonSerialized]
        public PlayerStats PlayerStats { get; set; }

        [SerializeField] private int _everyNth;

        [SerializeField] private float _amount;

        [SerializeField] private StatsLists _statsList;
        public string StatsList => _statsList.ToString();

        [SerializeField, HideInInspector] private string _stat; // SerializeField: accessible through FindPropertyRelative("_stat"); isn't shown in the inspector

        public float Multiplier
        {
            get
            {
                StatsList statsList = (StatsList)PlayerStats[StatsList];
                PlayerStat playerStat = statsList.List.Find(sk => sk.Name == _stat);

                Debug.Log("1: " + (1.0f / _everyNth) * _amount * playerStat.Value);

                return (1.0f / _everyNth) * _amount * playerStat.Value;
            }
        }
    }
}