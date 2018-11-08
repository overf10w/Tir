using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType
{
    NONE,
    GameOver,
    CubeDeath,
    PointAdded
}

public struct Message
{
    public MessageType Type;
    public int IntValue;
    public float FloatValue;
    public Vector3 Vector3Value;
    public GameObject GameObjectValue;
}
