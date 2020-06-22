using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // TODO: Gun to be init
    public class Gun : MonoBehaviour
    {
        // TODO: 
        // This Gun be initialized with WeaponStats (add PlayerDefaultPistol to WeaponDataFiles, rename StandardPistol to smth else)
        // private WeaponStat DPS; <-- where DPS means 1/interval, so the interval between shots = 1/DPS
        // private WeaponStat DMG;

        public Transform muzzle;

        private float msBetweenShots = 400.0f;

        private float nextShotTime;

        // Rays
        private Ray ray;
        private RaycastHit hit;

        public void UpdateGunRotation()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            transform.rotation = Quaternion.LookRotation(ray.direction);
        }

        public void Shoot(float damage)
        {
            if (Time.time > nextShotTime)
            {
                // Work only with 'Cube' layer
                int layerMask = 1 << LayerMask.NameToLayer("Cube");

                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
                {
                    nextShotTime = Time.time + msBetweenShots / 1000;
                    IDestroyable target = hit.transform.GetComponent<IDestroyable>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                    Debug.DrawRay(ray.origin, ray.direction * 10000, Color.red, 0.5f);
                    Debug.DrawRay(muzzle.position, hit.point - muzzle.position, Color.green, 0.7f);
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawSphere(hit.point, 0.3f);
        }
    }
}