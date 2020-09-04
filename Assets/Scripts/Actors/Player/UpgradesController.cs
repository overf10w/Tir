using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

namespace Game
{
    public class UpgradesController
    {
        private readonly PlayerModel _playerModel;
        private readonly ResearchPanel _researchPanel;
        private readonly UpgradesSO _upgradesSo;

        public UpgradesController(PlayerModel playerModel, UpgradesSO upgradesSO, ResearchPanel researchPanel)
        {
            _researchPanel = researchPanel;

            _playerModel = playerModel;

            _playerModel.PlayerStats.PropertyChanged += PlayerStatsChangedHandler;
            _playerModel.PlayerStats.TeamSkillsList.StatChanged += TeamSkillsListStatChanged;
            _playerModel.PlayerStats.ClickGunSkillsList.StatChanged += ClickGunSkillsListStatChanged;

            _upgradesSo = upgradesSO;

            _researchPanel.Init(playerModel, upgradesSO);
            _researchPanel.UpgradeBtnClick.UpgradesController = this;
            _researchPanel.ResearchPanelToggleCanvas.ToggleBtnClicked += ToggleBtnClickHandler;
            _researchPanel.AutoSaveTriggered += AutoSaveHandler;
        }

        private /*async*/ void ClickGunSkillsListStatChanged(object sender, PropertyChangedEventArgs e)
        {
            //await UniTask.Delay(1);  // For Criteria to validate its referenced Upgrade (assigned in inspector) isn't IsActive no more
            _researchPanel.UpdateView();
            Debug.Log("_researchPanel.UpdateView");
        }

        private /*async*/ void TeamSkillsListStatChanged(object sender, PropertyChangedEventArgs e)
        {
            //await UniTask.Delay(1);  // For Criteria to validate its referenced Upgrade (assigned in inspector) isn't IsActive no more
            _researchPanel.UpdateView();
            Debug.Log("_researchPanel.UpdateView");
        }

        public void UpgradeBtnClickHandler(UpgradeBtnClickEventArgs e)
        {
            string skillIndexer = e.Upgrade.Stat;

            StatsList statsList = (StatsList)_playerModel.PlayerStats[e.Upgrade.StatsList];

            PlayerStat skill = statsList.List.Find(sk => sk.Name == skillIndexer);

            if (_playerModel.PlayerStats.Gold >= e.Upgrade.Price)
            {
                // Upgrade.IsActive should always come first, as 
                //  1) By changing skill.Value we trigger events in _playerModel.PlayerStats.(Team/ClickGun)StatsList
                //  2) To which the UpgradesController subscibes to with (Team/ClickGun)SkillsListStatChanged methods
                //  3) (Team/ClickGun)SkillsListStatChanged: we check whether Upgrade.IsActive, so it's crucial to update it before setting skill.Value
                e.Upgrade.IsActive = false;
                _playerModel.PlayerStats.Gold -= e.Upgrade.Price;
                float cachedFloat = skill.Value;
                skill.Value = cachedFloat + (e.Upgrade.Amount / 100.0f);
            }
        }

        private void ToggleBtnClickHandler(object sender, System.EventArgs e)
        {
            _researchPanel.IsHidden = !_researchPanel.IsHidden;
        }

        private void PlayerStatsChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Gold")
            {
                _researchPanel.UpdateView();
            }
        }

        private void AutoSaveHandler(object sender, EventArgs e)
        {
            UpgradeData[] upgradesData = _upgradesSo.GetUpgradesData();
            string _upgradesSavePath = Path.Combine(Application.persistentDataPath, "upgradesSave.dat");
            ResourceLoader.Save<UpgradeData[]>(_upgradesSavePath, upgradesData);
        }
    }
}