using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MessageHandler
{
    public delegate void OnWaveChanged(int wave);

    public event OnWaveChanged onWaveChanged;

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
                    MessageBus.Instance.SendMessage(new Message() {Type = MessageType.GameOver});
                    return;
                }
                if (onWaveChanged != null)
                    onWaveChanged(currentWaveIndex);

                SpawnWave();
            }
        }
    }
}