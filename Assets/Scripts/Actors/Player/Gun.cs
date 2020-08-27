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
                int layerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Cube")) | (1 << LayerMask.NameToLayer("SpaceDecoration")) | (1 << LayerMask.NameToLayer("UI"));

                if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, Mathf.Infinity, layerMask))
                {
                    var dir = _hit.point - _muzzle.position;
                    GameObject obj = Instantiate(_projectilePrefab, _muzzle.position, Quaternion.LookRotation(dir, Vector3.up));

                    _nextShotTime = Time.time + _msBetweenShots / 1000;

                    ICube target = _hit.transform.GetComponent<ICube>();
                    if (target != null)
                    {
                        target.TakeDamage(damage, true);
                    }
                    Cube cube = _hit.transform.GetComponent<Cube>();
                    if (cube != null)
                    {
                        cube.ParticleMachine.Spawn("TakeDamageParticles", _hit.point);
                    }

                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_hit.point, 0.3f);
        }
    }
}