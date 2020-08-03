using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlayerWaves.Asset", menuName = "Character/PlayerWaves")]
    public class PlayerWaves : ScriptableObject
    {
        // TODO: private Wave[] _waves;
        //       public Wave[] Waves => _waves;
        public Wave[] waves;
    }
}