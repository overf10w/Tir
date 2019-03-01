using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using UnityEngine;

public class WeaponArgs : EventArgs
{
    public WeaponModel weaponModel;
    public Weapon sender;

    public WeaponArgs(Weapon sender, WeaponModel weaponModel)
    {
        this.sender = sender;
        this.weaponModel = weaponModel;
    }
}

public delegate void WeaponChanged(WeaponArgs e);

// TODO: value updated
public delegate void GoldChanged(float value);
public delegate void AutoFireUpdated(float seconds);
public delegate void AttackUpdated(float value);
public delegate void LevelChanged(int level);

[System.Serializable]
public class PlayerModel
{
    // boiler-plate
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    protected bool SetField<T>(ref T field, T value, string propertyName)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public event WeaponChanged OnWeaponChanged;
    public event AutoFireUpdated OnAutoFireUpdated;
    public event AttackUpdated OnAttackUpdated;

    [Header("Skills")]
    [SerializeField]
    public float currentDamage;
    public float currentAutoFire;

    private int damageLvl;
    private int autoFireLvl;

    private Weapon pistol;
    private int pistolLvl;    
    
    private Weapon doublePistol;
    private int doublePistolLvl;

    [SerializeField] private float autoFireDuration;

    public PlayerModel(PlayerStats playerStats)
    {
        InitStats(playerStats);

        currentDamage = 2.0f;
        currentAutoFire = 0.0f;

        var pistolPrefab = Resources.Load<Weapon>("Prefabs/WeaponPistolPrefab");
        pistol = UnityEngine.Object.Instantiate(pistolPrefab, GameObject.FindObjectOfType<PlayerView>().transform) as Weapon;
        pistol.weaponType = WeaponType.PISTOL;
        pistol.weaponModel = new WeaponModel(12, 2, 0);
        pistol.weaponModel.Init(pistolLvl);

        var doublePistolPrefab = Resources.Load<Weapon>("Prefabs/WeaponPistolDoublePrefab");
        doublePistol = UnityEngine.Object.Instantiate(doublePistolPrefab, GameObject.FindObjectOfType<PlayerView>().transform) as Weapon;
        doublePistol.weaponType = WeaponType.DOUBLE_PISTOL;
        doublePistol.weaponModel = new WeaponModel(14, 4, 0);
        doublePistol.weaponModel.Init(doublePistolLvl);
    }

    [Header("Player Stats")]
    [SerializeField]
    private float gold;
    public float Gold
    {
        get { return gold; }
        set
        {
            SetField(ref gold, value, "Gold");
        }
    }


    public void UpdatePistol()
    {
        if (gold >= pistol.weaponModel.Cost)
        {
            gold -= pistol.weaponModel.Cost;
            pistol.weaponModel.UpdateSelf();
            OnWeaponChanged?.Invoke(new WeaponArgs(pistol, pistol.weaponModel));
            pistolLvl = pistol.weaponModel.level;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }


    public void UpdateDoublePistol()
    {
        if (gold >= doublePistol.weaponModel.Cost)
        {
            gold -= doublePistol.weaponModel.Cost;
            doublePistol.weaponModel.UpdateSelf();
            OnWeaponChanged?.Invoke(new WeaponArgs(doublePistol, doublePistol.weaponModel));
            doublePistolLvl = doublePistol.weaponModel.level;
        }
        else
        {
            Debug.LogWarning("Sorry bro no money =((");
        }
    }

    public void ResetPlayerStats()
    {
        gold = 0;
        damageLvl = 0;
        pistolLvl = 0;
        doublePistolLvl = 0;
        autoFireLvl = 0;
        autoFireDuration = 0.0f;
    }

    public void InitStats(PlayerStats playerStats)
    {
        gold = playerStats._gold;
        damageLvl = playerStats._damageLvl;
        pistolLvl = playerStats._pistolLvl;
        doublePistolLvl = playerStats._doublePistolLvl;
        autoFireLvl = playerStats._autoFireLvl;
        autoFireDuration = playerStats._autoFireDuration;
    }

    public PlayerStats GetStats()
    {
        return new PlayerStats
        {
            _gold = gold,
            _damageLvl = damageLvl,
            _pistolLvl = pistolLvl,
            _doublePistolLvl = doublePistolLvl,
            _autoFireLvl = autoFireLvl,
            _autoFireDuration = autoFireDuration,
            _timeLastPlayed = DateTime.Now.Ticks
        };
    }

    // We use this shit so UI can be updated when weaponData is loaded
    public void InvokeWeaponChanged()
    {
        OnWeaponChanged?.Invoke(new WeaponArgs(doublePistol, doublePistol.weaponModel));
        OnWeaponChanged?.Invoke(new WeaponArgs(pistol, pistol.weaponModel));
    }
}

[System.Serializable]
public class PlayerStats
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
