using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class IdleProfitCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _idleProfitTxt;
        [SerializeField] private Button _closeBtn;

        public TextMeshProUGUI IdleProfitTxt => _idleProfitTxt;

        private event EventHandler<EventArgs> HiddenStatusToggle;

        private bool _isHidden = false;
        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value;
                HiddenStatusToggle?.Invoke(this, new EventArgs());
            }
        }

        public void Init()
        {
            _closeBtn.onClick.AddListener(() => IsHidden = true);
            HiddenStatusToggle += HiddenStatusToggleHandler;
        }

        private void HiddenStatusToggleHandler(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }
    }
}