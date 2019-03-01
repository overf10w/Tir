using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Polymorphic object, it can only contain current wave
public class Weapon : MessageHandler
{
    public WeaponType weaponType;
    private Wave wave;

    [SerializeField]
    public WeaponModel weaponModel;

    // Use this for initialization
    void Start()
    {
        //if (weaponType == WeaponType.PISTOL)
        //{
        //    weaponModel = new WeaponModel(12, 2, 0);
        //}
        //if (weaponType == WeaponType.DOUBLE_PISTOL)
        //{
        //    weaponModel = new WeaponModel(20, 4, 0);
        //}
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.WaveChanged)
        {
            this.wave = (Wave)message.objectValue;
        }
    }

    public void Fire()
    {
        if (weaponModel.level > 0)
        {
            weaponModel.Fire(wave);
        }
    }
}
