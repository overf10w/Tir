using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MessageHandler
{
    public PlayerStats playerStats;
    public delegate void OnWaveChanged(int wave);

    public event OnWaveChanged onWaveChanged;

    //public PlayerWaves waves;
    private Wave currentWave;

    // TODO: remove currentWaveIndex
    private int cubesRemainingAlive;

    // Use this for initialization
    void Start()
    {
        //playerStats = Resources.Load<PlayerStats>("SO/PlayerStats");

        playerStats.playerDb.OnCurrentWaveChanged += HandleWaveChanged;

        //var go = Instantiate(playerStats.stats.playerWaves.waves[0], playerStats.stats.playerWaves.waves[0].transform.position, Quaternion.identity) as Wave;

        if (playerStats != null)
        {
            
        }
        else
        {
            Debug.LogError("PlayerStats hasn't been loaded");
        }
        SpawnWave();
    }

    void SpawnWave()
    {
        currentWave = playerStats.playerDb.CurrentWave;
        //currentWave = waves.waves[currentWaveIndex];
        var go = Instantiate(currentWave, currentWave.transform.position, Quaternion.identity) as Wave;
        cubesRemainingAlive = go.cubesNumber;   
    }

    public void HandleWaveChanged(Wave wave)
    {
        SpawnWave();
    }

    public override void HandleMessage(Message message)
    {
        //if (message.Type == MessageType.CubeDeath)
        //{
        //    if (--cubesRemainingAlive <= 0)
        //    {
        //        if (++currentWaveIndex > waves.waves.Length - 1)
        //        {
        //            MessageBus.Instance.SendMessage(new Message() {Type = MessageType.GameOver});
        //            return;
        //        }
        //        if (onWaveChanged != null)
        //            onWaveChanged(currentWaveIndex);

        //        SpawnWave();
        //    }
        //}
    }
}