using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UserStatsCanvas : MessageHandler
    {
        public PlayerStats playerStats;

        public LevelListUI LevelListUI;
        public ResourceLoader ResourceLoader;

        public Text PlayerGoldTxt;
        public Text PlayerCurrentWaveTxt;
        public Text PlayerAttackTxt;
        public Text TeamDPSTxt;
        public Text ElapsedTimeSpanTxt;

        // pistol
        public float pistolDps;
        public Text pistolLbl;
        public Text pistolNextLbl;

        // double pistol
        public float doublePistolDps;
        public Text doublePistolLbl;
        public Text doublePistolNextLbl;

        // Use this for initialization
        void Awake()
        {
            // TODO: drag & drop
            LevelListUI = FindObjectOfType<LevelListUI>();

            // playerStats
            PlayerGoldTxt = GameObject.Find("GoldLbl").GetComponent<Text>();
            PlayerCurrentWaveTxt = GameObject.Find("CurrentWaveLbl").GetComponent<Text>();
            PlayerAttackTxt = GameObject.Find("DamageLbl").GetComponent<Text>();

            // pistol
            pistolLbl = GameObject.Find("PistolLbl").GetComponent<Text>();
            pistolNextLbl = GameObject.Find("PistolNextLbl").GetComponent<Text>();

            //doublePistol
            doublePistolLbl = GameObject.Find("DoublePistolLbl").GetComponent<Text>();
            doublePistolNextLbl = GameObject.Find("DoublePistolNextLbl").GetComponent<Text>();

            TeamDPSTxt = GameObject.Find("TeamDPSLbl").GetComponent<Text>();

            // --------------- //
            // TODO: use message bus
            //ResourceLoader.playerData.OnWeaponChanged += HandleWeaponChanged;
            //ResourceLoader.gameData.OnLevelChanged += HandleLevelChanged;

            ElapsedTimeSpanTxt = GameObject.Find("ElapsedTimeSpanLbl").GetComponent<Text>();
        }

        // TODO: this shouldn't be in update
        void Update()
        {
            //playerGoldLbl.text = ResourceLoader.playerData.Gold.ToString();
            //playerAttackLbl.text = "Dmg: " + ResourceLoader.playerData.Damage.value + " (" + ResourceLoader.playerData.Damage.level + "lvl) " + ". Next attack: " + ResourceLoader.playerData.NextDamage.goldWorth + " gold";
        }

        public void OnUpdateDamage(int kek = 2)
        {
            //ResourceLoader.playerData.UpdateDamage();
        }

        public void OnIsAutoShoot(bool isAutoShoot)
        {
            //ResourceLoader.playerData.UpdateAutoFire();
        }

        public void UpdatePistol()
        {
            //ResourceLoader.playerData.UpdatePistol();
        }

        public void UpdateDoublePistol()
        {
            //ResourceLoader.playerData.UpdateDoublePistol();
        }

        public void UpdateElapsedTimeSpan(double timeSpan)
        {
            ElapsedTimeSpanTxt.text = "Delta time: " + timeSpan;
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.LevelChanged)
            {
                UpdateCurrentLevelLabel(message.IntValue);
            }
            if (message.Type == MessageType.GameStarted)
            {
                UpdateElapsedTimeSpan(message.DoubleValue);
            }
        }

        public void UpdateCurrentLevelLabel(int level)
        {
            PlayerCurrentWaveTxt.text = "Round: " + level;
        }

        public void HandleWeaponChanged(WeaponArgs weapon)
        {
            WeaponModel w = weapon.weaponModel;
            string str = w.Cost.ToString() + "$, " + w.Dps.ToString() + " dps";
            string strNxt = w.NextCost.ToString() + "$, " + w.NextDps.ToString() + " dps";

            switch (weapon.sender.weaponType)
            {
                case WeaponType.PISTOL:
                    pistolDps = w.Dps;
                    pistolLbl.text = "Pistol: " + str;
                    pistolNextLbl.text = strNxt;
                    break;
                case WeaponType.DOUBLE_PISTOL:
                    doublePistolDps = w.Dps;
                    doublePistolLbl.text = "DP: " + str;
                    doublePistolNextLbl.text = strNxt;
                    break;
            }
            TeamDPSTxt.text = (pistolDps + doublePistolDps).ToString();
        }

        public void HandleLevelChanged(int level)
        {
            LevelListUI.SetLevel(level);
        }

        public void UpdateTeamDPSTxt(string text)
        {

        }
    }
}