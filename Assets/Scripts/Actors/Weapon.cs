using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    float timer = 0;
    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.4f)
        {
            IDestroyable cube = currentWave.Cubes.ElementAtOrDefault(new System.Random().Next(currentWave.Cubes.Count));
            if ((MonoBehaviour)cube != null)
            {
                cube.TakeDamage(2.0f);
            }
            timer = 0.0f;
        }
    }
}
