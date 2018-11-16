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
// (1)
// public PlayerWeapons pw;                 // assign in editor
// public List<WeaponData> weapons = pw.weapons;
// then
// (2)
// public WeaponData gun; ---> gun = 

public class CustomArgs : EventArgs
{
    public WeaponData weaponData;

    public WeaponCharacteristics currentPistolCharacteristics;

    public CustomArgs(WeaponData weaponData, WeaponCharacteristics weaponCharacteristics)
    {
        this.weaponData = weaponData;
        this.currentPistolCharacteristics = weaponCharacteristics;
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
    [SerializeField]
    private PlayerWeapons pw;

    // Pistol
    [SerializeField]
    private WeaponData pistolData;
    public WeaponCharacteristics currentPistol;
    public int _pistolLvl;    
    
    // M4A1
    [SerializeField]
    private WeaponData doublePistolData;
    public WeaponCharacteristics currentDoublePistol;
    public int _doublePistolLvl;

    public void UpdateCurrentSkills()
    {
        currentDamage = skills.DamageLvls[_damageLvl];
        currentAutoFire = skills.AutoFireLvls[_autoFireLvl];

        pistolData = pw.weapons.Find(i => i.name == "Pistol");
        currentPistol = pistolData.lvls[_pistolLvl];

        doublePistolData = pw.weapons.Find(i => i.name == "DoublePistol");
        currentDoublePistol = doublePistolData.lvls[_doublePistolLvl];

        //_currentWave = 
    }

    [Header("Player Stats")]
    #region GOLD
    [SerializeField]
    private int _gold;
    public int Gold
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

    #region PISTOL
    public delegate void WeaponChanged(CustomArgs e);
    public event WeaponChanged OnWeaponChanged;
    public WeaponCharacteristics Pistol
    {
        get
        {
            return pistolData.lvls[_pistolLvl]; 
        }
    }
    public void UpdatePistol()
    {
        int nextInd = _pistolLvl + 1;
        if (nextInd > pistolData.lvls.Length - 1)
        {
            Debug.LogWarning("This is the highest level");
            return;
        }
        if (_gold >= pistolData.lvls[nextInd].goldWorth)
        {
            _gold -= pistolData.lvls[nextInd].goldWorth;
            if (OnWeaponChanged != null)
            {
                OnWeaponChanged(new CustomArgs(pistolData, pistolData.lvls[nextInd]));
            }
            //OnPistolChanged(pistolData.lvls[nextInd]);
            _pistolLvl = nextInd;
            currentPistol = pistolData.lvls[_pistolLvl];
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    #endregion

    #region DOUBLE_PISTOL
    public WeaponCharacteristics DoublePistol
    {
        get
        {
            return doublePistolData.lvls[_doublePistolLvl]; 
        }
    }
    public void UpdateDoublePistol()
    {
        int nextInd = _doublePistolLvl + 1;
        if (nextInd > doublePistolData.lvls.Length - 1)
        {
            Debug.LogWarning("This is the highest level");
            return;
        }
        if (_gold >= doublePistolData.lvls[nextInd].goldWorth)
        {
            _gold -= doublePistolData.lvls[nextInd].goldWorth;
            if (OnWeaponChanged != null)
            {
                OnWeaponChanged(new CustomArgs(doublePistolData, doublePistolData.lvls[nextInd]));
            }
            //OnPistolChanged(pistolData.lvls[nextInd]);
            _doublePistolLvl = nextInd;
            currentDoublePistol = doublePistolData.lvls[_doublePistolLvl];
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    #endregion

    #region CURRENT_WAVE
    [SerializeField]
    private int _currentWave;
    public Wave CurrentWave
    {
        get { return playerWaves.waves[_currentWave]; }
    }
    public void UpdateCurrentWave()
    {
        int nextInd = _currentWave + 1;
        if (nextInd > playerWaves.waves.Length - 1)
        {
            Debug.LogWarning("This is the highest wave level");
            return;
        }
        _currentWave = nextInd;
        if (OnCurrentWaveChanged != null)
        {
            OnCurrentWaveChanged(playerWaves.waves[_currentWave]);
        }
    }
    public delegate void CurrentWaveChanged(Wave value);
    public event CurrentWaveChanged OnCurrentWaveChanged;
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
    public int _gold;
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
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";

    public PlayerDB playerDb;

    void OnEnable()
    {
        #if UNITY_EDITOR
        playerDb.UpdateCurrentSkills();
        #endif
        #if UNITY_STANDALONE
        ReadSelf();
        playerDb.UpdateCurrentSkills();
        #endif
    }

    // TODO: you'd better write this logic somewhere in monobehaviour,
    // as SO's OnDisable() not always called(?)
    void OnDisable()
    {
        #if UNITY_STANDALONE
        WriteSelf();
        #endif
    }

    public void ReadSelf()
    {
        string dataAsJson = File.ReadAllText(Application.dataPath + gameDataProjectFilePath);
        Stats stats = JsonUtility.FromJson<Stats>(dataAsJson);
        if (stats != null)
        {
            playerDb.InitStats(stats);
        }
        else
        {
            Debug.Log("NULLLLLLLL =((((((((((((((((");
        }
    }

    public void WriteSelf()
    {
        Stats save = playerDb.ReturnStats();
        string dataAsJson = JsonUtility.ToJson(save);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        Debug.Log("DataAsJson: " + dataAsJson);
        File.WriteAllText(filePath, dataAsJson);
    }

    public void Reset()
    {
        // Write resetted stats
        Stats resettedStats = new Stats();
        string dataAsJson = JsonUtility.ToJson(resettedStats);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

        playerDb.ResetPlayerStats();
    }
}
