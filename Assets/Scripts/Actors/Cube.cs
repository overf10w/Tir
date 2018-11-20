using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IDestroyable
{
    public CubeStats cubeStats;

    [SerializeField] private int gold = 2;
    [SerializeField] private float health = 100.0f;

    public void Awake()
    {
        cubeStats = Resources.Load<CubeStats>("SO/CubeStats");
        gold = cubeStats.stats.gold;
        health = cubeStats.stats.HP;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0.0f)
        {
            MessageBus.Instance.SendMessage(new Message { Type = MessageType.CubeDeath, objectValue = (Cube)this});
            Destroy(this.gameObject);
        }
    }
    public int Gold
    {
        get { return gold; }
    }
}