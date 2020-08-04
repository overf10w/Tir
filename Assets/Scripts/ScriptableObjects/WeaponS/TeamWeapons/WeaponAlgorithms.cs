using System.Collections.Generic;
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

        public float GetValue(PlayerStats playerStats, int level, int upgradeLevel)
        {
            List<PlayerStat> skills = playerStats.TeamSkills.Stats;

            float upgradeValue = upgradeLevel <= 0 ? 1 : 1 * Mathf.Pow(_upgradeValueMultiplier, upgradeLevel);
            float result = _baseValue * Mathf.Pow(_valueMultiplier, level) * level * upgradeValue;
            foreach (var skill in skills)
            {
                result *= skill.Value;
            }
            return result;
        }

        public float GetNextValue(PlayerStats playerStats, int level, int upgradeLevel)
        {
            return GetValue(playerStats, ++level, upgradeLevel);
        }
    }

    [System.Serializable]
    public class WeaponAlgorithms
    {
        [SerializeField] private WeaponAlgorithm _dps;
        public WeaponAlgorithm DPS => _dps;

        [SerializeField] private WeaponAlgorithm _dmg;
        public WeaponAlgorithm DMG => _dmg;
    }
}