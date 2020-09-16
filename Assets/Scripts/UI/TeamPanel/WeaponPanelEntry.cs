using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class WeaponPanelEntry : MonoBehaviour
    {
        private AssetBundle _assetBundle;

        [SerializeField] private Image _iconImg;
        [SerializeField] private TextMeshProUGUI _nameTxt;
        [SerializeField] private TextMeshProUGUI _statNameTxt;
        [SerializeField] private TextMeshProUGUI _levelTxt;
        [SerializeField] private TextMeshProUGUI _dpsNextPrice;
        [SerializeField] private TextMeshProUGUI _dpsValueTxt;
        [SerializeField] private TextMeshProUGUI _dpsNextValueTxt;

        public Button DPSButton { get; private set; }

        public Button DMGButton { get; private set; }

        private LTDescr _dpsTextTween;
        private float _prevDps = 0;
        private float _duration = 0.5f;

        private bool _initFlag = false;

        public void Render(PlayerModel model, string name, WeaponStat dps, WeaponStat dmg)
        {
            if (!_initFlag)
            {
                InitButtons();
                InitIcon(name);
                _nameTxt.text = name;

                _initFlag = true;
            }

            Color green;
            Color red;

            ColorUtility.TryParseHtmlString("#CCFFC8", out green);
            ColorUtility.TryParseHtmlString("#FF807C", out red);

            Color color;

            if (model.PlayerStats.Gold >= dps.Price)
            {
                color = green;
            } 
            else
            {
                color = red;
            }

            _nameTxt.color = color;
            _statNameTxt.color = color;

            _dpsNextPrice.text = "$" + dps.Price.SciFormat();
            _dpsNextPrice.color = color;

            if (_dpsTextTween != null)
            {
                if (LeanTween.isTweening(_dpsTextTween.id))
                {
                    _dpsValueTxt.color = color;
                    LeanTween.cancel(_dpsTextTween.id);

                    _dpsTextTween = _dpsValueTxt
                        .TweenTMProValue(_prevDps, dps.Value, _duration)
                        .setOnComplete(_ => _prevDps = dps.Value);
                }
            }

            _dpsTextTween = _dpsValueTxt
                .TweenTMProValue(_prevDps, dps.Value, _duration)
                .setOnComplete(_ => _prevDps = dps.Value);
            _dpsValueTxt.color = color;

            _dpsNextValueTxt.text = dps.NextValue.SciFormat();
            _dpsNextValueTxt.color = color;

            _levelTxt.text = dps.Level.ToString();
            _levelTxt.color = color;
        }

        private void InitIcon(string name)
        {
            //if (!assetBundle)
            //{
                _assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "uisprites"));
            //}

            if (_assetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return;
            }

            if (_assetBundle)
            {
                Texture2D tex = _assetBundle.LoadAsset<Texture2D>(name);
                if (tex != null)
                {
                    Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    _iconImg.sprite = mySprite;
                }
                _assetBundle.Unload(false);
            }
        }

        private void InitButtons()
        {
            DPSButton = transform.Find("DPSBtn").GetComponent<Button>();

            DMGButton = transform.Find("DMGBtn").GetComponent<Button>();
        }
    }
}
