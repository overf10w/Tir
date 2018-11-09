using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public float speed = 5f;

    public float msBetweenShots = 200.0f;

    private Vector3 target;
    private Vector3 mousePos;

    private float nextShotTime;

    // Rays
    private Ray ray;
    private RaycastHit hit;

    public void UpdateGunRotation()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(ray.direction);
    }

    public void Shoot(float playerAttack)
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
                    target.TakeDamage(playerAttack);
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