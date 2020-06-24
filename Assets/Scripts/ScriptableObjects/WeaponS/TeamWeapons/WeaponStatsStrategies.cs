using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class WeaponStatsAlgorithm
    {
        public float basePrice;
        [Tooltip("Optimal value between (1.07 - 1.15)")]
        public float priceMultiplier;

        // TODO: add these fields
        // public float baseValue;
        // public float valueMultiplier;

        public float GetPrice(int level)
        {
            return basePrice * Mathf.Pow(priceMultiplier, level);
        }

        public float GetNextPrice(int level)
        {
            return GetPrice(++level);
        }

        public float GetValue(int level)
        {
            return level;
        }

        public float GetNextValue(int level)
        {
            return ++level;
        }
    }

    [System.Serializable]
    public class WeaponStatsAlgorithmsHolder
    {
        public string name;

        public WeaponStatsAlgorithm DPS;

        public WeaponStatsAlgorithm DMG;
    }

    [CreateAssetMenu(fileName = "WeaponStatsStrategies", menuName = "ScriptableObjects/WeaponStatsStrategies", order = 1)]
    public class WeaponStatsStrategies : ScriptableObject
    {
        public WeaponStatsAlgorithmsHolder[] algorithms;
    }
}