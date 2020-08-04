using System;
using System.Collections;
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
            msc.MessageTypes = new MessageType[] { MessageType.CUBE_DEATH, MessageType.LEVEL_PASSED };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.CUBE_DEATH)
            {
                Cube cube = (Cube)message.objectValue;
                CubeDeath?.Invoke(this, new EventArgs<float>(cube.Gold));
            }
            else if (message.Type == MessageType.LEVEL_PASSED)
            {
                LevelPassed?.Invoke(this, new EventArgs());
            }
        }
        #endregion

        public event EventHandler<EventArgs> Clicked = (sender, e) => { };
        public event EventHandler<EventArgs<float>> CubeDeath = (sender, e) => { };
        public event EventHandler<EventArgs> LevelPassed = (sender, e) => { };
        public event EventHandler<EventArgs<WeaponStatBtnClickArgs>> TeamWeaponBtnClicked = (sender, e) => { };
        public event EventHandler<EventArgs<WeaponStatBtnClickArgs>> ClickGunBtnClicked = (sender, e) => { };

        [HideInInspector]
        public Gun Gun { get; private set; }

        public UserStatsCanvas Ui { get; private set; }

        public TeamPanel TeamPanel { get; private set; }

        public ClickGunPanel ClickGunPanel { get; private set; }

        public void Init(PlayerModel model, UpgradesSO upgrades)
        {
            InitMessageHandler();

            Ui = FindObjectOfType<UserStatsCanvas>();
            Ui.Init(model);

            TeamPanel = Ui.GetComponentInChildren<TeamPanel>();
            TeamPanel.Init(model);
            if (TeamPanel)
            {
                TeamPanel.WeaponBtnClick.PlayerView = this;
            }

            ClickGunPanel = Ui.GetComponentInChildren<ClickGunPanel>();
            ClickGunPanel.Init(model, model.GunData.WeaponName, model.DPS, model.DMG);
            if (ClickGunPanel)
            {
                ClickGunPanel.WeaponBtnClick.PlayerView = this;
            }

            Gun = GetComponentInChildren<Gun>();

            Gun.Init(model.GunData, model.PlayerStats);
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