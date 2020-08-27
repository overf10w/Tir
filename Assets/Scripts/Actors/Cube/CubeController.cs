using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class CubeTakeDamageEventArgs : EventArgs
    {
        public float Value { get; private set; }
        public bool ImpactByPlayer { get; private set; }

        public CubeTakeDamageEventArgs(float value, bool impactByPlayer)
        {
            Value = value;
            ImpactByPlayer = impactByPlayer;
        }

        
    }

    public class CubeController
    {
        private readonly Cube _cubeMV;
        private readonly SoundsMachine _soundsMachine;

        private bool isDead = false;

        public CubeController(Cube cubeMV)
        {
            _cubeMV = cubeMV;
            _soundsMachine = cubeMV.SoundsMachine;

            _cubeMV.DamageTaken += TakeDamageHandler;
            _cubeMV.HpChanged += HpChangedHandler;
        }

        private void TakeDamageHandler(object sender, CubeTakeDamageEventArgs e)
        {
            //_cubeMV.Health -= e.Value;
            float prevHp = _cubeMV.Health;
            _cubeMV.SetHealth(prevHp - e.Value, e.ImpactByPlayer);
            _cubeMV.ShowHealth(_cubeMV.Health);
        }

        private void HpChangedHandler(object sender, CubeHpChangeEventArgs e)
        {
            if (e.Value <= 0 && !isDead)
            {
                isDead = true;
                // TODO: wtf remove????
                LeanTween.cancel(_cubeMV.gameObject, false);
                DestroyMV();
            }
        }

        private async void DestroyMV()
        {
            await _soundsMachine.Play("OnDestroy");
            _cubeMV.Destroy();
        }
    }
}