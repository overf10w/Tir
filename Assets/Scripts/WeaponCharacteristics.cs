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

    public float dps, nextDps;
    public float cost, nextCost;

    public WeaponType weaponType;

    public WeaponCharacteristics(float baseCost, float baseDps, int level)
    {
        this.baseCost = cost = baseCost;
        this.baseDps = baseDps;
        this.level = level;
    }

    public void UpdateSelf()
    {
        if (level == 0)
        {
            nextCost = baseCost;
            nextDps = baseDps;
            level += 2;
        }

        cost = nextCost;
        dps = nextDps;

        nextCost = (int)Math.Floor(baseCost * (float)Math.Pow(1.10f, level));
        nextDps = baseDps * level;

        level++;
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
