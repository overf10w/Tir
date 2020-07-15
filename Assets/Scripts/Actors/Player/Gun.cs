using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;
        // TODO: 
        // Probably don't need these:
        public WeaponStat DPS { get; private set; }
        public WeaponStat DMG { get; private set; } 

        private float _msBetweenShots = 400.0f;
        private float _nextShotTime;

        private Ray _ray;
        private RaycastHit _hit;

        public void Init(WeaponStatData gunData, WeaponStatsAlgorithm dpsAlgorithm, WeaponStatsAlgorithm dmgAlgorithm)
        {
            DPS = new WeaponStat(gunData.dpsLevel, gunData.upgradeLevel, dpsAlgorithm);
            DMG = new WeaponStat(gunData.dmgLevel, gunData.upgradeLevel, dmgAlgorithm);
        }

        public void UpdateGunRotation()
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            transform.rotation = Quaternion.LookRotation(_ray.direction);
        }

        public void Shoot(float damage)
        {
            if (Time.time > _nextShotTime)
            {
                // Work only with 'Cube' layer
                int layerMask = 1 << LayerMask.NameToLayer("Cube");

                if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, Mathf.Infinity, layerMask))
                {
                    _nextShotTime = Time.time + _msBetweenShots / 1000;
                    IDestroyable target = _hit.transform.GetComponent<IDestroyable>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                    Debug.DrawRay(_ray.origin, _ray.direction * 10000, Color.red, 0.5f);
                    Debug.DrawRay(_muzzle.position, _hit.point - _muzzle.position, Color.green, 0.7f);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_hit.point, 0.3f);
        }
    }
}