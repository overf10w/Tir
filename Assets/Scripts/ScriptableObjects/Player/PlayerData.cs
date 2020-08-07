using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 5)]
    public class PlayerData : ScriptableObject
    {
        public PlayerStats playerStats;
    }
}