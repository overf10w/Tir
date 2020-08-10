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
                PlayerView.ClickGunBtnClickHandler(weaponClickInfo);
            }
        }
    }

    public class ClickGunPanel : MonoBehaviour
    {
        [SerializeField] private ClickGunEntry _clickGunEntry;

        private StatsList _skills;

        public ClickGunBtnClick WeaponBtnClick { get; private set; }

        private ClickGunSkillPanel clickGunSkillPanel;

        private string _name;

        private bool _initFlag = false;

        public void Render(PlayerModel model, string name, WeaponStat dps, WeaponStat dmg)
        {
            if (!_initFlag)
            {
                _skills = model.PlayerStats.ClickGunSkillsList;
                _skills.StatChanged += SkillChangedHandler;

                clickGunSkillPanel = GetComponentInChildren<ClickGunSkillPanel>();
                clickGunSkillPanel.Init(_skills.List);

                WeaponBtnClick = new ClickGunBtnClick();

                _clickGunEntry.Render(model, name, dps, dmg);

                _clickGunEntry.DPSButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs("ClickGun", "DPS")); });
                _clickGunEntry.DMGButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs("ClickGun", "DMG")); });

                _initFlag = true;
                return;
            }

            _clickGunEntry.Render(model, name, dps, dmg);
        }

        private void SkillChangedHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PlayerStat stat = (PlayerStat)sender;
            clickGunSkillPanel.Render(stat);
        }
    }
}