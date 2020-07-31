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
        [SerializeField] private ClickGunEntry _clickGunEntry;

        private StatsContainer _skills;

        public ClickGunBtnClick WeaponBtnClick { get; private set; }

        private ClickGunSkillPanel clickGunSkillPanel;

        public void Init(PlayerModel model, string name, WeaponStat dps, WeaponStat dmg)
        {
            _skills = model.PlayerStats.ClickGunSkills;
            _skills.StatChanged += HandleSkillChanged;

            clickGunSkillPanel = GetComponentInChildren<ClickGunSkillPanel>();
            clickGunSkillPanel.Init(_skills.Stats);

            WeaponBtnClick = new ClickGunBtnClick();
            _clickGunEntry.Init(model, name, dps, dmg);
            _clickGunEntry.DPSButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs("ClickGun", "DPS")); Debug.Log("CLickGunPanel. Click DPS"); }); 
            _clickGunEntry.DMGButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs("ClickGun", "DMG")); Debug.Log("CLickGunPanel. Click DMG"); });
        }

        private void HandleSkillChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PlayerStat stat = (PlayerStat)sender;

            Debug.Log("ClickGunPanel: HandleSkillChanged: Skill.Name: " + stat.Name + ", Skill.Value: " + stat.Value);
            clickGunSkillPanel.UpdateSelf(stat);
        }

        public void UpdateView(PlayerModel model, WeaponStat dps, WeaponStat dmg)
        {
            _clickGunEntry.Render(model, dps, dmg);
        }
    }
}