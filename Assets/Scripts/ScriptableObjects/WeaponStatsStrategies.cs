using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class WeaponStatsAlgorithm
    {
        public string name;
        
        public float basePrice;

        [Tooltip("Optimal value between (1.07 - 1.15)")]
        public float multiplier;
        public float level;

        public float GetPrice()
        {
            return basePrice * Mathf.Pow(multiplier, level);
        }

        public float GetPrice(int level)
        {
            return basePrice * Mathf.Pow(multiplier, level);
        }
    }

    [CreateAssetMenu(fileName = "WeaponStatsStrategies", menuName = "ScriptableObjects/WeaponStatsStrategies", order = 1)]
    public class WeaponStatsStrategies : ScriptableObject
    {
        public WeaponStatsAlgorithm[] algorithms;
    }
}