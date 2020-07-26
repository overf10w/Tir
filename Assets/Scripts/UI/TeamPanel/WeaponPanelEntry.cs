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

        // ViewModel
        public WeaponStat DPS { get; private set; }
        public WeaponStat DMG { get; private set; }

        public void Init(float dpsPrice, float dmgPrice)
        {
            InitButtons();
        }

        public void Init(string name, WeaponStat dps, WeaponStat dmg)
        {
            InitButtons();

            DPS = dps;
            DMG = dmg;

            InitIcon(name);

            _nameTxt.text = name;

            _dpsNextPrice.text = dps.Price.SciFormat();
            _dmgNextPrice.text = dmg.Price.SciFormat();

            _dpsValueTxt.text = dps.Value.SciFormat();
            _dmgValueTxt.text = dmg.Value.SciFormat();

            _dpsNextValueTxt.text = dps.NextValue.SciFormat();
            _dmgNextValueTxt.text = dmg.NextValue.SciFormat();
        }

        public void UpdateSelf(WeaponStat dps, WeaponStat dmg)
        {
            _dpsNextPrice.text = dps.Price.SciFormat();
            _dmgNextPrice.text = dmg.Price.SciFormat();

            _dpsValueTxt.text = dps.Value.SciFormat();
            _dmgValueTxt.text = dmg.Value.SciFormat();

            _dpsNextValueTxt.text = dps.NextValue.SciFormat();
            _dmgNextValueTxt.text = dmg.NextValue.SciFormat();
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
