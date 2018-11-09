using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IDestroyable
{
    private int gold = 2;

    [SerializeField] private float health = 100.0f;

    public void Start()
    {
        health = 100.0f;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0.0f)
        {
            MessageBus.Instance.SendMessage(new Message {Type = MessageType.CubeDeath, IntValue = gold });
            Destroy(this.gameObject);
        }
    }
}