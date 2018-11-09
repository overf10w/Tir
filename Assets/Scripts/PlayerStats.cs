using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    // GOLD
    [SerializeField] private int _gold;
    public int Gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            if (onGoldChanged != null)
                onGoldChanged(value);
        }
    }
    public delegate void OnGoldChanged(float value);
    public event OnGoldChanged onGoldChanged;

    // CURRENT WAVE
    [SerializeField] private int _currentWave;
    public int CurrentWave
    {
        get { return _currentWave; }
        set
        {
            _currentWave = value;
            if (onCurrentWaveChanged != null)
                onCurrentWaveChanged(value);
        }
    }
    public delegate void OnCurrentWaveChanged(float value);
    public event OnCurrentWaveChanged onCurrentWaveChanged;

    // ATTACK
    [SerializeField] private int _attack;
    public int Attack
    {
        get { return _attack; }
        set
        {
            _attack = value;
            if (onAttackChanged != null)
                onAttackChanged(value);
        }
    }
    public delegate void OnAttackChanged(float value);
    public event OnAttackChanged onAttackChanged;

    // IS AUTO SHOOT
    [SerializeField] private bool _isAutoShoot;
    public bool IsAutoShoot
    {
        get { return _isAutoShoot; }
        set
        {
            _isAutoShoot = value;
            if (onIsAutoShootChanged != null)
                onIsAutoShootChanged(value);
        }
    }
    public delegate void OnIsAutoShootChanged(bool value);
    public event OnIsAutoShootChanged onIsAutoShootChanged;

    public void Nullify()
    {
        _gold = 0;
        _currentWave = 0;
        _attack = 0;
        _isAutoShoot = false;
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

