using System;
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
    public int _attackLvl;
    public int _autoShootLvl;

    public Skill[] attackSkills;        // TODO: attackLvls
    public Skill[] autoClickSkills;     // TODO: autoClickLvls

    public Skill currentAttack;
    public Skill currentAutoClick;

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
            if (onGoldChanged != null)
                onGoldChanged(value);
        }
    }
    public delegate void OnGoldChanged(float value);
    public event OnGoldChanged onGoldChanged;

    // CURRENT WAVE
    [SerializeField]
    private int _currentWave;
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
    [SerializeField]
    private int _attack;                // TODO: we don't need backing fields (_attack no more)

    public Skill Attack
    {
        get { return attackSkills[_attackLvl]; }
    }

    public Skill NextAttack
    {
        get
        {
            int nextInd = _attackLvl + 1;
            if (nextInd > attackSkills.Length - 1)
            {
                return attackSkills[_attackLvl];
            }
            else
            {
                return attackSkills[nextInd];
            }
        }   
    }
    public delegate void OnAttackChanged(float value);
    public event OnAttackChanged onAttackChanged;

    public void UpdateAttack()
    {
        int nextInd = _attackLvl + 1;
        if (nextInd > attackSkills.Length - 1)
        {
            Debug.LogWarning("There's no more skills to unlock");
            return;
        }
        if (_gold >= attackSkills[nextInd].goldWorth)
        {
            _gold -= attackSkills[nextInd].goldWorth;

            _attack = attackSkills[nextInd].value;
            if (onAttackChanged != null)
                onAttackChanged(attackSkills[nextInd].value);
            _attackLvl = nextInd;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }

    [SerializeField] private float _autoShootDuration;
    public void UpdateAutoShoot()
    {
        int _nextInd = _autoShootLvl + 1;
        if (_nextInd > autoClickSkills.Length - 1)
        {
            Debug.Log("AutoShoot: there's no more levels to unlock");
            return;
        }
        if (_gold >= autoClickSkills[_nextInd].goldWorth)
        {
            _gold -= autoClickSkills[_nextInd].goldWorth;

            _autoShootDuration = autoClickSkills[_nextInd].value;
            if (onIsAutoShootChanged != null)
                onIsAutoShootChanged(_autoShootDuration); // TODO: dafuq is true?

            _autoShootLvl = _nextInd;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }
    public delegate void OnIsAutoShootChanged(float seconds);
    public event OnIsAutoShootChanged onIsAutoShootChanged;

    public void Nullify()
    {
        //TODO: uncomment for final build
        _gold = 0;
        _attack = 2;
        _currentWave = 0;
        _attackLvl = 0;
        _autoShootLvl = 0;
        _autoShootDuration = 0.0f;
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

