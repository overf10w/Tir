using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UserStatsCanvas : MessageHandler
    {
        #region MessageHandler
        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.LEVEL_PASSED, MessageType.GAME_STARTED };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.LEVEL_PASSED)
            {
                return;
            }
            if (message.Type == MessageType.GAME_STARTED)
            {
                UpdateElapsedTimeSpan(message.DoubleValue);
            }
        }
        #endregion

        //[SerializeField] private ResearchPanelToggleCanvas _researchPanelToggleCanvas;

        private LevelListUI _levelListUI;

        public Text PlayerGoldTxt { get; private set; }

        public TextMeshProUGUI PlayerLevelTxt { get; private set; }

        private Text _teamDPSTxt;
        private Text _elapsedTimeSpanTxt;

        public void Init(PlayerModel playerModel)
        {
            InitMessageHandler();

            // TODO: drag & drop
            _levelListUI = FindObjectOfType<LevelListUI>();

            // playerStats
            PlayerGoldTxt = GameObject.Find("GoldLbl").GetComponent<Text>();
            PlayerGoldTxt.text = playerModel.PlayerStats.Gold.ToString();

            PlayerLevelTxt = transform.Find("MainPanel/StatsPanel/LevelTxt").GetComponent<TextMeshProUGUI>();

            _teamDPSTxt = GameObject.Find("TeamDPSLbl").GetComponent<Text>();



            // --------------- //
            // TODO: use message bus
            //ResourceLoader.playerData.OnWeaponChanged += HandleWeaponChanged;
            //ResourceLoader.gameData.OnLevelChanged += HandleLevelChanged;

            _elapsedTimeSpanTxt = GameObject.Find("ElapsedTimeSpanLbl").GetComponent<Text>();
            TimeSpan idleTimeSpan = new TimeSpan(playerModel.PlayerStats.IdleTimeSpan);
            _elapsedTimeSpanTxt.text = idleTimeSpan.ToString("hh\\:mm\\:ss");
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
            _elapsedTimeSpanTxt.text = "Delta time: " + timeSpan;
        }

        public void HandleLevelChanged(int level)
        {
            _levelListUI.SetLevel(level);
        }

        //public void HandleWeaponChanged(WeaponArgs weapon)
        //{
        //    WeaponModel w = weapon.weaponModel;
        //    string str = w.Cost.ToString() + "$, " + w.Dps.ToString() + " dps";
        //    string strNxt = w.NextCost.ToString() + "$, " + w.NextDps.ToString() + " dps";

        //    TeamDPSTxt.text = (pistolDps + doublePistolDps).ToString();
        //}
    }
}