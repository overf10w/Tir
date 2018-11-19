using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MessageHandler
{
    public PlayerStats playerStats;
    private Wave wave;
    private int cubes;
    private int waveInd;

    // Use this for initialization
    void Start()
    {
        waveInd = playerStats.playerDb._currentWave;
        SpawnWave();
    }

    void SpawnWave()
    {
        var wavePrefab = playerStats.playerDb.playerWaves.waves[waveInd];
        wave = Instantiate(wavePrefab, wavePrefab.transform.position, Quaternion.identity) as Wave;
        cubes = wave.cubesNumber;
        waveInd += 1;
        MessageBus.Instance.SendMessage(new Message() { Type = MessageType.WaveChanged, objectValue = wave });
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            if (--cubes <= 0)
            {
                if (waveInd > playerStats.playerDb.playerWaves.waves.Length - 1)
                {
                    MessageBus.Instance.SendMessage(new Message() { Type = MessageType.GameOver });
                    return;
                }
                
                SpawnWave();
            }
        }
    }
}