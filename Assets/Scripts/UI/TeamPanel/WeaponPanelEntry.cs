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
        //[SerializeField] private TextMeshProUGUI _dmgNextPrice;

        // curr value
        [SerializeField] private TextMeshProUGUI _dpsValueTxt;
        //[SerializeField] private TextMeshProUGUI _dmgValueTxt;
        
        // next value
        [SerializeField] private TextMeshProUGUI _dpsNextValueTxt;
        //[SerializeField] private TextMeshProUGUI _dmgNextValueTxt;

        public Button DPSButton { get; private set; }

        public Button DMGButton { get; private set; }

        // ViewModel
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
            if (model.PlayerStats.Gold >= dps.Price)
            {
                _dpsNextPrice.color = Color.green;
                _dpsValueTxt.color = Color.green;
                _dpsNextValueTxt.color = Color.green;
            } 
            else
            {
                _dpsNextPrice.color = Color.red;
                _dpsValueTxt.color = Color.red;
                _dpsNextValueTxt.color = Color.red;
            }
            _dpsNextPrice.text = "$" + dps.Price.SciFormat();
            _dpsValueTxt.text = dps.Value.SciFormat();
            _dpsNextValueTxt.text = dps.NextValue.SciFormat();
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
