using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData", order = 3)]
    public class GunData : ScriptableObject
    {
        public WeaponData gunStats;
    }
}