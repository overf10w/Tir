using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MessageHandler
{
    public PlayerStats playerStats;
    private Gun gunContoller;

    [HideInInspector]
    public int gold = 0;

    void Start()
    {
        gold = 0;
        gunContoller = GetComponentInChildren<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        gunContoller.UpdateGunRotation();
        if (Input.GetMouseButton(0))
        {
            gunContoller.Shoot();
        }
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            gold += message.IntValue;
            //PlayerData.gold = gold;
            //PlayerData.playerData.GetType().GetProperty("gold").SetValue(PlayerData.playerData, gold, null);
            playerStats.stats.gold += message.IntValue;
        }
    }


}