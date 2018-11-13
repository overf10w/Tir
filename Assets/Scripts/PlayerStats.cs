using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [SerializeField] public int level;
    [SerializeField] public int value;
    [SerializeField] public int goldWorth;
}

[System.Serializable]
public class Stats
{
    [Header("Skills")]
    public int _damageLvl;
    public int _autoFireLvl;

    [SerializeField]
    private PlayerSkills skills;

    public Skill currentDamage;
    public Skill currentAutoFire;

    public void UpdateCurrentSkills()
    {
        currentDamage = skills.damageLvls[_damageLvl];
        currentAutoFire = skills.autoFireLvls[_autoFireLvl];
    }

    [Header("Player Stats")]
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

    [SerializeField]
    private int _currentWave;
    public int CurrentWave
    {
        get { return _currentWave; }
        set
        {
            _currentWave = value;
            if (OnCurrentWaveChanged != null)
                OnCurrentWaveChanged(value);
        }
    }
    public delegate void CurrentWaveChanged(float value);
    public event CurrentWaveChanged OnCurrentWaveChanged;

    // ATTACK
    [SerializeField]
    public Skill Damage
    {
        get
        {
            return skills.damageLvls[_damageLvl];
        }
    }
    public Skill NextDamage
    {
        get
        {
            int nextInd = _damageLvl + 1;
            if (nextInd > skills.damageLvls.Length - 1)
            {
                return skills.damageLvls[_damageLvl];
            }
            else
            {
                return skills.damageLvls[nextInd];
            }
        }   
    }
    public delegate void AttackUpdated(float value);
    public event AttackUpdated OnAttackUpdated;

    public void UpdateDamage()
    {
        int nextInd = _damageLvl + 1;
        if (nextInd > skills.damageLvls.Length - 1)
        {
            Debug.LogWarning("There's no more skills to unlock");
            return;
        }
        if (_gold >= skills.damageLvls[nextInd].goldWorth)
        {
            _gold -= skills.damageLvls[nextInd].goldWorth;

            if (OnAttackUpdated != null)
                OnAttackUpdated(skills.damageLvls[nextInd].value);
            _damageLvl = nextInd;

            currentDamage = skills.damageLvls[_damageLvl];
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }

    // AUTOFIRE
    [SerializeField] private float _autoFireDuration;
    public void UpdateAutoFire()
    {
        int _nextInd = _autoFireLvl + 1;
        if (_nextInd > skills.autoFireLvls.Length - 1)
        {
            Debug.Log("AutoShoot: there's no more levels to unlock");
            return;
        }
        if (_gold >= skills.autoFireLvls[_nextInd].goldWorth)
        {
            _gold -= skills.autoFireLvls[_nextInd].goldWorth;

            _autoFireDuration = skills.autoFireLvls[_nextInd].value;
            if (OnAutoFireUpdated != null)
                OnAutoFireUpdated(_autoFireDuration);

            _autoFireLvl = _nextInd;
            currentAutoFire = skills.autoFireLvls[_autoFireLvl];
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    public delegate void AutoFireUpdated(float seconds);
    public event AutoFireUpdated OnAutoFireUpdated;

    public void ResetPlayerStats()
    {
        //TODO: uncomment for final build
        _gold = 0;
        _currentWave = 0;
        _damageLvl = 0;
        _autoFireLvl = 0;
        _autoFireDuration = 0.0f;
        UpdateCurrentSkills();
    }
}

[CreateAssetMenu(fileName = "Stats.Asset", menuName = "Character/Stats")]
public class PlayerStats : ScriptableObject
{
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";

    public Stats stats;

    void OnEnable()
    {
        #if UNITY_EDITOR
        stats.UpdateCurrentSkills();
        #endif
        #if UNITY_STANDALONE
        ReadSelf();
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
        Stats pcd = JsonUtility.FromJson<Stats>(dataAsJson);
        if (pcd != null)
        {
            this.stats = pcd;
        }
        else
        {
            Debug.Log("NULLLLLLLL =((((((((((((((((");
            //stats.Nullify();
        }
    }

    public void WriteSelf()
    {
        string dataAsJson = JsonUtility.ToJson(stats);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        Debug.Log("DataAsJson: " + dataAsJson);
        File.WriteAllText(filePath, dataAsJson);
    }

    public void Reset()
    {
        stats.ResetPlayerStats();
    }
}

