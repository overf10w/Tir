using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Wave : MessageHandler
{
    public PlayerStats playerStats;

    [HideInInspector]
    public int cubesNumber;

    public int index;

    void Awake()
    {
        Init();
    }

    public void Update()
    {

    }

    private void Init()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            cubesNumber++;
        }
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            if (playerStats.playerDb.CurrentWave.index == index)
            {
                cubesNumber -= 1;
                if (cubesNumber <= 0)
                {
                    playerStats.playerDb.UpdateCurrentWave();
                }
            }
        }
    }
}
