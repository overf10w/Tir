﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Remoting.Channels;
using UnityEngine;

namespace Game
{
    public class EventArgs<T> : EventArgs
    {
        public T Val { get; }

        public EventArgs(T val) 
        {
            Val = val;
        }
    }

    public class PlayerView : MessageHandler
    {
        #region MessageHandler
        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.CUBE_DEATH, MessageType.LEVEL_COMPLETE };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.CUBE_DEATH)
            {
                Cube cube = (Cube)message.objectValue;
                CubeDeath?.Invoke(this, new EventArgs<float>(cube.Gold));
            }
            else if (message.Type == MessageType.LEVEL_COMPLETE)
            {
                LevelPassed?.Invoke(this, new EventArgs());
            }
        }
        #endregion

        [SerializeField] private SoundsMachine _soundsMachine;
        public SoundsMachine SoundsMachine => _soundsMachine;

        public event EventHandler<EventArgs> Clicked = (sender, e) => { };
        public event EventHandler<EventArgs<float>> CubeDeath = (sender, e) => { };
        public event EventHandler<EventArgs> LevelPassed = (sender, e) => { };
        public event EventHandler<EventArgs<WeaponStatBtnClickArgs>> TeamWeaponBtnClicked = (sender, e) => { };
        public event EventHandler<EventArgs<WeaponStatBtnClickArgs>> ClickGunBtnClicked = (sender, e) => { };
        public event EventHandler<EventArgs<int>> WaveChanged = (sender, e) => { };

        [HideInInspector]
        public Gun Gun { get; private set; }

        public UserStatsCanvas Ui { get; private set; }

        public TeamPanel TeamPanel { get; private set; }

        public ClickGunPanel ClickGunPanel { get; private set; }

        public IdleProfitCanvas IdleProfitCanvas { get; private set; }

        public WaveSpawner WaveSpawner { get; private set; }

        public void Init(PlayerModel model, UpgradesSO upgrades)
        {
            InitMessageHandler();

            _soundsMachine.Init();

            WaveSpawner = FindObjectOfType<WaveSpawner>();
            WaveSpawner.PropertyChanged += WaveSpawner_PropertyChanged;

            Ui = FindObjectOfType<UserStatsCanvas>();
            Ui.Init(model, WaveSpawner.WaveInd);

            TeamPanel = FindObjectOfType<TeamPanel>();
            TeamPanel.Init(model);
            if (TeamPanel)
            {
                TeamPanel.WeaponBtnClick.PlayerView = this;
            }

            IdleProfitCanvas = FindObjectOfType<IdleProfitCanvas>();
            IdleProfitCanvas.Init();
            IdleProfitCanvas.IdleProfitTxt.SetText($"While you were away, your team has earned you \n <color=#1FD22C> {model.PlayerStats.IdleProfit.SciFormat()} </color> gold");

            ClickGunPanel = Ui.GetComponentInChildren<ClickGunPanel>();
            ClickGunPanel.Render(model, model.GunData.WeaponName, model.DPS, model.DMG);



            if (ClickGunPanel)
            {
                ClickGunPanel.WeaponBtnClick.PlayerView = this;
            }

            Gun = GetComponentInChildren<Gun>();
        }

        private void WaveSpawner_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            WaveSpawner waveSpawner = (WaveSpawner)sender;
            if (waveSpawner)
            {
                int waveInd = waveSpawner.WaveInd;
                WaveChanged?.Invoke(this, new EventArgs<int>(waveInd));
            }
        }

        private void Update()
        {
            Gun.UpdateGunRotation();
            if (Input.GetMouseButtonDown(0))
            {
                Clicked?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnAutoShoot(float _autoShootDuration)
        {
            StartCoroutine(AutoShoot(_autoShootDuration));
        }

        public IEnumerator AutoShoot(float _autoShootDuration)
        {
            float timer = 0.0f;
            float timeBetweenShots = 0.2f;
            while (timer <= _autoShootDuration)
            {
                timer += Time.deltaTime;
                timeBetweenShots++;
                if (timeBetweenShots >= 0.2f)
                {
                    timeBetweenShots = 0.0f;
                }
                yield return null;
            }
        }

        public void WeaponBtnClickHandler(WeaponStatBtnClickArgs weaponClickInfo)
        {
            TeamWeaponBtnClicked?.Invoke(this, new EventArgs<WeaponStatBtnClickArgs>(weaponClickInfo));
        }

        public void ClickGunBtnClickHandler(WeaponStatBtnClickArgs clickGunClickInfo)
        {
            ClickGunBtnClicked?.Invoke(this, new EventArgs<WeaponStatBtnClickArgs>(clickGunClickInfo));
        }
    }
}