using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "WeaponsData", menuName = "ScriptableObjects/WeaponsData", order = 2)]
    [System.Serializable]
    public class WeaponsData : ScriptableObject
    {
        private static WeaponsData _instance;
        public static WeaponsData Instance { get => _instance; }
        // TODO:
        // private WeaponData[] _weapons;
        // public WeaponData[] Weapons => _weapons;
        public WeaponData[] weapons;
    }
}