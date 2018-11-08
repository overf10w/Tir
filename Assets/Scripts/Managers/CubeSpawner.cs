using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MessageHandler
{
    public Wave[] waves;
    private Wave currentWave;

    private int currentWaveIndex;
    private int cubesRemainingAlive;

    // Use this for initialization
    void Start()
    {
        currentWaveIndex = 0;
        SpawnWave();
    }

    void SpawnWave()
    {
        currentWave = waves[currentWaveIndex];
        var go = Instantiate(currentWave, currentWave.transform.position, Quaternion.identity) as Wave;
        cubesRemainingAlive = go.cubesNumber;
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            if (--cubesRemainingAlive <= 0)
            {
                if (++currentWaveIndex > waves.Length - 1)
                {
                    Message deathMessage = new Message();
                    deathMessage.Type = MessageType.GameOver;
                    MessageBus.Instance.SendMessage(deathMessage);
                    return;
                }
                PlayerData.currentWave = currentWaveIndex;
                SpawnWave();
            }
        }
    }
}