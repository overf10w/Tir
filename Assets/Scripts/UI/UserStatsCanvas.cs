using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStatsCanvas : MonoBehaviour
{
    public PlayerStats playerStats;

    public Text playerGold;
    public Text playerCurrentWave;
    public Text playerAttack;

    private WeaponData pistolWD;
    // Use this for initialization
    void Start()
    {
        playerGold = GameObject.Find("GoldTxt").GetComponent<Text>();
        playerCurrentWave = GameObject.Find("CurrentWaveLbl").GetComponent<Text>();
        playerAttack = GameObject.Find("DamageTxt").GetComponent<Text>();
        // --------------- //
    }

    // TODO: this shouldn't be in update
    void Update()
    {
        playerGold.text = playerStats.stats.Gold.ToString();
        playerCurrentWave.text = "Round: " + playerStats.stats.CurrentWave.ToString();
        playerAttack.text = "Dmg: " + playerStats.stats.Damage.value + " (" + playerStats.stats.Damage.level + "lvl) " + ". Next attack: " + playerStats.stats.NextDamage.goldWorth + " gold";
    }

    public void OnUpdateDamage(int kek = 2)
    {
        playerStats.stats.UpdateDamage();
    }

    public void OnIsAutoShoot(bool isAutoShoot)
    {
        //playerStats.stats.IsAutoShoot = isAutoShoot;
        playerStats.stats.UpdateAutoFire();
    }

    public void UpdatePistol()
    {
        Debug.Log(playerStats.stats.Pistol.value);
        playerStats.stats.UpdatePistol();
        Debug.Log(playerStats.stats.Pistol.value);
        //Debug.Log("BEFORE: Pistol lvl: " + pistolWD.lvls[pistolWD.lvlInd].level);   // TODO: 
        //pistolWD.lvlInd++;
        //Debug.Log("AFTER: Pistol lvl: " + pistolWD.lvls[pistolWD.lvlInd].level);
    }
}