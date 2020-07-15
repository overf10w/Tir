using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ScrollbarIncrementer : MonoBehaviour
    {
        public ScrollRect levelList;
        private LevelListUI _levelListScript;
        public Button theOtherButton;

        public void Start()
        {
            _levelListScript = levelList.GetComponent<LevelListUI>();
        }

        public void Increment()
        {
            if (_levelListScript == null || theOtherButton == null) throw new Exception("Setup ScrollbarIncrementer first!");
            _levelListScript.Increment();
            GetComponent<Button>().interactable = _levelListScript.HorizontalNormalizedPosition() <= 0.999f;
            theOtherButton.interactable = true;
        }

        public void Decrement()
        {
            if (_levelListScript == null || theOtherButton == null) throw new Exception("Setup ScrollbarIncrementer first!");
            _levelListScript.Decrement();
            GetComponent<Button>().interactable = _levelListScript.HorizontalNormalizedPosition() >= 0.001f;
            theOtherButton.interactable = true;
        }
    }
}