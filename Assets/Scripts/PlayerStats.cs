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

    public Skill[] damageLvls;        // TODO: attackLvls
    public Skill[] autoFireLvls;      // TODO: autoClickLvls

    public Skill currentDamage;
    public Skill currentAutoFire;

    public void InitStats()
    {
        currentDamage = damageLvls[_damageLvl];
        currentAutoFire = autoFireLvls[_autoFireLvl];
    }

    // GOLD
    [Header("Player Stats")]
    [Space(10)]
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

    // CURRENT WAVE
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
    private int _attack;                // TODO: we don't need backing fields (_attack no more)

    public Skill Attack
    {
        get
        {
            return damageLvls[_damageLvl];
        }
    }
    public Skill NextAttack
    {
        get
        {
            int nextInd = _damageLvl + 1;
            if (nextInd > damageLvls.Length - 1)
            {
                return damageLvls[_damageLvl];
            }
            else
            {
                return damageLvls[nextInd];
            }
        }   
    }
    public delegate void AttackUpdated(float value);
    public event AttackUpdated OnAttackUpdated;

    public void UpdateAttack()
    {
        int nextInd = _damageLvl + 1;
        if (nextInd > damageLvls.Length - 1)
        {
            Debug.LogWarning("There's no more skills to unlock");
            return;
        }
        if (_gold >= damageLvls[nextInd].goldWorth)
        {
            _gold -= damageLvls[nextInd].goldWorth;

            _attack = damageLvls[nextInd].value;
            if (OnAttackUpdated != null)
                OnAttackUpdated(damageLvls[nextInd].value);
            _damageLvl = nextInd;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }

    // AUTOFIRE
    [SerializeField] private float _autoFireDuration;
    public void UpdateAutoShoot()
    {
        int _nextInd = _autoFireLvl + 1;
        if (_nextInd > autoFireLvls.Length - 1)
        {
            Debug.Log("AutoShoot: there's no more levels to unlock");
            return;
        }
        if (_gold >= autoFireLvls[_nextInd].goldWorth)
        {
            _gold -= autoFireLvls[_nextInd].goldWorth;

            _autoFireDuration = autoFireLvls[_nextInd].value;
            if (OnAutoFireUpdated != null)
                OnAutoFireUpdated(_autoFireDuration); // TODO: dafuq is true?

            _autoFireLvl = _nextInd;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    public delegate void AutoFireUpdated(float seconds);
    public event AutoFireUpdated OnAutoFireUpdated;

    public void Nullify()
    {
        //TODO: uncomment for final build
        _gold = 0;
        _attack = 2;
        _currentWave = 0;
        _damageLvl = 0;
        _autoFireLvl = 0;
        _autoFireDuration = 0.0f;
    }
}

[CreateAssetMenu(fileName = "Stats.Asset", menuName = "Character/Stats")]
public class PlayerStats : ScriptableObject
{
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";

    public Stats stats;

    void OnEnable()
    {
        
    }

    //TODO: remove it for dev build
    void OnDisable()
    {

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
        stats.Nullify();
    }
}

