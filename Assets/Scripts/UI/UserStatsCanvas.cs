using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStatsCanvas : MessageHandler
{
    public PlayerStats playerStats;

    public Text playerGold;
    public Text playerCurrentWave;
    public Text playerAttack;

    // GUNS
    public Text pistolTxt;
    public Text doublePistolTxt;
    public Text teamDPSTxt;

    private WeaponData pistolWD;

    public float pistolDps;
    public float doublePistolDps;
    // Use this for initialization
    void Start()
    {
        playerGold = GameObject.Find("GoldTxt").GetComponent<Text>();
        playerCurrentWave = GameObject.Find("CurrentWaveLbl").GetComponent<Text>();
        playerAttack = GameObject.Find("DamageTxt").GetComponent<Text>();

        pistolTxt = GameObject.Find("PistolTxt").GetComponent<Text>();
        doublePistolTxt = GameObject.Find("DoublePistolTxt").GetComponent<Text>();

        teamDPSTxt = GameObject.Find("TeamDPSTxt").GetComponent<Text>();
        // --------------- //
        playerStats.playerDb.OnWeaponChanged += HandleWeaponChanged;
    }

    // TODO: this shouldn't be in update
    void Update()
    {
        playerGold.text = playerStats.playerDb.Gold.ToString();
        playerAttack.text = "Dmg: " + playerStats.playerDb.Damage.value + " (" + playerStats.playerDb.Damage.level + "lvl) " + ". Next attack: " + playerStats.playerDb.NextDamage.goldWorth + " gold";
    }

    public void OnUpdateDamage(int kek = 2)
    {
        playerStats.playerDb.UpdateDamage();
    }

    public void OnIsAutoShoot(bool isAutoShoot)
    {
        playerStats.playerDb.UpdateAutoFire();
    }

    public void UpdatePistol()
    {
        playerStats.playerDb.UpdatePistol();
    }

    public void UpdateDoublePistol()
    {
        playerStats.playerDb.UpdateDoublePistol();
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.WaveChanged)
        {
            Wave tmpWave = (Wave) message.objectValue;
            playerCurrentWave.text = "Round: " + tmpWave.index;
        }
    }

    public void HandleWeaponChanged(CustomArgs weapon)
    {
        WeaponCharacteristics w = weapon.weaponCharacteristics;
        string str = w.cost.ToString() + "$, " + w.dps.ToString() + " dps";
        switch (w.weaponType)
        {
            case WeaponType.PISTOL:
                pistolDps = w.dps;
                pistolTxt.text = str;
                break;
            case WeaponType.DOUBLE_PISTOL:
                doublePistolDps = w.dps;
                doublePistolTxt.text = str;
                break;
            default:
                break;
        }
        teamDPSTxt.text = (pistolDps + doublePistolDps).ToString();
    }

    public void UpdateTeamDPSTxt(string text)
    {
        
    }
}