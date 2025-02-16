﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum MessageType
    {
        NONE,
        GAMEOVER,
        CUBE_DEATH,
        WAVE_CHANGED,
        LEVEL_COMPLETE,
        GAME_STARTED,
        POINT_ADDED,
        LEVEL_RESTARTED
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
}