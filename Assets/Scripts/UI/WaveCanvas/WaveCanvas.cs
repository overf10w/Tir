using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class WaveCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveHPText;
        public TextMeshProUGUI WaveHPText => _waveHPText;

        private LTDescr _tweenHpText;
        private float _prev;
        private float _duration = 0.45f;

        public void Render(float value)
        {
            if (_tweenHpText != null)
            {
                if (LeanTween.isTweening(_tweenHpText.id))
                {
                    LeanTween.cancel(_tweenHpText.id);
                    _tweenHpText = _waveHPText.TweenTMProValue(_prev, value, _duration)
                                   .setOnComplete(_ => _prev = value);
                }
            }
            _tweenHpText = _waveHPText.TweenTMProValue(_prev, value, _duration)
                           .setEase(LeanTweenType.easeOutSine)
                           .setOnComplete(_ => _prev = value);
        }
    }
}