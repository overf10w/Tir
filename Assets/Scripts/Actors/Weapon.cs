using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Cost
    {
        public Cost()
        {

        }

        // JUST RENAME THE COST TO WEAPONSTAT AND USE THE MIX OF STRATEGY PATTERN AND Unity Forum tutorial:
        // 1. https://en.wikipedia.org/wiki/Strategy_pattern#C#
        // +
        // 2. https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/


        //////// How this thing should be done:
        //////// 1. Store just the level (int or float)
        //////// 2. Some scriptable object (or static class) stores 
        ////////    an algorithm (with formulae and coefficients) of 
        ////////    how to get the cost out of DPS/DMG level

        public Cost(int curr, int next)
        {
            CurrCost = curr;
            NextCost = next;
        }

        public int CurrCost { get; set; }
        public int NextCost { get; set; }
    }

    public class Weapon : MessageHandler
    {
        public Cost DPS { get; set; }

        public WeaponType weaponType;
        private Wave wave;

        [SerializeField]
        public WeaponModel WeaponModel;

        public float nextShotTime;
        public float msBetweenShots = 200;

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
                    cube.TakeDamage(WeaponModel.Dps);
                }
                else
                {
                    Debug.Log("Weapon: " + weaponType + ": There's no cube there!");
                }
            }
        }
    }
}