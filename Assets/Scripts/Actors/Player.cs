using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MessageHandler
{
    public PlayerStats playerStats;
    private Gun gunContoller;

    private bool isAutoShoot;

    [HideInInspector]
    public int gold = 0;

    void Start()
    {
        isAutoShoot = false;
        playerStats.stats.OnAutoFireUpdated += OnAutoShoot;
        gold = 0;
        gunContoller = GetComponentInChildren<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        gunContoller.UpdateGunRotation();
        if (Input.GetMouseButton(0))
        {
            gunContoller.Shoot(playerStats.stats.Attack.value);
        }
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            gold += message.IntValue;
            playerStats.stats.Gold += message.IntValue;
        }
    }

    public void OnAutoShoot(float _autoShootDuration)
    {
        //this.isAutoShoot = isAuthoShoot;
        StartCoroutine(AutoShoot(_autoShootDuration));
    }

    public IEnumerator AutoShoot(float _autoShootDuration)
    {
        Debug.Log("Player: Autoshoot: START");
        float timer = 0.0f;
        float timeBetweenShots = 0.2f;
        while (timer <= _autoShootDuration)
        {
            timer += Time.deltaTime;
            timeBetweenShots++;
            if (timeBetweenShots >= 0.2f)
            {
                gunContoller.Shoot(playerStats.stats.Attack.value);
                timeBetweenShots = 0.0f;
            }
            yield return null;
        }
        //yield return new WaitForSeconds(3.0f);
        Debug.Log("Player: Autoshoot: START");
    }

    public void OnDisable()
    {
        playerStats.stats.OnAutoFireUpdated -= OnAutoShoot;
    }
}