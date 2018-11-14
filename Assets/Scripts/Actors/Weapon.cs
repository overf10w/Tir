using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: forget about PlayerWeapons and access everything through PlayerStats: ps.weaponList. ...
public class Weapon : MonoBehaviour
{
    public PlayerStats ps;

    public string weaponName;

    [SerializeField]
    private WeaponCharacteristics weaponCharacteristics;

    // Use this for initialization
    void Start()
    {
        ps.stats.OnWeaponChanged += WeaponDataChanged;
        weaponCharacteristics = (WeaponCharacteristics)ps.stats.GetType().GetProperty(weaponName).GetValue(ps.stats, null);
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void WeaponDataChanged(CustomArgs kek)
    {
        if (kek.weaponData.name == weaponName)
        {
            weaponCharacteristics = kek.currentPistolCharacteristics;
        }
    }
}
