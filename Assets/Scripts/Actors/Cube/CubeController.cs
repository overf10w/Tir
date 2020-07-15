using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CubeController
    {
        private Cube _cubeMV;

        public CubeController (Cube cubeMV)
        {
            _cubeMV = cubeMV;

            _cubeMV.OnTakeDamage += HandleCubeTakeDamage;
            _cubeMV.OnHpChange += HandleCubeHpChange;
        }

        public CubeController()
        {

        }

        public void HandleCubeTakeDamage(object sender, GenericEventArgs<float> damage)
        {
            _cubeMV.Health -= damage.Val;
            _cubeMV.ShowHealth(_cubeMV.Health);
        }

        public void HandleCubeHpChange(object sender, GenericEventArgs<float> hp)
        {
            if (hp.Val <= 0)
            {
                _cubeMV.Destroy();
            }
        }
    }
}