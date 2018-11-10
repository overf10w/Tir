using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cube : MonoBehaviour, IDestroyable
{
    public CubeStats cubeStats;

    [SerializeField] private int gold = 2;
    [SerializeField] private float health = 100.0f;

    public void Awake()
    {
        cubeStats = AssetDatabase.LoadAssetAtPath<CubeStats>("Assets/CubeStats.Asset");
        //Debug.Log("cubeStats == null: " + cubeStats == null);
        gold = cubeStats.stats.gold;
        health = cubeStats.stats.HP;
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