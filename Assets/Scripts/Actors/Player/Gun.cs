using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Gun : MonoBehaviour
    {
        // TODO: 
        // Probably don't need these:
        public WeaponStat DPS;
        public WeaponStat DMG;

        public Transform muzzle;

        private float msBetweenShots = 400.0f;

        private float nextShotTime;

        // Rays
        private Ray ray;
        private RaycastHit hit;

        public void Init(WeaponStatData gunData, WeaponStatsAlgorithm dpsAlgorithm, WeaponStatsAlgorithm dmgAlgorithm)
        {
            DPS = new WeaponStat(gunData.dpsLevel, dpsAlgorithm);
            DMG = new WeaponStat(gunData.dmgLevel, dmgAlgorithm);

            Debug.Log("Gun.cs: Init: DPS.Value: " + DPS.Value + ", DMG.Value: " + DMG.Value);
        }

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