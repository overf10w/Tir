using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlayerDataFile", menuName = "ScriptableObjects/PlayerDataFile", order = 5)]
    public class PlayerDataFile : ScriptableObject
    {
        public PlayerStats playerStats;
    }
}