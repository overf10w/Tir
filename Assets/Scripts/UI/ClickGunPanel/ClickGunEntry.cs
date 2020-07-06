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
        private AssetBundle assetBundle;

        // View

        // icon
        [SerializeField]
        private Image IconImg;

        // name
        [SerializeField]
        private TextMeshProUGUI NameTxt;

        // next price
        [SerializeField]
        private TextMeshProUGUI DPSNextPrice;
        [SerializeField]
        private TextMeshProUGUI DMGNextPrice;

        // curr value
        [SerializeField]
        private TextMeshProUGUI DPSValueTxt;
        [SerializeField]
        private TextMeshProUGUI DMGValueTxt;
        
        // next value
        [SerializeField]
        private TextMeshProUGUI DPSNextValueTxt;
        [SerializeField]
        private TextMeshProUGUI DMGNextValueTxt;

        [HideInInspector]
        public Button DPSButton;

        [HideInInspector]
        public Button DMGButton;

        // ViewModel
        [HideInInspector]
        public WeaponStat DPS;

        [HideInInspector]
        public WeaponStat DMG;

        public void Init(float dpsPrice, float dmgPrice)
        {
            InitButtons();

            //DPSPrice.text = dpsPrice.ToString();
            //DMGPrice.text = dmgPrice.ToString();
        }

        private void InitIcon(string name)
        {
            //if (!assetBundle)
            //{
            assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "uisprites"));
            //}

            if (assetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
                return;
            }

            if (assetBundle)
            {
                Texture2D tex = assetBundle.LoadAsset<Texture2D>(name);
                if (tex != null)
                {
                    Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    IconImg.sprite = mySprite;
                    assetBundle.Unload(false);
                }
                else
                {
                    Debug.LogError("ClickGunEntry.cs: Failed to load texture: " + name);
                }
            }
        }

        public void Init(string name, WeaponStat dps, WeaponStat dmg)
        {

            InitButtons();

            DPS = dps;
            DMG = dmg;

            InitIcon(name);

            NameTxt.text = name;

            DPSNextPrice.text = dps.Price.ToString();
            DMGNextPrice.text = dmg.Price.ToString();

            DPSValueTxt.text = dps.Value.ToString();
            DMGValueTxt.text = dmg.Value.ToString();

            DPSNextValueTxt.text = dps.NextValue.ToString();
            DMGNextValueTxt.text = dmg.NextValue.ToString();
        }

        public void UpdateSelf(WeaponStat dps, WeaponStat dmg)
        {
            DPSNextPrice.text = dps.Price.ToString();
            DMGNextPrice.text = dmg.Price.ToString();

            DPSValueTxt.text = dps.Value.ToString();
            DMGValueTxt.text = dmg.Value.ToString();

            DPSNextValueTxt.text = dps.NextValue.ToString();
            DMGNextValueTxt.text = dmg.NextValue.ToString();
        }

        private void InitButtons()
        {
            DPSButton = transform.Find("DPSBtn").GetComponent<Button>();

            DMGButton = transform.Find("DMGBtn").GetComponent<Button>();
        }

        public void UpdateSelf(float dpsPrice, float dmgPrice)
        {
            //DPSPrice.text = dpsPrice.ToString();
            //DMGPrice.text = dmgPrice.ToString();
        }
    }
}