using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [SerializeField] public int level;
    [SerializeField] public int value;
    [SerializeField] public int goldWorth;
}

// TODO: 
// Idea: pistol to contain & calculate all its data, playerStats to write pistolLvl when game closed, read when opened
// public IWeapon pistol;
// void UpdatePistol 
// {
//     pistol.UpdateSelf();
//     if (OnWeaponChanged != null) { OnWeaponChanged(new CustomArgs(pistol)) } // OR OnWeaponChanged?.Invoke(pistol.UpdateSelf());
// }
// 

public class CustomArgs : EventArgs
{
    public WeaponCharacteristics weaponCharacteristics;

    public Weapon sender;

    public CustomArgs(Weapon sender, WeaponCharacteristics weaponCharacteristics)
    {
        this.sender = sender;
        this.weaponCharacteristics = weaponCharacteristics;
    }
}

[System.Serializable]
public class PlayerDB
{
    public Stats stats;

    [Header("Skills")]
    [SerializeField]
    private PlayerSkills skills;
    public Skill currentDamage;
    public Skill currentAutoFire;

    public int _damageLvl;
    public int _autoFireLvl;

    [Header("Waves")]
    [SerializeField]
    public PlayerWaves playerWaves;

    [Header("Weapons")]

    // Pistol
    public Weapon pistol;
    public int _pistolLvl;    
    
    // M4A1
    [SerializeField]
    public Weapon doublePistol;
    public int _doublePistolLvl;

    public void UpdateCurrentSkills()
    {
        currentDamage = skills.DamageLvls[_damageLvl];
        currentAutoFire = skills.AutoFireLvls[_autoFireLvl];

        pistol = GameObject.Find("WeaponPistolPrefab").GetComponent<Weapon>();

        Debug.Log("PISTOL: " + pistol == null);

        doublePistol = GameObject.Find("WeaponPistolDoublePrefab").GetComponent<Weapon>();

        _pistolLvl = _doublePistolLvl = 0;

        //UpdatePistol();
        //UpdateDoublePistol();

        //OnWeaponChanged?.Invoke(new CustomArgs(pistolData, pistol));
        //OnWeaponChanged?.Invoke(new CustomArgs(doublePistolData, doublePistol));
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
    public delegate void GoldChanged(float value);
    public event GoldChanged OnGoldChanged;
    #endregion

    public delegate void WeaponChanged(CustomArgs e);
    public event WeaponChanged OnWeaponChanged;

    #region PISTOL
    public void UpdatePistol()
    {
        if (_gold >= pistol.weaponCharacteristics.cost)
        {
            _gold -= pistol.weaponCharacteristics.cost;

            pistol.weaponCharacteristics.UpdateSelf();

            OnWeaponChanged?.Invoke(new CustomArgs(pistol, pistol.weaponCharacteristics));

            _pistolLvl++;
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
        if (_gold >= doublePistol.weaponCharacteristics.cost)
        {
            _gold -= doublePistol.weaponCharacteristics.cost;

            doublePistol.weaponCharacteristics.UpdateSelf();

            OnWeaponChanged?.Invoke(new CustomArgs(doublePistol, doublePistol.weaponCharacteristics));

            _doublePistolLvl++;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    #endregion

    #region CURRENT_WAVE
    [SerializeField]
    public int _currentWave;
    #endregion

    #region DAMAGE_SKILL
    public delegate void AttackUpdated(float value);
    public event AttackUpdated OnAttackUpdated;
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

            if (OnAttackUpdated != null)
                OnAttackUpdated(skills.DamageLvls[nextInd].value);
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
    public delegate void AutoFireUpdated(float seconds);
    public event AutoFireUpdated OnAutoFireUpdated;
    #endregion

    public void ResetPlayerStats()
    {
        //TODO: uncomment for final build
        _gold = 0;
        _currentWave = 0;
        _damageLvl = 0;
        _pistolLvl = 0;
        _doublePistolLvl = 0;
        _autoFireLvl = 0;
        _autoFireDuration = 0.0f;

        //UpdatePlayerStats();
        UpdateCurrentSkills();
    }

    public void InitStats(Stats stats)
    {
        _gold = stats._gold;
        _currentWave = stats._currentWave;
        _damageLvl = stats._damageLvl;
        _pistolLvl = stats._pistolLvl;
        _doublePistolLvl = stats._doublePistolLvl;
        _autoFireLvl = stats._autoFireLvl;
        _autoFireDuration = stats._autoFireDuration;
    }

    public Stats ReturnStats()
    {
        Stats playerStats = new Stats();
        playerStats._gold = _gold;
        playerStats._currentWave = _currentWave;
        playerStats._damageLvl = _damageLvl;
        playerStats._pistolLvl = _pistolLvl;
        playerStats._doublePistolLvl = _doublePistolLvl;
        playerStats._autoFireLvl = _autoFireLvl;
        playerStats._autoFireDuration = _autoFireDuration;

        return playerStats;
    }
}

[System.Serializable]
public class Stats
{
    public float _gold;
    public int _currentWave;
    public int _damageLvl;
    public int _pistolLvl;
    public int _doublePistolLvl;
    public int _autoFireLvl;
    public float _autoFireDuration;
}

[CreateAssetMenu(fileName = "Stats.Asset", menuName = "Character/Stats")]
public class PlayerStats : ScriptableObject
{
    //private string gameDataProjectFilePath = "/StreamingAssets/data.json";

    //public PlayerDB playerDb;

    //void OnEnable()
    //{
    //#if UNITY_EDITOR
    //    playerDb.UpdateCurrentSkills();
    //#endif
    //#if UNITY_STANDALONE
    //    ReadSelf();
    //    playerDb.UpdateCurrentSkills();
    //#endif
    //}

    //// TODO: you'd better write this logic somewhere in monobehaviour,
    //// as SO's OnDisable() not always called(?)
    //void OnDisable()
    //{
    //#if UNITY_STANDALONE
    //    WriteSelf();
    //#endif
    //}

    //public void ReadSelf()
    //{
    //    string dataAsJson = File.ReadAllText(Application.dataPath + gameDataProjectFilePath);
    //    Stats stats = JsonUtility.FromJson<Stats>(dataAsJson);
    //    if (stats != null)
    //    {
    //        playerDb.InitStats(stats);
    //    }
    //    else
    //    {
    //        Debug.Log("NULLLLLLLL =((((((((((((((((");
    //    }
    //}

    //public void WriteSelf()
    //{
    //    Stats save = playerDb.ReturnStats();
    //    string dataAsJson = JsonUtility.ToJson(save);
    //    string filePath = Application.dataPath + gameDataProjectFilePath;
    //    Debug.Log("DataAsJson: " + dataAsJson);
    //    File.WriteAllText(filePath, dataAsJson);
    //}

    //public void Reset()
    //{
    //    // Write resetted stats
    //    Stats resettedStats = new Stats();
    //    string dataAsJson = JsonUtility.ToJson(resettedStats);
    //    string filePath = Application.dataPath + gameDataProjectFilePath;
    //    File.WriteAllText(filePath, dataAsJson);

    //    playerDb.ResetPlayerStats();
    //}
}
