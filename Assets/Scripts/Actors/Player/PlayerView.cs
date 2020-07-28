using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using UnityEngine;

namespace Game
{
    public class CustomArgs : EventArgs
    {
        public float Val { get; }

        public CustomArgs(float val)
        {
            Val = val;
        }
    }

    public class GenericEventArgs<T> : EventArgs
    {
        public T Val { get;}

        public GenericEventArgs(T val) 
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
                OnCubeDeath?.Invoke(this, new CustomArgs(cube.Gold));
            }
            else if (message.Type == MessageType.LEVEL_PASSED)
            {
                OnLevelPassed?.Invoke(this, new EventArgs());
            }
        }
        #endregion

        public event EventHandler<EventArgs> OnClicked = (sender, e) => { };
        public event EventHandler<CustomArgs> OnCubeDeath = (sender, e) => { };
        public event EventHandler<EventArgs> OnLevelPassed = (sender, e) => { };
        public event EventHandler<GenericEventArgs<WeaponStatBtnClickArgs>> OnTeamWeaponBtnClick = (sender, e) => { };
        public event EventHandler<GenericEventArgs<WeaponStatBtnClickArgs>> OnClickGunBtnClick = (sender, e) => { };
        public event EventHandler<UpgradeBtnClickEventArgs> OnResearchBtnClick = (sender, e) => { };
        public event EventHandler<EventArgs> OnResearchCenterToggleBtnClick = (sender, e) => { };

        [HideInInspector]
        public Gun Gun { get; private set; }

        public UserStatsCanvas Ui { get; private set; }

        public TeamPanel TeamPanel { get; private set; }

        public ClickGunPanel ClickGunPanel { get; private set; }

        public ResearchPanel ResearchPanel { get; private set; }

        public ResearchPanelToggleCanvas ResearchToggleCanvas { get; private set; }

        public void Init(PlayerModel model, Upgrades.Upgrade[] upgrades)
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
            ClickGunPanel.Init(model.PlayerStats, model.GunData.WeaponName, model.DPS, model.DMG);
            if (ClickGunPanel)
            {
                ClickGunPanel.WeaponBtnClick.PlayerView = this;
            }

            ResearchPanel = Ui.GetComponentInChildren<ResearchPanel>();
            ResearchPanel.Init(upgrades);
            if (ResearchPanel)
            {
                ResearchPanel.UpgradeBtnClick.PlayerView = this;
            }

            ResearchToggleCanvas = Ui.ResearchPanelToggleCanvas;
            ResearchToggleCanvas.Init();
            ResearchToggleCanvas.ToggleBtnClick.PlayerView = this;

            Gun = GetComponentInChildren<Gun>();

            Gun.Init(model.GunData, model.PlayerStats);
        }

        private void Update()
        {
            Gun.UpdateGunRotation();
            if (Input.GetMouseButtonDown(0))
            {
                OnClicked?.Invoke(this, EventArgs.Empty);
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
                    // gun.Shoot(model.currentDamage);
                    timeBetweenShots = 0.0f;
                }
                yield return null;
            }
        }

        public void HandleWeaponBtnClick(WeaponStatBtnClickArgs weaponClickInfo)
        {
            OnTeamWeaponBtnClick?.Invoke(this, new GenericEventArgs<WeaponStatBtnClickArgs>(weaponClickInfo));
        }

        public void HandleClickGunBtnClick(WeaponStatBtnClickArgs clickGunClickInfo)
        {
            OnClickGunBtnClick?.Invoke(this, new GenericEventArgs<WeaponStatBtnClickArgs>(clickGunClickInfo));
        }

        public void HandleUpgradeBtnClick(UpgradeBtnClickEventArgs clickInfo)
        {
            OnResearchBtnClick?.Invoke(this, clickInfo);
        }

        internal void HandleResearchPanelToggleBtnClick()
        {
            OnResearchCenterToggleBtnClick?.Invoke(this, new EventArgs());
        }

        //public void OnDisable()
        //{
        //    //model.OnAutoFireUpdated -= OnAutoShoot;
        //    //ResourceLoader.instance.Write(model.GetStats());
        //}

        //
    }
}