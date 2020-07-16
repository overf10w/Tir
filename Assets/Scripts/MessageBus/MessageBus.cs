using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MessageBus
    {
        #region Singleton
        /* Singleton */
        static MessageBus instance;

        public static MessageBus Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageBus();
                }
                return instance;
            }
        }

        private MessageBus() { }
        #endregion

        private readonly Dictionary<MessageType, List<MessageSubscriber>> _subscriberLists =
            new Dictionary<MessageType, List<MessageSubscriber>>();

        public void AddSubscriber(MessageSubscriber subscriber)
        {
            MessageType[] messageTypes = subscriber.MessageTypes;
            for (int i = 0; i < messageTypes.Length; i++)
            {
                AddSubscriberToMessage(messageTypes[i], subscriber);
            }
        }

        public void SendMessage(Message message)
        {
            if (!_subscriberLists.ContainsKey(message.Type))
            {
                return;
            }

            List<MessageSubscriber> subscriberList = _subscriberLists[message.Type];

            for (int i = 0; i < subscriberList.Count; i++)
            {
                SendMessageToSubscriber(message, subscriberList[i]);
            }
        }

        public static void Nullify()
        {
            instance = null;
        }

        private void AddSubscriberToMessage(MessageType messageType, MessageSubscriber subscriber)
        {
            if (!_subscriberLists.ContainsKey(messageType))
            {
                _subscriberLists[messageType] = new List<MessageSubscriber>();
            }
            _subscriberLists[messageType].Add(subscriber);
        }

        private void SendMessageToSubscriber(Message message, MessageSubscriber subscriber)
        {
            subscriber.Handler.HandleMessage(message);
        }
    }
}