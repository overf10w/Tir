using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponCharacteristics
{
    public int level;
    public int value;
    public int goldWorth;
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
