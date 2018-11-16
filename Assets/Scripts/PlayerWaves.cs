using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWaves.Asset", menuName = "Character/PlayerWaves")]
public class PlayerWaves : ScriptableObject
{
    public Wave[] waves;
    private Wave currentWave;
}