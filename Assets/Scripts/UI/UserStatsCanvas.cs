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
            msc.MessageTypes = new MessageType[] { MessageType.LEVEL_COMPLETE, MessageType.GAME_STARTED };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.LEVEL_COMPLETE)
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

        public TextMeshProUGUI PlayerWaveTxt { get; private set; }

        private Button _playerRestartLevelBtn;

        private LevelListUI _levelListUI;

        private Text _elapsedTimeSpanTxt;

        public void Init(PlayerModel playerModel, int waveInd)
        {
            InitMessageHandler();

            _levelListUI = FindObjectOfType<LevelListUI>();
            _playerGoldCanvas.Render(playerModel.PlayerStats.Gold);

            PlayerLevelTxt = transform.Find("MainPanel/LevelPanel/LevelBtn/LevelTxt").GetComponent<TextMeshProUGUI>();
            PlayerLevelTxt.text = playerModel.PlayerStats.Level.ToString();

            _playerRestartLevelBtn = transform.Find("MainPanel/LevelPanel/RestartBtn").GetComponent<Button>();
            _playerRestartLevelBtn.onClick.AddListener(() => MessageBus.Instance.SendMessage(new Message { Type = MessageType.LEVEL_RESTARTED }));

            PlayerWaveTxt = transform.Find("MainPanel/LevelPanel/LevelBtn/WaveTxt").GetComponent<TextMeshProUGUI>();
            PlayerWaveTxt.text = waveInd.ToString() + "/5";

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