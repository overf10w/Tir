using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class WeaponPanelEntry : MonoBehaviour
    {
        // View

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

        public void Init(WeaponStat dps, WeaponStat dmg)
        {
            InitButtons();

            DPS = dps;
            DMG = dmg;

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
