using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ClickGunBtnClick
    {
        public PlayerView PlayerView;

        public void Dispatch(WeaponStatBtnClickArgs weaponClickInfo)
        {
            if (PlayerView != null)
            {
                PlayerView.HandleClickGunBtnClick(weaponClickInfo);
            }
        }
    }
    public class ClickGunPanel : MonoBehaviour
    {
        [SerializeField]
        private ClickGunEntry _clickGunEntry;

        public ClickGunBtnClick WeaponBtnClick { get; set; }

        public void Init(string name, WeaponStat dps, WeaponStat dmg)
        {
            WeaponBtnClick = new ClickGunBtnClick();
            _clickGunEntry.Init(name, dps, dmg);
            _clickGunEntry.DPSButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs("ClickGun", "DPS")); Debug.Log("CLickGunPanel. Click DPS"); }); 
            _clickGunEntry.DMGButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs("ClickGun", "DMG")); Debug.Log("CLickGunPanel. Click DMG"); });
        }

        public void UpdateClickGunPanel(WeaponStat dps, WeaponStat dmg)
        {
            _clickGunEntry.UpdateSelf(dps, dmg);
        }
    }
}