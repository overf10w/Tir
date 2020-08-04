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

        private CoroutineQueue _showGoldQueue;

        public float _prevValue;
        public float _currValue;

        LTSeq seq;
        public void Init(float value)
        {
            _prevValue = _currValue = value;
            _showGoldQueue = new CoroutineQueue(1, StartCoroutine);

            seq = LeanTween.sequence();

            seq.append(() =>
            {
                LeanTween.value(0, _currValue, 0.3f)
                    .setEase(LeanTweenType.easeOutSine)
                    .setOnUpdate((float val) =>
                    {
                        PlayerGoldTxt.text = ((int)val).ToString();
                        //Debug.Log("Elapsed Time: " + Time.time);
                    });
            });
        }

        public void Show(float value)
        {
            seq.append(() =>
            {
                LeanTween.value(_currValue, value, 0.3f)
                .setEase(LeanTweenType.easeOutSine)
                .setOnUpdate((float val) =>
                {
                    PlayerGoldTxt.text = ((int)val).ToString();
                    //Debug.Log("Elapsed Time: " + Time.time);
                }).setOnComplete(() => _currValue = value );
            });
        }

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

        //// Start is called before the first frame update
        //void Start()
        //{
        //
        //}

        //// Update is called once per frame
        //void Update()
        //{
        //
        //}
    }
}