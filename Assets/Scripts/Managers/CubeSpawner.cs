using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubeSpawner : MessageHandler
{
    public PlayerStats playerStats;
    public delegate void OnWaveChanged(int wave);

    public event OnWaveChanged onWaveChanged;

    public Wave[] waves;
    private Wave currentWave;

    private int currentWaveIndex;
    private int cubesRemainingAlive;

    // Use this for initialization
    void Start()
    {
        playerStats = AssetDatabase.LoadAssetAtPath<PlayerStats>("Assets/PlayerStats.Asset");
        if (playerStats != null)
        {
            currentWaveIndex = playerStats.stats.CurrentWave;
        }
        else
        {
            currentWaveIndex = 0;
            Debug.LogError("PlayerStats hasn't been loaded");
        }
        SpawnWave();
    }

    void SpawnWave()
    {
        if (currentWaveIndex <= waves.Length - 1)
        {
            currentWave = waves[currentWaveIndex];
            var go = Instantiate(currentWave, currentWave.transform.position, Quaternion.identity) as Wave;
            cubesRemainingAlive = go.cubesNumber;
        }
        else
        {
            Debug.LogWarning("No more waves to spawn");
        }
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