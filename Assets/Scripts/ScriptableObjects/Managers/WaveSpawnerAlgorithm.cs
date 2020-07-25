using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "WaveSpawnerAlgorithm", menuName = "ScriptableObjects/WaveSpawnerAlgorithm")]
    public class WaveSpawnerAlgorithm : ScriptableObject
    { 
        [Header ("(BaseValue ^ Level) * Coefficient + Offset")]
        [SerializeField] private float baseValue;
        [SerializeField] private float coefficient;
        [SerializeField] private float offset;

        public float GetWaveHp(int level)
        {
            return Mathf.Pow(baseValue, level) * coefficient;
        }
    }
}