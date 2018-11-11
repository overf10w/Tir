using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct AttackSkill
{
    [SerializeField] public int level;
    [SerializeField] public int attack;
    [SerializeField] public int goldWorth;
}

[System.Serializable]
public struct AutoClickSkill
{
    [SerializeField] public int level;
    [SerializeField] public int goldWorth;
}

[System.Serializable]
public class Stats
{
    [Header("Skills")]
    [SerializeField]
    public int _attackSkillIndex;

    [SerializeField]
    public int _autoClickSkillIndex;

    [SerializeField]
    public AttackSkill[] attackSkills;

    [SerializeField]
    public AutoClickSkill[] autoClickSkills;

    // GOLD
    [Header("Player Stats")]
    [Space(10)]
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
            if (_attackSkillIndex > attackSkills.Length - 1)
            {
                Debug.LogWarning("There's no more skills to unlock");
                return;
            }
            if (_gold >= attackSkills[_attackSkillIndex].goldWorth)
            {
                _attack = attackSkills[_attackSkillIndex].attack;
                if (onAttackChanged != null)
                    onAttackChanged(value);
            }
            else
            {
                Debug.LogWarning("Can't update player attack because not enough gold");
            }
        }
    }
    public delegate void OnAttackChanged(float value);
    public event OnAttackChanged onAttackChanged;

    public void UpdateAttack()
    {
        Debug.Log("Update attack called!");
        if (_attackSkillIndex > attackSkills.Length - 1)
        {
            Debug.LogWarning("There's no more skills to unlock");
            return;
        }
        if (_gold >= attackSkills[_attackSkillIndex].goldWorth)
        {
            _gold -= attackSkills[_attackSkillIndex].goldWorth;

            _attack = attackSkills[_attackSkillIndex].attack;
            if (onAttackChanged != null)
                onAttackChanged(attackSkills[_attackSkillIndex].attack);
            _attackSkillIndex++;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }

    public void UpdateAutoShoot()
    {
        //Debug.Log("Update attack called!");
        if (_autoClickSkillIndex > autoClickSkills.Length - 1) return;
        if (_gold >= autoClickSkills[_autoClickSkillIndex].goldWorth)
        {
            _gold -= autoClickSkills[_autoClickSkillIndex].goldWorth;

            _attack = attackSkills[_autoClickSkillIndex].attack;
            if (onIsAutoShootChanged != null)
                onIsAutoShootChanged(true); // TODO: dafuq is true?

            _autoClickSkillIndex++;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }

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
        //TODO: uncomment for final build
        _gold = 0;
        _attack = 2;
        _currentWave = 0;
        _attackSkillIndex = 0;
        _autoClickSkillIndex = 0;
        _isAutoShoot = false;
    }
}

[Serializable]
public class PlayerCurrentData
{
    //public Stats stats;

    public int _gold = 0;
    public int _attack = 2;
    public int _currentWave = 0;
    public int _attackSkillIndex = 0;
    public int _autoClickSkillIndex = 0;
    public bool _isAutoShoot = false;
}

[CreateAssetMenu(fileName = "Stats.Asset", menuName = "Character/Stats")]
public class PlayerStats : ScriptableObject
{
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";

    public Stats stats;

    void OnEnable()
    {
        //string dataAsJson = File.ReadAllText(Application.dataPath + gameDataProjectFilePath);
        //Stats pcd = JsonUtility.FromJson<Stats>(dataAsJson);
        //if (pcd != null)
        //{
        //    this.stats = pcd;
        //}
        //else
        //{
        //    Debug.Log("NULLLLLLLL =((((((((((((((((");
        //    //stats.Nullify();
        //}
    }

    //TODO: remove it for dev build
    void OnDisable()
    {
        //string dataAsJson = JsonUtility.ToJson(stats);
        //string filePath = Application.dataPath + gameDataProjectFilePath;
        //Debug.Log("DataAsJson: " + dataAsJson);
        //File.WriteAllText(filePath, dataAsJson);
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

