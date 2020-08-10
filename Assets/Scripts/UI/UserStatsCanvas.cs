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

        [SerializeField] private PlayerGoldCanvas _playerGoldCanvas;

        public TextMeshProUGUI PlayerLevelTxt { get; private set; }

        private LevelListUI _levelListUI;

        private Text _elapsedTimeSpanTxt;

        public void Init(PlayerModel playerModel)
        {
            InitMessageHandler();

            _levelListUI = FindObjectOfType<LevelListUI>();
            _playerGoldCanvas.Render(playerModel.PlayerStats.Gold);

            PlayerLevelTxt = transform.Find("MainPanel/StatsPanel/LevelTxt").GetComponent<TextMeshProUGUI>();
            PlayerLevelTxt.text = playerModel.PlayerStats.Level.ToString();

            _elapsedTimeSpanTxt = GameObject.Find("ElapsedTimeSpanLbl").GetComponent<Text>();
            TimeSpan idleTimeSpan = new TimeSpan(playerModel.PlayerStats.IdleTimeSpan);
            _elapsedTimeSpanTxt.text = idleTimeSpan.ToString("hh\\:mm\\:ss");
        }

        public void UpdatePlayerGold(float gold)
        {
            _playerGoldCanvas.Render(gold);
        }

        public void UpdateElapsedTimeSpan(double timeSpan)
        {
            _elapsedTimeSpanTxt.text = "Delta time: " + timeSpan;
        }
    }
}