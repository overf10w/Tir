using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MessageHandler
{
    public WeaponType weaponType;
    private Wave wave;

    [SerializeField]
    public WeaponModel weaponModel;

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.WaveChanged)
        {
            this.wave = (Wave)message.objectValue;
        }
    }

    // TODO: 
    // 0. Remove call to this method from PlayerController.cs (line 45)
    // 1. Replace this method with WeaponModel.cs: Fire() method
    // 2. This be called through Command pattern in PlayerView.cs
    //      2.1. All the Fire() commands in PlayerView.cs should be queued
    public void Fire()
    {
        if (weaponModel.level > 0)
        {
            weaponModel.Fire(wave);
        }
    }
}
