using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public class InputEventArgs : EventArgs
    {
        public enum INPUT_KEY_CODE
        {
            NUM_KEY_1,
            NUM_KEY_2,
            NUM_KEY_3,
            NUM_KEY_4,
            NUM_KEY_5,
            DPS_MULTIPLIER
        }
        public const string NUM_KEY_1 = "1";
        public const string NUM_KEY_2 = "2";
        public const string NUM_KEY_3 = "3";
        public const string NUM_KEY_4 = "4";
        public const string NUM_KEY_5 = "5";

        public INPUT_KEY_CODE KeyCode;

        public InputEventArgs(INPUT_KEY_CODE keyCode)
        {
            KeyCode = keyCode;
        }
    }
    /// <summary>
    /// Used primarily for cases when player presses special keys like 1, 2, etc.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public event EventHandler<InputEventArgs> OnKeyPress;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                OnKeyPress?.Invoke(this, new InputEventArgs(InputEventArgs.INPUT_KEY_CODE.NUM_KEY_1));
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                OnKeyPress?.Invoke(this, new InputEventArgs(InputEventArgs.INPUT_KEY_CODE.DPS_MULTIPLIER));
            }
        }
    }
}