using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class Cube : MonoBehaviour, IDestroyable
{
    [SerializeField] private float health = 100.0f;

    // Update is called once per frame
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}