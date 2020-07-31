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

        public WeaponStat DPS { get; private set; }
        public WeaponStat DMG { get; private set; }

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
            Debug.Log("WeaponPanelEntry: Render()");

            Color green;
            Color red;

            ColorUtility.TryParseHtmlString("#CCFFC8", out green);
            ColorUtility.TryParseHtmlString("#FF807C", out red);

            if (model.PlayerStats.Gold >= dmg.Price)
            {
                _nameTxt.color = green;
                _statNameTxt.color = green;
                _dmgNextPrice.color = green;
                _dmgValueTxt.color = green;
                _dmgNextValueTxt.color = green;
            }
            else
            {
                _nameTxt.color = red;
                _statNameTxt.color = red;
                _dmgNextPrice.color = red;
                _dmgValueTxt.color = red;
                _dmgNextValueTxt.color = red;
            }


            _dmgNextPrice.text = "$" + dmg.Price.SciFormat();
            _dmgValueTxt.text = dmg.Value.SciFormat();
            _dmgNextValueTxt.text = dmg.NextValue.SciFormat();
        }

        //public void Init(string name, WeaponStat dps, WeaponStat dmg)
        //{
        //    InitButtons();

        //    DPS = dps;
        //    DMG = dmg;

        //    InitIcon(name);

        //    _nameTxt.text = name;

        //    _dpsNextPrice.text = dps.Price.SciFormat();
        //    _dmgNextPrice.text = dmg.Price.SciFormat();

        //    _dpsValueTxt.text = dps.Value.SciFormat();
        //    _dmgValueTxt.text = dmg.Value.SciFormat();

        //    _dpsNextValueTxt.text = dps.NextValue.SciFormat();
        //    _dmgNextValueTxt.text = dmg.NextValue.SciFormat();
        //}

        //public void Render(WeaponStat dps, WeaponStat dmg)
        //{
        //    _dpsNextPrice.text = dps.Price.SciFormat();
        //    _dmgNextPrice.text = dmg.Price.SciFormat();

        //    _dpsValueTxt.text = dps.Value.SciFormat();
        //    _dmgValueTxt.text = dmg.Value.SciFormat();

        //    _dpsNextValueTxt.text = dps.NextValue.SciFormat();
        //    _dmgNextValueTxt.text = dmg.NextValue.SciFormat();
        //}

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