using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct CubeStat
{
    public int HP;
    public int gold;
}

[CreateAssetMenu(fileName = "CubeStats.Asset", menuName = "Character/Cube")]
public class CubeStats : ScriptableObject
{
    public CubeStat stats;
}
