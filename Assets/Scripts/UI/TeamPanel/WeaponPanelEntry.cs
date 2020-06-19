using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class WeaponPanelEntry : MonoBehaviour
    {
        //[HideInInspector]
        //public 

        [SerializeField]
        private TextMeshProUGUI DPSPrice;

        [SerializeField]
        private TextMeshProUGUI DMGPrice;

        [HideInInspector]
        public Button DPSButton;

        [HideInInspector]
        public Button DMGButton;

        public void Init(float dpsPrice, float dmgPrice)
        {
            InitButtons();

            DPSPrice.text = dpsPrice.ToString();
            DMGPrice.text = dmgPrice.ToString();
        }

        private void InitButtons()
        {
            DPSButton = transform.Find("DPSBtn").GetComponent<Button>();

            DMGButton = transform.Find("DMGBtn").GetComponent<Button>();
        }

        public void UpdateSelf(float dpsPrice, float dmgPrice)
        {
            DPSPrice.text = dpsPrice.ToString();
            DMGPrice.text = dmgPrice.ToString();
        }
    }
}
