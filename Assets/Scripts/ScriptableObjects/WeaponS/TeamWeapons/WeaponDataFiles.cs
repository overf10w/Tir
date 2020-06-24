using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "WeaponDataFiles", menuName = "ScriptableObjects/WeaponDataFiles", order = 2)]
    [System.Serializable]
    public class WeaponDataFiles : ScriptableObject
    {
        public WeaponStatData[] weapons;
    }
}