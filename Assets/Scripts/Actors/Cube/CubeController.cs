using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CubeController
    {
        private Cube cubeMV;

        public CubeController (Cube cubeMV)
        {
            this.cubeMV = cubeMV;

            cubeMV.OnTakeDamage += HandleCubeTakeDamage;
            cubeMV.OnHpChange += HandleCubeHpChange;
        }

        public CubeController()
        {

        }

        public void HandleCubeTakeDamage(object sender, GenericEventArgs<float> damage)
        {
            cubeMV.Health -= damage.val;
            cubeMV.ShowHealth(cubeMV.Health);
        }

        public void HandleCubeHpChange(object sender, GenericEventArgs<float> hp)
        {
            if (hp.val <= 0)
            {
                cubeMV.Destroy();
            }
        }
    }
}