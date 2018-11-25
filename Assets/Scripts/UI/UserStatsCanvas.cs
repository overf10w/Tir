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

    // pistol
    public Text pistolTxt;
    public Text pistolNextTxt;

    // double pistol
    public Text doublePistolTxt;
    public Text doublePistolNextTxt;

    public Text teamDPSTxt;

    public float pistolDps;
    public float doublePistolDps;

    // Use this for initialization
    void Awake()
    {
        //yield return new W;
        playerGold = GameObject.Find("GoldTxt").GetComponent<Text>();
        playerCurrentWave = GameObject.Find("CurrentWaveLbl").GetComponent<Text>();
        playerAttack = GameObject.Find("DamageTxt").GetComponent<Text>();

        // pistol
        pistolTxt = GameObject.Find("PistolTxt").GetComponent<Text>();
        pistolNextTxt = GameObject.Find("PistolNextTxt").GetComponent<Text>();

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

    public void HandleWeaponChanged(WeaponArgs weapon)
    {
        WeaponCharacteristics w = weapon.weaponCharacteristics;
        string str = w.Cost.ToString() + "$, " + w.Dps.ToString() + " dps";
        string strNxt = w.NextCost.ToString() + "$, " + w.NextDps.ToString() + " dps";

        Debug.Log("HANDLE WEAPON CHANGED!: str: " + str);
        Debug.Log("HANDLE WEAPON CHANGED!: strNxt: " + strNxt);
        switch (weapon.sender.weaponType)
        {
            case WeaponType.PISTOL:
                pistolDps = w.Dps;
                pistolTxt.text = "Pistol: " + str;
                pistolNextTxt.text = strNxt;
                break;
            case WeaponType.DOUBLE_PISTOL:
                doublePistolDps = w.Dps;
                doublePistolTxt.text = "DP: " + str;
                doublePistolNextTxt.text = strNxt;
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