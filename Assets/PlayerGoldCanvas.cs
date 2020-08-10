using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class PlayerGoldCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerGoldText;
        public TextMeshProUGUI PlayerGoldTxt => _playerGoldText;

        private LTDescr _goldTextTween;
        private float _prev = 0;
        private float _duration = 0.9f;

        public void Render(float value)
        {
            if (_goldTextTween != null)
            {
                if (LeanTween.isTweening(_goldTextTween.id))
                {
                    LeanTween.cancel(_goldTextTween.id);

                    _goldTextTween = _playerGoldText.TweenTMProValue(_prev, value, _duration)
                                     .setOnComplete(_ => _prev = value);
                }
            }
            _goldTextTween = _playerGoldText.TweenTMProValue(_prev, value, _duration)
                             .setEase(LeanTweenType.easeOutSine)
                             .setOnComplete(_ => _prev = value);
        }
    }
}