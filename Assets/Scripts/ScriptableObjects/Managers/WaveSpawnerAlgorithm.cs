using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "WaveSpawnerAlgorithm", menuName = "ScriptableObjects/WaveSpawnerAlgorithm")]
    public class WaveSpawnerAlgorithm : ScriptableObject
    {
        [Serializable]
        private class AlgorithmData
        {
            [SerializeField] public float baseValue;
            [SerializeField] public float coefficient;
            [SerializeField] public float offset;
        }

        [Header("(BaseValue ^ Level) * Coefficient + Offset")]
        [SerializeField] private AlgorithmData waveHP;

        [Header("(BaseValue ^ Level) * Coefficient + Offset")]
        [SerializeField] private AlgorithmData waveGold;

        public float GetWaveHp(int level)
        {
            return Mathf.Pow(waveHP.baseValue, level) * waveHP.coefficient + waveHP.offset;
        }

        public float GetWaveGold(int level)
        {
            return Mathf.Pow(waveGold.baseValue, level) * waveGold.coefficient + waveGold.offset;
        }
    }
}