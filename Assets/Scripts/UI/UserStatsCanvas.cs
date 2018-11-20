using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStatsCanvas : MessageHandler
{
    public GameManager gameManager;

    public Text playerGold;
    public Text playerCurrentWave;
    public Text playerAttack;

    // GUNS
    public Text pistolTxt;

    // double pistol
    public Text doublePistolTxt;
    public Text doublePistolNextTxt;

    public Text teamDPSTxt;

    private WeaponData pistolWD;

    public float pistolDps;
    public float doublePistolDps;
    // Use this for initialization
    void Awake()
    {
        playerGold = GameObject.Find("GoldTxt").GetComponent<Text>();
        playerCurrentWave = GameObject.Find("CurrentWaveLbl").GetComponent<Text>();
        playerAttack = GameObject.Find("DamageTxt").GetComponent<Text>();

        pistolTxt = GameObject.Find("PistolTxt").GetComponent<Text>();

        //doublePistol
        doublePistolTxt = GameObject.Find("DoublePistolTxt").GetComponent<Text>();
        doublePistolNextTxt = GameObject.Find("DoublePistolNextTxt").GetComponent<Text>();

        teamDPSTxt = GameObject.Find("TeamDPSTxt").GetComponent<Text>();
        // --------------- //
        gameManager.playerDb.OnWeaponChanged += HandleWeaponChanged;
    }

    // TODO: this shouldn't be in update
    void Update()
    {
        playerGold.text = gameManager.playerDb.Gold.ToString();
        playerAttack.text = "Dmg: " + gameManager.playerDb.Damage.value + " (" + gameManager.playerDb.Damage.level + "lvl) " + ". Next attack: " + gameManager.playerDb.NextDamage.goldWorth + " gold";
    }

    public void OnUpdateDamage(int kek = 2)
    {
        gameManager.playerDb.UpdateDamage();
    }

    public void OnIsAutoShoot(bool isAutoShoot)
    {
        gameManager.playerDb.UpdateAutoFire();
    }

    public void UpdatePistol()
    {
        gameManager.playerDb.UpdatePistol();
    }

    public void UpdateDoublePistol()
    {
        gameManager.playerDb.UpdateDoublePistol();
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
        string strNxt = w.nextCost.ToString() + "$, " + w.nextDps.ToString() + " dps";
        switch (w.weaponType)
        {
            case WeaponType.PISTOL:
                pistolDps = w.dps;
                pistolTxt.text = str;
                break;
            case WeaponType.DOUBLE_PISTOL:
                doublePistolDps = w.dps;
                doublePistolTxt.text = "DP" + str;
                doublePistolNextTxt.text = "DP: " + strNxt;
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