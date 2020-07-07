using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class WeaponStatsAlgorithm
    {
        [Header("Price/Value algorithm")]
        public float basePrice;
        [Tooltip("Optimal between (1.07 - 1.15)")]
        public float priceMultiplier;
        
        public float baseValue;
        public float valueMultiplier;

        [Header("Upgrade algorithm")]
        public float baseUpgradePrice;
        public float upgradePriceMultiplier;
        public int maxUpgradeLevel;
        public float upgradeValueMultiplier = 2.0f;

        // TODO: add these fields
        // public float baseValue;
        // public float valueMultiplier;

        // TODO: add these fields:
        // public float baseUpgradePrice;
        // public float maxSkillLevel;
        // public float skillMultiplier; <-- thus, we should add skillLevel field in the Weapon

        public float GetPrice(int level)
        {
            return basePrice * Mathf.Pow(priceMultiplier, level);
        }

        public float GetNextPrice(int level)
        {
            return GetPrice(++level);
        }

        public float GetUpgradePrice(int skillLevel)
        {
            return baseUpgradePrice * Mathf.Pow(upgradePriceMultiplier, skillLevel);
        }

        public float GetNextUpgradePrice(int level)
        {
            return GetUpgradePrice(level);
        }

        public float GetValue(int level, int upgradeLevel)
        {
            // How value should be calculated: 
            // public float GetValue(int level, int skillLevel, int globalDPSMultiplier)
            // return level * baseValue * valueMultiplier * skillLevel * skillMultiplier * globalDPSMultiplier;
            float upgradeValue = upgradeLevel <= 0 ? 1 : 1 * Mathf.Pow(upgradeValueMultiplier, upgradeLevel);
            return baseValue * Mathf.Pow(valueMultiplier, level) * level * upgradeValue;
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