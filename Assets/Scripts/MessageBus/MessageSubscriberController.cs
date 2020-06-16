using UnityEngine;
using System.Collections;

namespace Game
{
    public class MessageSubscriberController : MonoBehaviour
    {
        public MessageType[] MessageTypes;
        public MessageHandler Handler;

        void Start()
        {
            MessageSubscriber subscriber = new MessageSubscriber();
            subscriber.MessageTypes = MessageTypes;
            subscriber.Handler = Handler;

            MessageBus.Instance.AddSubscriber(subscriber);
        }
    }
}