using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CubeController
    {
        private readonly Cube _cubeMV;

        public CubeController(Cube cubeMV)
        {
            _cubeMV = cubeMV;

            _cubeMV.OnTakeDamage += HandleCubeTakeDamage;
            _cubeMV.OnHpChange += HandleCubeHpChange;
        }

        private void HandleCubeTakeDamage(object sender, GenericEventArgs<float> damage)
        {
            _cubeMV.Health -= damage.Val;
            _cubeMV.ShowHealth(_cubeMV.Health);
        }

        private void HandleCubeHpChange(object sender, GenericEventArgs<float> hp)
        {
            if (hp.Val <= 0)
            {
                _cubeMV.Destroy();
            }
        }
    }
}