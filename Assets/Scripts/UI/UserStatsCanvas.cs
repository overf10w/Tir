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
        // TODO: subscribe to playerStats.OnCurrentWaveChanged() += ...
    }

    // TODO: this shouldn't be in update
    void Update()
    {
        playerGold.text = playerStats.playerDb.Gold.ToString();
        playerCurrentWave.text = "Round: " + playerStats.playerDb.CurrentWave.index.ToString();
        playerAttack.text = "Dmg: " + playerStats.playerDb.Damage.value + " (" + playerStats.playerDb.Damage.level + "lvl) " + ". Next attack: " + playerStats.playerDb.NextDamage.goldWorth + " gold";
    }

    public void OnUpdateDamage(int kek = 2)
    {
        playerStats.playerDb.UpdateDamage();
    }

    public void OnIsAutoShoot(bool isAutoShoot)
    {
        //playerStats.stats.IsAutoShoot = isAutoShoot;
        playerStats.playerDb.UpdateAutoFire();
    }

    public void UpdatePistol()
    {
        //Debug.Log(playerStats.stats.Pistol.value);
        playerStats.playerDb.UpdatePistol();
        //Debug.Log(playerStats.stats.Pistol.value);
        //Debug.Log("BEFORE: Pistol lvl: " + pistolWD.lvls[pistolWD.lvlInd].level);   // TODO: 
        //pistolWD.lvlInd++;
        //Debug.Log("AFTER: Pistol lvl: " + pistolWD.lvls[pistolWD.lvlInd].level);
    }

    public void UpdateDoublePistol()
    {
        playerStats.playerDb.UpdateDoublePistol();
    }
}