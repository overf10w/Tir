using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class WeaponStatData
    {
        public string weaponName;

        public int dpsLevel;

        public int dmgLevel;
    }

    // TODO: WeaponStat => WeaponStats - [done]
    // WeaponStats to have these props:
    // Value
    // Level
    // Price
    // (each of the props can be of type CharacterStat (https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/)) - but that's not obligatory: 
    public class WeaponStat
    {
        public WeaponStat()
        {

        }

        public WeaponStat(int level, int price, int value)
        {
            Level = level;
            Price = price;
            Value = value;
        }

        public WeaponStat(int level)
        {
            Level = level;
        }

        public int Level { get; set; }
        public float Price { get; set; }
        public int Value { get; set; }
    }

    public class Weapon : MessageHandler
    {
        // TODO: (?) private set
        public WeaponStat DPS { get; set; }

        // TODO: (?) private set
        public WeaponStat DMG { get; set; }

        private Wave wave;

        public float nextShotTime;
        public float msBetweenShots = 200;

        private WeaponStatsAlgorithmsHolder algorithm;

        public void Init(WeaponStatsAlgorithmsHolder algorithm, WeaponStatData data)
        {
            this.algorithm = algorithm;

            DPS = new WeaponStat(data.dpsLevel);
            // TODO: algorithm.GetDPSPrice(level)
            DPS.Price = algorithm.DPS.GetPrice(data.dpsLevel);

            DMG = new WeaponStat(data.dmgLevel);
            // TODO: algorithm.GetDMGPrice(data.dmgLevel)
            DMG.Price = algorithm.DMG.GetPrice(data.dmgLevel);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.WaveChanged)
            {
                this.wave = (Wave)message.objectValue;
            }
        }

        // TODO: 
        // 2. This be called through Command pattern in PlayerView.cs
        //      2.1. All the Fire() commands in PlayerView.cs should be queued
        public void Fire(Wave wave)
        {
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                IDestroyable cube = wave.Cubes.ElementAtOrDefault(new System.Random().Next(wave.cubesNumber));
                if ((MonoBehaviour)cube != null)
                {
                    cube.TakeDamage(DPS.Price);
                }
                else
                {
                    Debug.Log("Weapon: " + ": There's no cube there!");
                }
            }
        }
    }
}