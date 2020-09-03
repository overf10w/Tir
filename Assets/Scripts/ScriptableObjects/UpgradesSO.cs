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

        [SerializeField] private StatsLists _statsList;
        public string StatsList => _statsList.ToString();
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
                StatsList statsList = (StatsList)PlayerStats[StatsList];
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

    // Keep in sync with Resources/SO/PlayerData scriptable object
    public enum StatsLists
    {
        TeamSkillsList,
        ClickGunSkillsList
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