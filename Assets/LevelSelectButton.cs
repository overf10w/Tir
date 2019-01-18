using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public void OnButtonClicked(int level)
    {
        MessageBus.Instance.SendMessage(new Message() {Type = MessageType.LevelChanged, IntValue = level});
    }
}
