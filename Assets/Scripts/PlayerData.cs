﻿using System;
using System.IO;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [SerializeField] public int level;
    [SerializeField] public int value;
    [SerializeField] public int goldWorth;
}

// TODO: 
// 1. [+] Event delegates out of class
// 2. [+] Custom EventArgs for gold, currentDamage, currentAutoFireDuration, etc.
// 3. [+] In initPlayer, not to FIND weapons, but CREATE the weapons according to State data...
// 4. Since PlayerDB is non-monobehaviour, TRY to use constructors!!!

public class WeaponArgs : EventArgs
{
    public WeaponCharacteristics weaponCharacteristics;
    public Weapon sender;

    public WeaponArgs(Weapon sender, WeaponCharacteristics weaponCharacteristics)
    {
        this.sender = sender;
        this.weaponCharacteristics = weaponCharacteristics;
    }
}

public delegate void WeaponChanged(WeaponArgs e);
public delegate void GoldChanged(float value);
public delegate void AutoFireUpdated(float seconds);
public delegate void AttackUpdated(float value);
public delegate void LevelChanged(int level);

[System.Serializable]
public class PlayerData
{
    public event WeaponChanged OnWeaponChanged;
    public event GoldChanged OnGoldChanged;
    public event AutoFireUpdated OnAutoFireUpdated;
    public event AttackUpdated OnAttackUpdated;
    public event LevelChanged OnLevelChanged;

    public long _timeLastPlayed;

    [Header("Skills")]
    [SerializeField]
    private PlayerSkills skills;
    public Skill currentDamage;
    public Skill currentAutoFire;

    private int _damageLvl;
    private int _autoFireLvl;

    [Header("Waves")]
    [SerializeField]
    public PlayerWaves playerWaves;

    private Weapon pistol;
    private int _pistolLvl;    
    
    private Weapon doublePistol;
    private int _doublePistolLvl;

    public int _level;

    public void InitPlayer()
    {
        currentDamage = skills.DamageLvls[_damageLvl];
        currentAutoFire = skills.AutoFireLvls[_autoFireLvl];

        var pistolPrefab = Resources.Load<Weapon>("Prefabs/WeaponPistolPrefab");
        pistol = UnityEngine.Object.Instantiate(pistolPrefab, GameObject.FindObjectOfType<Player>().transform) as Weapon;
        pistol.weaponType = WeaponType.PISTOL;
        pistol.weaponCharacteristics = new WeaponCharacteristics(12, 2, 0);
        pistol.weaponCharacteristics.Init(_pistolLvl);

        var doublePistolPrefab = Resources.Load<Weapon>("Prefabs/WeaponPistolDoublePrefab");
        doublePistol = UnityEngine.Object.Instantiate(doublePistolPrefab, GameObject.FindObjectOfType<Player>().transform) as Weapon;
        doublePistol.weaponType = WeaponType.DOUBLE_PISTOL;
        doublePistol.weaponCharacteristics = new WeaponCharacteristics(14, 4, 0);
        doublePistol.weaponCharacteristics.Init(_doublePistolLvl);
    }

