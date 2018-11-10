using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStatsCanvas : MonoBehaviour
{
    public PlayerStats playerStats;

    public Text playerGold;
    public Text playerCurrentWave;
    public Text playerDamage;

    // Use this for initialization
    void Start()
    {
        playerGold = GameObject.Find("GoldTxt").GetComponent<Text>();
        playerCurrentWave = GameObject.Find("CurrentWaveLbl").GetComponent<Text>();
        playerDamage = GameObject.Find("DamageTxt").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        playerGold.text = playerStats.stats.Gold.ToString();
        playerCurrentWave.text = "Round: " + playerStats.stats.CurrentWave.ToString();
        playerDamage.text = "Dmg: " + playerStats.stats.Attack.ToString();
    }

    public void OnUpdateDamage(int kek = 2)
    {
        //playerStats.stats.Attack;
        playerStats.stats.UpdateAttack();
    }

    public void OnIsAutoShoot(bool isAutoShoot)
    {
        //playerStats.stats.IsAutoShoot = isAutoShoot;
        playerStats.stats.UpdateAutoShoot();
    }
}