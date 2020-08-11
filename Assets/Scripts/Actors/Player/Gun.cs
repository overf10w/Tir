using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // TODO: rename to ClickGun
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private GameObject _projectilePrefab;

        private float _msBetweenShots = 15.0f;
        private float _nextShotTime;

        private Ray _ray;
        private RaycastHit _hit;

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
                    //Debug.DrawRay(_ray.origin, _ray.direction * 10000, Color.red, 0.5f);
                    //Debug.DrawRay(_muzzle.position, _hit.point - _muzzle.position, Color.green, 0.7f);

                    var dir = _hit.point - _muzzle.position;

                    GameObject obj = Instantiate(_projectilePrefab, _muzzle.position, Quaternion.LookRotation(dir, Vector3.up));

                    //obj.getcom

                }
                //bool kek = false;

                //var dir = _ray.origin - _ray.direction;

                //Instantiate(_projectilePrefab, _muzzle.position, Quaternion.LookRotation(_ray.direction, Vector3.up)); // good code (almost)
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_hit.point, 0.3f);
        }
    }
}