using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 'Mediator' between PlayerStats and CubeSpawner
// PlayerStats and CubeSpawner don't know about each other
public class GameManager : MonoBehaviour
{
    public PlayerStats playerStats;
    private CubeSpawner cubeSpawner;

    public void Start()
    {
        cubeSpawner = FindObjectOfType<CubeSpawner>();
        cubeSpawner.onWaveChanged += OnNewWave;
    }

    public void OnNewWave(int wave)
    {
        playerStats.stats.CurrentWave = wave;
    }

    public void OnDisable()
    {
        cubeSpawner.onWaveChanged -= OnNewWave;
    }
}

