using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelSelectButton : MonoBehaviour
    {
        public void OnButtonClicked(int level)
        {
            MessageBus.Instance.SendMessage(new Message() { Type = MessageType.LEVEL_PASSED, IntValue = level });
        }
    }
}