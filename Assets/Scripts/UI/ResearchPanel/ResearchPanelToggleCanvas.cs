using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ResearchPanelToggleCanvas : MonoBehaviour
    {
        [SerializeField] private Button _researchPanelToggleBtn;

        public event EventHandler<EventArgs> ToggleBtnClicked = (s, e) => { };

        public void Init()
        {
            _researchPanelToggleBtn.onClick.AddListener(() => { ToggleBtnClicked?.Invoke(this, new EventArgs()); });
        }
    }
}