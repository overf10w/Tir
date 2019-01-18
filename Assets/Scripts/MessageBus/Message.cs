using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType
{
    NONE,
    GameOver,
    CubeDeath,
    WaveChanged,
    LevelChanged,
    GameStarted,
    PointAdded
}

public struct Message
{
    public MessageType Type;
    public int IntValue;
    public double DoubleValue;
    public float FloatValue;
    public Vector3 Vector3Value;
    public GameObject GameObjectValue;
    public object objectValue;
}
