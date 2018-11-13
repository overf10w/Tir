using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;

    public PlayerWeapons playerWeapons;
    private List<WeaponData> weaponList;
    private WeaponData weaponData;

    // Use this for initialization
    void Start()
    {
        weaponList = playerWeapons.weapons;
        weaponData = weaponList.Find(i => i.name == weaponName);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
