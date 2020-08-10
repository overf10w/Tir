using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ClickGunEntry : MonoBehaviour
    {
        private AssetBundle _assetBundle;

        // icon
        [SerializeField] private Image _iconImg;

        // name
        [SerializeField] private TextMeshProUGUI _nameTxt;

        [SerializeField] private TextMeshProUGUI _statNameTxt;

        // next price
        [SerializeField] private TextMeshProUGUI _dpsNextPrice;
        [SerializeField] private TextMeshProUGUI _dmgNextPrice;

        // curr value
        [SerializeField] private TextMeshProUGUI _dpsValueTxt;
        [SerializeField] private TextMeshProUGUI _dmgValueTxt;
        
        // next value
        [SerializeField] private TextMeshProUGUI _dpsNextValueTxt;
        [SerializeField] private TextMeshProUGUI _dmgNextValueTxt;

        public Button DPSButton { get; private set; }
        public Button DMGButton { get; private set; }

        private LTDescr _tweenDmgText;
        private float _prevDmg = 0;
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

            if (model.PlayerStats.Gold >= dmg.Price)
            {
                color = green;
            }
            else
            {
                color = red;
            }

            _nameTxt.color = color;
            _statNameTxt.color = color;

            if (_tweenDmgText != null)
            {
                if (LeanTween.isTweening(_tweenDmgText.id))
                {
                    LeanTween.cancel(_tweenDmgText.id);

                    _tweenDmgText = _dmgValueTxt.TweenTMProValue(_prevDmg, dmg.Value, _duration)
                            .setOnComplete(_ => _prevDmg = dmg.Value);
                }
            }
            _tweenDmgText = _dmgValueTxt.TweenTMProValue(_prevDmg, dmg.Value, _duration)
                    .setEase(LeanTweenType.easeOutSine)
                    .setOnComplete(_ => _prevDmg = dmg.Value);
            _dmgValueTxt.color = color;

            _dmgNextPrice.text = "$" + dmg.Price.SciFormat();
            _dmgNextPrice.color = color;

            _dmgNextValueTxt.text = dmg.NextValue.SciFormat();
            _dmgNextValueTxt.color = color;
        }

        private void InitIcon(string name)
        {
            _assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "uisprites"));

            if (_assetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
                return;
            }

            if (_assetBundle)
            {
                Texture2D tex = _assetBundle.LoadAsset<Texture2D>(name);
                if (tex != null)
                {
                    Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    _iconImg.sprite = mySprite;
                    _assetBundle.Unload(false);
                }
                else
                {
                    Debug.LogError("ClickGunEntry.cs: Failed to load texture: " + name);
                }
            }
        }

        private void InitButtons()
        {
            DPSButton = transform.Find("DPSBtn").GetComponent<Button>();

            DMGButton = transform.Find("DMGBtn").GetComponent<Button>();
        }
    }
}