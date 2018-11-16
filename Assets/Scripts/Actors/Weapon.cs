using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: forget about PlayerWeapons and access everything through PlayerStats: ps.weaponList. ...
public class Weapon : MonoBehaviour
{
    public PlayerStats ps;

    //private int currentWave;

    public string weaponName;

    public Wave currentWave;

    [SerializeField]
    private WeaponCharacteristics weaponCharacteristics;

    // Use this for initialization
    void Start()
    {
        ps.playerDb.OnWeaponChanged += WeaponDataChanged;
        ps.playerDb.OnCurrentWaveChanged += CurrentWaveChanged;
        weaponCharacteristics = (WeaponCharacteristics)ps.playerDb.GetType().GetProperty(weaponName).GetValue(ps.playerDb, null);
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

    public void CurrentWaveChanged(Wave value)
    {
        this.currentWave = value;
        Debug.Log("Weapon: " + weaponName + ": Current Wave Cubes Number: " + this.currentWave.cubesNumber);
    }
}
