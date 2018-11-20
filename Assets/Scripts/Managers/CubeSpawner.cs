using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MessageHandler
{
    public GameManager gameManager;
    private Wave wave;
    private int cubes;
    private int waveInd;

    // Use this for initialization
    void Start()
    {
        waveInd = gameManager.playerDb._currentWave;
        SpawnWave();
    }

    public void Update()
    {
        //Debug.Log(wave.Cubes.Count);
        if (wave.Cubes.Count <= 0)
        {
            if (waveInd > gameManager.playerDb.playerWaves.waves.Length - 1)
            {
                MessageBus.Instance.SendMessage(new Message() { Type = MessageType.GameOver });
                return;
            }
            Debug.Log("CubeSpawner: " + cubes);
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        var wavePrefab = gameManager.playerDb.playerWaves.waves[waveInd];
        wave = Instantiate(wavePrefab, wavePrefab.transform.position, Quaternion.identity) as Wave;
        cubes = wave.cubesNumber;
        waveInd += 1;
        MessageBus.Instance.SendMessage(new Message() { Type = MessageType.WaveChanged, objectValue = wave });
    }

    public override void HandleMessage(Message message)
    {
        //if (message.Type == MessageType.CubeDeath)
        //{
        //    if (--cubes <= 0)
        //    {
        //        if (waveInd > playerStats.playerDb.playerWaves.waves.Length - 1)
        //        {
        //            MessageBus.Instance.SendMessage(new Message() { Type = MessageType.GameOver });
        //            return;
        //        }
        //        Debug.Log("CubeSpawner: " + cubes);
        //        SpawnWave();
        //    }
        //}
    }

    public void OnDisable()
    {
        gameManager.playerDb._currentWave = waveInd;
    }
}