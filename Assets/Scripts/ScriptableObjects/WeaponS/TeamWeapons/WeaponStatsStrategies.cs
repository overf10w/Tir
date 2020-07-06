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

        // TODO: add these fields: 
        // public float[] skillMultipliers;        <-- to be set by outer script, init through PlayerController/GameManager Init() chain
        // public float[] abilityMultipliers;      <-- to be set by outer script, updated through some methods, etc.

        public float GetPrice(int level)
        {
            return basePrice * Mathf.Pow(priceMultiplier, level);
        }

        public float GetNextPrice(int level)
        {
            return GetPrice(++level);
        }

        public float GetValue(float currValue, int level)
        {
            // How value should be calculated:
            // public float GetValue(int currValue, int level, int skillLevel)
            // return currValue + level * baseValue * valueMultiplier * skillLevel * baseSkillValue * skillMultiplier;
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