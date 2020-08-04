using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Game
{
    public class UpgradesController
    {
        private readonly PlayerModel _playerModel;
        private readonly ResearchPanel _researchPanel;

        private readonly Upgrade[] _upgrades;
        private readonly UpgradesSO _upgradesSo;

        public UpgradesController(PlayerModel playerModel, UpgradesSO upgradesSO, ResearchPanel researchPanel)
        {
            _researchPanel = researchPanel;

            _playerModel = playerModel;
            _playerModel.PlayerStats.PropertyChanged += PlayerStatsChangedHandler;

            _upgrades = upgradesSO.Upgrades;

            _researchPanel.Init(playerModel, upgradesSO);
            _researchPanel.UpgradeBtnClick.UpgradesController = this;
            _researchPanel.ResearchPanelToggleCanvas.ToggleBtnClicked += ToggleBtnClickHandler;

            foreach (var upgrade in _upgrades)
            {
                upgrade.PropertyChanged += UpgradeChangedHandler;
            }
        }

        public void UpgradeBtnClickHandler(UpgradeBtnClickEventArgs e)
        {
            string skillIndexer = e.Upgrade.Skill;

            StatsContainer statsContainer = (StatsContainer)_playerModel.PlayerStats[e.Upgrade.SkillContainer];
            PlayerStat skill = statsContainer.Stats.Find(sk => sk.Name == skillIndexer);

            if (_playerModel.PlayerStats.Gold >= e.Upgrade.Price)
            {
                _playerModel.PlayerStats.Gold -= e.Upgrade.Price;
                float cachedFloat = skill.Value;
                skill.Value = cachedFloat + 1;
                e.Upgrade.IsActive = false;
            }
        }

        private void ToggleBtnClickHandler(object sender, System.EventArgs e)
        {
            _researchPanel.IsHidden = !_researchPanel.IsHidden;
        }

        private void UpgradeChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            Upgrade upgrade = (Upgrade)sender;
        }

        private void PlayerStatsChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Gold")
            {
                _researchPanel.UpdateView();
            }
        }
    }
}