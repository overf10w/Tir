using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GunDataFile", menuName = "ScriptableObjects/GunDataFile", order = 3)]
    public class GunDataFile : ScriptableObject
    {
        public WeaponStatData gunStats;
    }
}