using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerSoundController
    {
        private readonly PlayerView _view;

        public PlayerSoundController(PlayerView view)
        {
            _view = view;
            _view.Clicked += ClickedHandler;
        }

        private void ClickedHandler(object sender, System.EventArgs e)
        {
            _view.SoundsMachine.PlayForced("ShootSound");
            //throw new System.NotImplementedException();
        }
    }
}