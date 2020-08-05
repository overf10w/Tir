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

        private float _prev;

        private LTDescr tween;
        
        private float duration = 0.9f;

        public void Init(float value)
        {
            _prev = 0;

            LeanTween.value(_prev, value, 0.3f)
                .setEase(LeanTweenType.easeOutSine)
                .setOnUpdate((float val) =>
                {
                    PlayerGoldTxt.text = val.SciFormat().ToString();
                })
                .setOnComplete(() => _prev = value);
        }

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
                        PlayerGoldTxt.text = (val).SciFormat().ToString();
                        _prev = val;
                    })
                    .setDelay(0f)
                    .setOnComplete(() => _prev = value);
        }

        //// "GOOD"
        //private LTDescr tween;
        //public void Show(float value)
        //{
        //    if (tween != null)
        //    {
        //        Debug.Log("tween.isTweening: " + (LeanTween.isTweening(tween.id).ToString()));
        //    }
        //    tween = LeanTween.value(_currValue, value, 0.3f)
        //    .setEase(LeanTweenType.easeOutSine)
        //    .setOnUpdate((float val) =>
        //    {
        //        PlayerGoldTxt.text = ((int)val).ToString();
        //        //Debug.Log("Elapsed Time: " + Time.time);
        //        _currValue = val;
        //    })
        //    .setDelay(0f)
        //    .setOnComplete(() => _currValue = value);
        //}
        //// ENDOF "GOOD"



        //private float defaultPlaytime = 0.3f;
        ////private float prevTimestamp;

        //private LTDescr id;
        //public void Show(float value)
        //{
        //    if (id != null)
        //    {
        //        defaultPlaytime += defaultPlaytime;
        //        id.setTime(defaultPlaytime);
        //        //id.setValue3();
        //        return;
        //    }
        //    else
        //    {
        //        defaultPlaytime = 0.3f;

        //        id = LeanTween.value(_currValue, value, defaultPlaytime)
        //                .setEase(LeanTweenType.easeOutSine)
        //                .setOnUpdate((float val) =>
        //                {
        //                    PlayerGoldTxt.text = ((int)val).ToString();
        //                                //Debug.Log("Elapsed Time: " + Time.time);
        //                                //_currValue = val;
        //                            })
        //                .setDelay(0f)
        //                .setOnComplete(() => _currValue = value);
        //    }
        //    //if (Time.time - prevTimestamp <= 2.0f)
        //    //{
        //    //    // change the currently running tween by adding to its time
        //    //}
        //    //else
        //    //{
        //    //    // call sequence
        //    //}
        //    //prevTimestamp = Time.time;
        //}






        //public IEnumerator Show(float value)
        //{
        //    isActive = true;
        //    LeanTween.value(_currValue, value, 1.6f)
        //    .setEase(LeanTweenType.easeOutCirc)
        //    .setOnUpdate((float val) =>
        //    {
        //        PlayerGoldTxt.text = ((int)val).ToString();
        //        Debug.Log("Elapsed Time: " + Time.time);
        //    }).setOnComplete(() => { _currValue = value; isActive = false; });

        //    _showGoldQueue.Run()
        //}
    }
}