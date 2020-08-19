using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
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

        private void TakeDamageHandler(object sender, EventArgs<float> damage)
        {
            _cubeMV.Health -= damage.Val;
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