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

        private LTDescr tween;
        private float _prev;
        private float duration = 0.45f;

        public void Show(float value)
        {
            if (tween != null)
            {
                if (LeanTween.isTweening(tween.id))
                {
                    LeanTween.cancel(tween.id);
                    tween = GetLTDescr(_prev, value, duration);
                }
            }

            tween = GetLTDescr(_prev, value, duration).setEase(LeanTweenType.easeOutSine);
        }

        private LTDescr GetLTDescr(float from, float value, float duration)
        {
            return LeanTween.value(from, value, duration)
                    //.setEase(LeanTweenType.easeOutSine)
                    .setOnUpdate((float val) =>
                    {
                        WaveHPText.text = (val).SciFormat().ToString();
                        _prev = val;
                    })
                    .setDelay(0f)
                    .setOnComplete(() => _prev = value);
        }
    }
}