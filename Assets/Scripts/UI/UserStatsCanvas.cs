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

        [SerializeField] private ResearchPanelToggleCanvas _researchPanelToggleCanvas;
        public ResearchPanelToggleCanvas ResearchPanelToggleCanvas => _researchPanelToggleCanvas;

        [SerializeField] private TextMeshProUGUI _playerGoldText;
        public TextMeshProUGUI PlayerGoldTxt => _playerGoldText;

        private LevelListUI _levelListUI;

        public TextMeshProUGUI PlayerLevelTxt { get; private set; }

        private Text _teamDPSTxt;
        private Text _elapsedTimeSpanTxt;

        public void Init(PlayerModel playerModel)
        {
            InitMessageHandler();

            // TODO: drag & drop
            _levelListUI = FindObjectOfType<LevelListUI>();

            PlayerGoldTxt.text = playerModel.PlayerStats.Gold.SciFormat();

            PlayerLevelTxt = transform.Find("MainPanel/StatsPanel/LevelTxt").GetComponent<TextMeshProUGUI>();
            PlayerLevelTxt.text = playerModel.PlayerStats.Level.ToString();

            _teamDPSTxt = GameObject.Find("TeamDPSLbl").GetComponent<Text>();

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

        public void LevelChangedHandler(int level)
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