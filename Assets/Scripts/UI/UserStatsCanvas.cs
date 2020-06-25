using System;
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
        public Text TeamDPSTxt;
        public Text ElapsedTimeSpanTxt;

        public void Init(PlayerModel playerModel)
        {
            InitMessageHandler();
            // TODO: drag & drop
            LevelListUI = FindObjectOfType<LevelListUI>();

            // playerStats
            PlayerGoldTxt = GameObject.Find("GoldLbl").GetComponent<Text>();
            PlayerGoldTxt.text = playerModel.Gold.ToString();

            PlayerCurrentWaveTxt = GameObject.Find("CurrentWaveLbl").GetComponent<Text>();

            TeamDPSTxt = GameObject.Find("TeamDPSLbl").GetComponent<Text>();

            // --------------- //
            // TODO: use message bus
            //ResourceLoader.playerData.OnWeaponChanged += HandleWeaponChanged;
            //ResourceLoader.gameData.OnLevelChanged += HandleLevelChanged;

            ElapsedTimeSpanTxt = GameObject.Find("ElapsedTimeSpanLbl").GetComponent<Text>();
            TimeSpan idleTimeSpan = new TimeSpan(playerModel.IdleTimeSpan);
            ElapsedTimeSpanTxt.text = idleTimeSpan.ToString("hh\\:mm\\:ss");
        }

        // TODO: this shouldn't be in update
        private void Update()
        {
            //playerGoldLbl.text = ResourceLoader.playerData.Gold.ToString();
            //playerAttackLbl.text = "Dmg: " + ResourceLoader.playerData.Damage.value + " (" + ResourceLoader.playerData.Damage.level + "lvl) " + ". Next attack: " + ResourceLoader.playerData.NextDamage.goldWorth + " gold";
        }

        public void OnUpdateDamage(int kek = 2)
        {
            //ResourceLoader.playerData.UpdateDamage();
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

        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.LevelChanged, MessageType.GameStarted };
            MessageBus.Instance.AddSubscriber(msc);
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

        //public void HandleWeaponChanged(WeaponArgs weapon)
        //{
        //    WeaponModel w = weapon.weaponModel;
        //    string str = w.Cost.ToString() + "$, " + w.Dps.ToString() + " dps";
        //    string strNxt = w.NextCost.ToString() + "$, " + w.NextDps.ToString() + " dps";

        //    TeamDPSTxt.text = (pistolDps + doublePistolDps).ToString();
        //}

        public void HandleLevelChanged(int level)
        {
            LevelListUI.SetLevel(level);
        }

        public void UpdateTeamDPSTxt(string text)
        {

        }
    }
}