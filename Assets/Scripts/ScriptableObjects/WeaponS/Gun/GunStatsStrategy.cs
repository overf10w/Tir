using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GunStatsStrategy", menuName = "ScriptableObjects/GunStatsStrategy", order = 4)]
    public class GunStatsStrategy : ScriptableObject
    {
        public WeaponStatsAlgorithmsHolder algorithm;
    }
}