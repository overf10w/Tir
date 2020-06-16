using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class MessageHandler : MonoBehaviour
    {
        public abstract void HandleMessage(Message message);
    }
}