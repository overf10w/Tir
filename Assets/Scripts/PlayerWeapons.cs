using System.Collections.Generic;
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

    public WeaponCharacteristics(float baseCost, float baseDps, WeaponType weaponType)
    {
        this.baseCost = cost = nextCost = baseCost;
        this.baseDps = dps = nextCost = baseDps;
        this.weaponType = weaponType;
    }
}

[System.Serializable]
public class WeaponData
{
    public WeaponCharacteristics[] lvls;
    public string name;
}

[CreateAssetMenu(fileName = "PlayerWeapons.Asset", menuName = "Character/PlayerWeapons")]
public class PlayerWeapons : ScriptableObject
{
    public List<WeaponData> weapons;
}
