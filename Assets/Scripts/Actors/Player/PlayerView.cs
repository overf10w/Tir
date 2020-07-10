using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using UnityEngine;

namespace Game
{
    public class CustomArgs : EventArgs
    {
        public float val;

        public CustomArgs(float val)
        {
            this.val = val;
        }
    }

    public class GenericEventArgs<T> : EventArgs
    {
        public T val;

        public GenericEventArgs(T val) 
        {
            this.val = val;
        }
    }

    public class PlayerView : MessageHandler
    {
        public event EventHandler<EventArgs> OnClicked = (sender, e) => { };
        public event EventHandler<CustomArgs> OnCubeDeath = (sender, e) => { };
        public event EventHandler<GenericEventArgs<WeaponStatBtnClickArgs>> OnTeamWeaponBtnClick = (sender, e) => { };
        public event EventHandler<GenericEventArgs<WeaponStatBtnClickArgs>> OnClickGunBtnClick = (sender, e) => { };

        public Gun Gun;

        public UserStatsCanvas Ui;

        public TeamPanel TeamPanel;

        public ClickGunPanel ClickGunPanel;

        public void Init(PlayerModel model)
        {
            InitMessageHandler();

            Ui = FindObjectOfType<UserStatsCanvas>();
            Ui.Init(model);

            TeamPanel = Ui.GetComponentInChildren<TeamPanel>();
            TeamPanel.Init(model.teamWeapons);
            if (TeamPanel)
            {
                TeamPanel.WeaponBtnClick.PlayerView = this;
            }

            ClickGunPanel = Ui.GetComponentInChildren<ClickGunPanel>();
            ClickGunPanel.Init(model.gunData.weaponName, model.DPS, model.DMG);
            if (ClickGunPanel)
            {
                ClickGunPanel.WeaponBtnClick.PlayerView = this;
            }

            Gun = GetComponentInChildren<Gun>();

            Gun.Init(model.gunData, model.gunAlgorithmHolder.DPS, model.gunAlgorithmHolder.DMG);

            Debug.Log("PlayerView: Gun == null: " + (Gun == null).ToString());
        }

        private void Update()
        {
            Gun.UpdateGunRotation();
            if (Input.GetMouseButton(0))
            {
                OnClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.CubeDeath };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.CubeDeath)
            {
                Cube cube = (Cube)message.objectValue;
                OnCubeDeath?.Invoke(this, new CustomArgs(cube.Gold));
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


        //public void OnDisable()
        //{
        //    //model.OnAutoFireUpdated -= OnAutoShoot;
        //    //ResourceLoader.instance.Write(model.GetStats());
        //}

        //
    }
}