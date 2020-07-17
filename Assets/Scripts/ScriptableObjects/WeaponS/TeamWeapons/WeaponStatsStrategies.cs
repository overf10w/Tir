using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class WeaponAlgorithm
    {
        [Header("Price/Value algorithm")]
        [SerializeField] private float _basePrice;
        [Tooltip("Optimal between (1.07 - 1.15)")]
        [SerializeField] private float _priceMultiplier;
        [SerializeField] private float _baseValue;
        [SerializeField] private float _valueMultiplier;

        [Header("Upgrade algorithm")]
        [SerializeField] private float _baseUpgradePrice;
        [SerializeField] private float _upgradePriceMultiplier;
        [SerializeField] private int _maxUpgradeLevel;
        [SerializeField] private float _upgradeValueMultiplier = 2.0f;

        public float GetPrice(int level)
        {
            return _basePrice * Mathf.Pow(_priceMultiplier, level);
        }

        public float GetNextPrice(int level)
        {
            return GetPrice(++level);
        }

        public float GetUpgradePrice(int skillLevel)
        {
            return _baseUpgradePrice * Mathf.Pow(_upgradePriceMultiplier, skillLevel);
        }

        public float GetNextUpgradePrice(int level)
        {
            return GetUpgradePrice(level);
        }

        public float GetValue(int level, int upgradeLevel, float dpsMultiplier)
        {
            // How value should be calculated: 
            // public float GetValue(int level, int skillLevel, int globalDPSMultiplier)
            // return level * baseValue * valueMultiplier * skillLevel * skillMultiplier * globalDPSMultiplier;
            float upgradeValue = upgradeLevel <= 0 ? 1 : 1 * Mathf.Pow(_upgradeValueMultiplier, upgradeLevel);
            return _baseValue * Mathf.Pow(_valueMultiplier, level) * level * upgradeValue * dpsMultiplier;
        }

        public float GetNextValue(int level)
        {
            return ++level;
        }
    }

    [System.Serializable]
    public class WeaponAlgorithms
    {
        public string name;

        public WeaponAlgorithm DPS;

        public WeaponAlgorithm DMG;
    }

    // TODO: remove it whatsoever
    [CreateAssetMenu(fileName = "WeaponStatsStrategies", menuName = "ScriptableObjects/WeaponStatsStrategies", order = 1)]
    public class WeaponStatsStrategies : ScriptableObject
    {
        public WeaponAlgorithms[] algorithms;
    }
}