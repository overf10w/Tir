using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class WeaponBtnClick
    {
        public PlayerView PlayerView;

        public void Dispatch(string btnName)
        {
            if (PlayerView != null)
            {
                PlayerView.HandleWeaponBtnClick(btnName);
            }
        }
    }

    public class WeaponButton
    {
        public Transform transform;

        public Weapon weapon;

        public Button DpsBtn;
        public Button DmgBtn;
        public TextMeshProUGUI DpsTxt;
        public TextMeshProUGUI DmgTxt;
    }

    public class WeaponBtns
    {
        public const string StandardPistolDpsBtn = "StandardPistol/DPSBtn";
        public const string StandardPistolDmgBtn = "StandardPistol/DMGBtn";
    }

    public class TeamPanel : MonoBehaviour
    {
        public WeaponBtnClick WeaponBtnClick { get; set; }

        private void Awake()
        {
            WeaponBtnClick = new WeaponBtnClick();

            WeaponButton StandardPistol = new WeaponButton();
            
            StandardPistol.DpsBtn = transform.Find("StandardPistol/DPSBtn").GetComponent<Button>();
            StandardPistol.DpsTxt = transform.Find("StandardPistol/DPSBtn/DPSTxt").GetComponent<TextMeshProUGUI>();

            StandardPistol.DmgBtn = transform.Find("StandardPistol/DMGBtn").GetComponent<Button>();
            StandardPistol.DmgTxt = transform.Find("StandardPistol/DMGBtn/DMGTxt").GetComponent<TextMeshProUGUI>();

            StandardPistol.DmgBtn.onClick.AddListener(() => { WeaponBtnClick.Dispatch(WeaponBtns.StandardPistolDmgBtn); });
            StandardPistol.DpsBtn.onClick.AddListener(() => { WeaponBtnClick.Dispatch(WeaponBtns.StandardPistolDpsBtn); });
        }
    }
}