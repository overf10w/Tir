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

        // name

        [SerializeField] private TextMeshProUGUI _nameTxt;

        [SerializeField] private TextMeshProUGUI _statNameTxt;

        // next price
        [SerializeField] private TextMeshProUGUI _dpsNextPrice;
        //[SerializeField] private TextMeshProUGUI _dmgNextPrice;

        // curr value
        [SerializeField] private TextMeshProUGUI _dpsValueTxt;
        //[SerializeField] private TextMeshProUGUI _dmgValueTxt;
        
        // next value
        [SerializeField] private TextMeshProUGUI _dpsNextValueTxt;
        //[SerializeField] private TextMeshProUGUI _dmgNextValueTxt;

        public Button DPSButton { get; private set; }

        public Button DMGButton { get; private set; }

        public WeaponStat DPS { get; private set; }
        public WeaponStat DMG { get; private set; }

        private LTDescr tween;
        private float _prevDps = 0;
        private float duration = 0.5f;

        public void Init(PlayerModel model, string name, WeaponStat dps, WeaponStat dmg)
        {
            InitButtons();

            DPS = dps;
            DMG = dmg;

            InitIcon(name);

            _nameTxt.text = name;

            Render(model, dps, dmg);
        }

        public void Render(PlayerModel model, WeaponStat dps, WeaponStat dmg)
        {
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

            if (tween != null)
            {
                if (LeanTween.isTweening(tween.id))
                {
                    LeanTween.cancel(tween.id);
                    tween = GetLTDescr(_prevDps, dps.Value, duration, color);
                }
            }

            tween = GetLTDescr(_prevDps, dps.Value, duration, color).setEase(LeanTweenType.easeOutSine);

            _dpsValueTxt.text = dps.Value.SciFormat();
            _dpsValueTxt.color = color;

            _dpsNextValueTxt.text = dps.NextValue.SciFormat();
            _dpsNextValueTxt.color = color;
        }

        private LTDescr GetLTDescr(float from, float value, float duration, Color color)
        {
            return LeanTween.value(from, value, duration)
                    //.setEase(LeanTweenType.easeOutSine)
                    .setOnUpdate((float val) =>
                    {
                        _dpsValueTxt.text = val.SciFormat().ToString();
                        _dpsValueTxt.color = color;
                    })
                    .setDelay(0f)
                    .setOnComplete(() => _prevDps = value);
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
