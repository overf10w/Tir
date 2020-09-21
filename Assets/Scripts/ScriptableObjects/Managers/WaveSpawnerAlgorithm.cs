using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "WaveSpawnerAlgorithm", menuName = "ScriptableObjects/WaveSpawnerAlgorithm")]
    public class WaveSpawnerAlgorithm : ScriptableObject
    {
        #region Singleton
        /* Singleton */
        static WaveSpawnerAlgorithm instance;

        public static WaveSpawnerAlgorithm Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WaveSpawnerAlgorithm();
                }
                return instance;
            }
        }

        private WaveSpawnerAlgorithm() { }
        #endregion

        [Serializable]
        private class AlgorithmData
        {
            [SerializeField] private float _baseValue;
            public float BaseValue => _baseValue;

            [SerializeField] private float _coefficient;
            public float Coefficient => _coefficient;

            [SerializeField] private float _offset;
            public float Offset => _offset;

        }

        [Header("(BaseValue ^ Level) * Coefficient + Offset")]
        [SerializeField] private AlgorithmData _waveHP;

        [Header("(BaseValue ^ Level) * Coefficient + Offset")]
        [SerializeField] private AlgorithmData _waveGold;

        public float GetWaveHp(int level)
        {
            return Mathf.Pow(_waveHP.BaseValue, level) * _waveHP.Coefficient + _waveHP.Offset;
        }

        public float GetWaveGold(int level)
        {
            return Mathf.Pow(_waveGold.BaseValue, level) * _waveGold.Coefficient + _waveGold.Offset;
        }

        public static float GetLevelGold(int level)
        {
            return Mathf.Pow(instance._waveGold.BaseValue, level) * instance._waveGold.Coefficient + instance._waveGold.Offset;
        }
    }
}