using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WeaponType
{
    NONE,
    PISTOL,
    DOUBLE_PISTOL
}

[System.Serializable]
public class WeaponCharacteristics
{
    public int level;

    public float baseDps;
    public float baseCost;

    private float dps, nextDps;
    private float cost, nextCost;

    public WeaponType weaponType;

    public WeaponCharacteristics(float baseCost, float baseDps, int level)
    {
        this.baseCost = baseCost;
        this.baseDps = baseDps;
        this.level = level;

        this.cost = 0;
        this.dps = 0;
    }
    
    // TODO: remove these props

    public float Dps
    {
        get { return dps; }
        set { dps = value; }
    }

    public float Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public float NextCost
    {
        get { return nextCost; }
        set { nextCost = value; }
    }

    public float NextDps
    {
        get { return nextDps; }
        set { nextDps = value; }
    }

    public void Init(int lvl)
    {
        level = lvl;
        if (level == 0)
        {
            nextCost = baseCost;
            nextDps = baseDps;

            cost = baseCost;
            dps = 0;

            return;
        }

        cost = (int)Math.Floor(baseCost * (float)Math.Pow(1.10f, level));
        dps = baseDps * level;

        var nxtLvl = lvl + 1;

        nextCost = (int)Math.Floor(baseCost * (float)Math.Pow(1.10f, nxtLvl));
        nextDps = baseDps * nxtLvl;
    }

    public void UpdateSelf()
    {
        level++;

        cost = nextCost;
        dps = nextDps;

        nextCost = (int)Math.Floor(baseCost * (float)Math.Pow(1.10f, level + 1));
        nextDps = baseDps * (level + 1);
    }

    public float nextShotTime;

    public float msBetweenShots = 200;

    public void Fire(Wave wave)
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            IDestroyable cube = wave.Cubes.ElementAtOrDefault(new System.Random().Next(wave.Cubes.Count));
            if ((MonoBehaviour)cube != null)
            {
                cube.TakeDamage(dps);
            }
            else
            {
                Debug.Log("Weapon: " + weaponType + ": There's no cube there!");
            }
        }
    }
}
