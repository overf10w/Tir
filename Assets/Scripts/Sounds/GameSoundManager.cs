using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameSoundManager : MessageHandler
    {
        #region MessageHandler
        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.LEVEL_PASSED };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            switch(message.Type)
            {
                case MessageType.LEVEL_PASSED:
                    _soundsMachine.Play("FireWork");
                    break;
                default:
                    break;
            }
        }
        #endregion

        [SerializeField] private SoundsMachine _soundsMachine;

        private void Awake()
        {
            _soundsMachine.Init();
            InitMessageHandler();
        }
    }
}