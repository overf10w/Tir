using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStatsCanvas : MonoBehaviour
{
    public Text playerGold;
    public Text playerCurrentWave;

    // Use this for initialization
    void Start()
    {
        playerGold = GameObject.Find("GoldTxt").GetComponent<Text>();
        playerCurrentWave = GameObject.Find("CurrentWaveLbl").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        playerGold.text = PlayerData.gold.ToString();

        playerCurrentWave.text = "Round " + PlayerData.currentWave.ToString();
    }
}