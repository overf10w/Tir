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

            _cubeMV.OnTakeDamage += HandleCubeTakeDamage;
            _cubeMV.OnHpChange += HandleCubeHpChange;
        }

        private void HandleCubeTakeDamage(object sender, GenericEventArgs<float> damage)
        {
            _cubeMV.Health -= damage.Val;
            _cubeMV.ShowHealth(_cubeMV.Health);
        }

        private bool isDead = false;

        private void HandleCubeHpChange(object sender, GenericEventArgs<float> hp)
        {
            if (hp.Val <= 0 && !isDead)
            {
                isDead = true;
                DestroyMV();
            }
        }

        private async void DestroyMV()
        {
            //Debug.Log("DestroyMV: callCnt: " + (++callCnt).ToString());
            //Debug.Log("CubeController.DestroyMV: 1");
            //_cubeMV.
            await _soundsMachine.Play("OnDestroy");
            //await Task.Delay(100);
            //Debug.Log("CubeController.DestroyMV: 2");

            _cubeMV.Destroy();
        }
    }
}