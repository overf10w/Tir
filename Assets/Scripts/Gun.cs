using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
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

    // Update is called once per frame
    void Update()
    {
        UpdateGunRotation();
        if (Input.GetMouseButton(0) && Time.time > nextShotTime)
        {
            Shoot();
        }
    }

    public void UpdateGunRotation()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(ray.direction);
    }

    public void Shoot()
    {
        // Work only with 'Cube' layer
        int layerMask = 1 << LayerMask.NameToLayer("Cube");

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Debug.DrawRay(ray.origin, ray.direction * 10000, Color.red, 0.5f);
            Debug.DrawRay(muzzle.position, hit.point - muzzle.position, Color.green, 0.7f);
            Debug.Log("hit.point: " + hit.point);
        }

        //var projectile = Instantiate(Resources.Load<GameObject>("Prefabs/Kek"), muzzle.position, muzzle.rotation);
        //projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * 300);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(hit.point, 0.3f);
    }
}