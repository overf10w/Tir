using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int gold;
    public int currentWave;
    public int attack;
    public bool isShootingAutomatically;

    public void Nullify()
    {
        gold = 0;
        currentWave = 0;
        attack = 0;
        isShootingAutomatically = false;
    }
}

[CreateAssetMenu(fileName = "Stats.Asset", menuName = "Character/Stats")]
public class PlayerStats : ScriptableObject
{
    public Stats stats;

    // TODO: remove it for dev build
    void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        stats.Nullify();
    }
}

