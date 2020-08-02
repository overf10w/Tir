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

        private bool isDead = false;

        private void HpChangedHandler(object sender, CubeHpChangeEventArgs e)
        {
            if (e.Value <= 0 && !isDead)
            {
                isDead = true;
                DestroyMV();
            }
        }

        private async void DestroyMV()
        {
            await _soundsMachine.Play("OnDestroy");
            //await Task.Delay(100);
            _cubeMV.Destroy();
        }
    }
}