    [Header("Player Stats")]
    #region GOLD
    [SerializeField]
    private float _gold;
    public float Gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            if (OnGoldChanged != null)
                OnGoldChanged(value);
        }
    }
    #endregion

    #region PISTOL
    public void UpdatePistol()
    {
        if (_gold >= pistol.weaponCharacteristics.Cost)
        {
            _gold -= pistol.weaponCharacteristics.Cost;
            pistol.weaponCharacteristics.UpdateSelf();
            OnWeaponChanged?.Invoke(new WeaponArgs(pistol, pistol.weaponCharacteristics));
            _pistolLvl = pistol.weaponCharacteristics.level;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    #endregion

    #region DOUBLE_PISTOL
    public void UpdateDoublePistol()
    {
        if (_gold >= doublePistol.weaponCharacteristics.Cost)
        {
            _gold -= doublePistol.weaponCharacteristics.Cost;
            doublePistol.weaponCharacteristics.UpdateSelf();
            OnWeaponChanged?.Invoke(new WeaponArgs(doublePistol, doublePistol.weaponCharacteristics));
            _doublePistolLvl = doublePistol.weaponCharacteristics.level;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    #endregion

    #region DAMAGE_SKILL
    [SerializeField]
    public Skill Damage
    {
        get
        {
            return skills.DamageLvls[_damageLvl];
        }
    }
    public Skill NextDamage
    {
        get
        {
            int nextInd = _damageLvl + 1;
            if (nextInd > skills.DamageLvls.Length - 1)
            {
                return skills.DamageLvls[_damageLvl];
            }
            else
            {
                return skills.DamageLvls[nextInd];
            }
        }
    }
    public void UpdateDamage()
    {
        int nextInd = _damageLvl + 1;
        if (nextInd > skills.DamageLvls.Length - 1)
        {
            Debug.LogWarning("There's no more skills to unlock");
            return;
        }
        if (_gold >= skills.DamageLvls[nextInd].goldWorth)
        {
            _gold -= skills.DamageLvls[nextInd].goldWorth;

            OnAttackUpdated?.Invoke(skills.DamageLvls[nextInd].value);
            _damageLvl = nextInd;

            currentDamage = skills.DamageLvls[_damageLvl];
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    #endregion

    #region AUTOFIRE_SKILL
    [SerializeField] private float _autoFireDuration;
    public void UpdateAutoFire()
    {
        int _nextInd = _autoFireLvl + 1;
        if (_nextInd > skills.AutoFireLvls.Length - 1)
        {
            Debug.Log("AutoShoot: there's no more levels to unlock");
            return;
        }
        if (_gold >= skills.AutoFireLvls[_nextInd].goldWorth)
        {
            _gold -= skills.AutoFireLvls[_nextInd].goldWorth;

            _autoFireDuration = skills.AutoFireLvls[_nextInd].value;
            if (OnAutoFireUpdated != null)
                OnAutoFireUpdated(_autoFireDuration);

            _autoFireLvl = _nextInd;
            currentAutoFire = skills.AutoFireLvls[_autoFireLvl];
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    #endregion

    public void ResetPlayerStats()
    {
        //TODO: uncomment for final build
        _gold = 0;
        _level = 0;
        _damageLvl = 0;
        _pistolLvl = 0;
        _doublePistolLvl = 0;
        _autoFireLvl = 0;
        _autoFireDuration = 0.0f;
        _timeLastPlayed = 0;
        //UpdatePlayerStats();
        //InitPlayer();
    }

    public void InitStats(Stats stats)
    {
        _gold = stats._gold;
        _level = stats._level;
        _damageLvl = stats._damageLvl;
        _pistolLvl = stats._pistolLvl;
        _doublePistolLvl = stats._doublePistolLvl;
        _autoFireLvl = stats._autoFireLvl;
        _autoFireDuration = stats._autoFireDuration;
        _timeLastPlayed = stats._timeLastPlayed;
    }

    public Stats GetData()
    {
        Stats playerStats = new Stats();
        playerStats._gold = _gold;
        playerStats._level = _level;
        playerStats._damageLvl = _damageLvl;
        playerStats._pistolLvl = _pistolLvl;
        playerStats._doublePistolLvl = _doublePistolLvl;
        playerStats._autoFireLvl = _autoFireLvl;
        playerStats._autoFireDuration = _autoFireDuration;
        playerStats._timeLastPlayed = DateTime.Now.Ticks;

        return playerStats;
    }

    // TODO: we use this shit so UI can be updated when weaponData is loaded
    public void InvokeWeaponChanged()
    {
        OnWeaponChanged?.Invoke(new WeaponArgs(doublePistol, doublePistol.weaponCharacteristics));
        OnWeaponChanged?.Invoke(new WeaponArgs(pistol, pistol.weaponCharacteristics));
    }

    public void InvokeLevelChanged()
    {
        Debug.Log("INVOKED?");
        OnLevelChanged?.Invoke(_level);
    }
}

[System.Serializable]
public class Stats
{
    public float _gold;
    public int _level;
    public int _currentWave;
    public int _damageLvl;
    public int _pistolLvl;
    public int _doublePistolLvl;
    public int _autoFireLvl;
    public float _autoFireDuration;
    public long _timeLastPlayed;
}

[CreateAssetMenu(fileName = "Stats.Asset", menuName = "Character/Stats")]
public class PlayerStats : ScriptableObject
{
    
}