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

            _upgradesSo = upgradesSO;

            _researchPanel.Init(playerModel, upgradesSO);
            _researchPanel.UpgradeBtnClick.UpgradesController = this;
            _researchPanel.ResearchPanelToggleCanvas.ToggleBtnClicked += ToggleBtnClickHandler;
            _researchPanel.AutoSaveTriggered += AutoSaveHandler;
        }

        public void UpgradeBtnClickHandler(UpgradeBtnClickEventArgs e)
        {
            string skillIndexer = e.Upgrade.Stat;

            StatsList statsList = (StatsList)_playerModel.PlayerStats[e.Upgrade.StatsList];

            PlayerStat skill = statsList.List.Find(sk => sk.Name == skillIndexer);

            if (_playerModel.PlayerStats.Gold >= e.Upgrade.Price)
            {
                _playerModel.PlayerStats.Gold -= e.Upgrade.Price;
                float cachedFloat = skill.Value;
                skill.Value = cachedFloat + (e.Upgrade.Amount / 100.0f);
                e.Upgrade.IsActive = false;
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