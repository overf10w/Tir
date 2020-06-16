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
        public event EventHandler<GenericEventArgs<string>> OnUpdateWeaponBtnClick = (sender, e) => { };

        public Gun Gun;

        public UserStatsCanvas Ui;

        public TeamPanel TeamPanel;

        private void Start()
        {
            Ui = FindObjectOfType<UserStatsCanvas>();
            TeamPanel = Ui.GetComponentInChildren<TeamPanel>();
            if (TeamPanel)
            {
                TeamPanel.WeaponBtnClick.PlayerView = this;
            }
            Gun = GetComponentInChildren<Gun>();
        }

        private void Update()
        {
            Gun.UpdateGunRotation();
            if (Input.GetMouseButton(0))
            {
                OnClicked?.Invoke(this, EventArgs.Empty);
            }
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

        public void HandleWeaponBtnClick(string btnName)
        {
            OnUpdateWeaponBtnClick?.Invoke(this, new GenericEventArgs<string>(btnName));
        }

        //public void OnDisable()
        //{
        //    //model.OnAutoFireUpdated -= OnAutoShoot;
        //    //ResourceLoader.instance.Write(model.GetStats());
        //}

        //
    }
}