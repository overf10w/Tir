using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct CubeStat
    {
        public int HP;
        public int gold;
        [Tooltip("In seconds")]
        public float takeDamageEffectDuration;
    }

    [CreateAssetMenu(fileName = "CubeStats.Asset", menuName = "Character/Cube")]
    public class CubeStats : ScriptableObject
    {
        [SerializeField] private CubeStat stats;
        public CubeStat Stats => stats;
    }
}