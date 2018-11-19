using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MessageHandler
{
    public PlayerStats ps;
    public string weaponName;
    private Wave currentWave;

    [SerializeField]
    private WeaponCharacteristics weaponCharacteristics;

    // Use this for initialization
    void Start()
    {
        ps.playerDb.OnWeaponChanged += WeaponDataChanged;
        weaponCharacteristics = (WeaponCharacteristics)ps.playerDb.GetType().GetProperty(weaponName).GetValue(ps.playerDb, null);
    }
    
    public void WeaponDataChanged(CustomArgs kek)
    {
        if (kek.weaponData.name == weaponName)
        {
            weaponCharacteristics = kek.weaponCharacteristics;
            Debug.Log("Weapon " + weaponName + " updated: " + kek.weaponCharacteristics.cost);
        }
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.WaveChanged)
        {
            this.currentWave = (Wave)message.objectValue;
        }
    }
}
