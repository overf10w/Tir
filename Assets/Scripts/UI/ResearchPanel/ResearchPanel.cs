using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

namespace Game
{
    public class UpgradeBtnClick
    {
        public PlayerView PlayerView;

        public void Dispatch(UpgradeBtnClickEventArgs clickInfo)
        {
            if (PlayerView != null)
            {
                PlayerView.HandleUpgradeBtnClick(clickInfo);
            }
        }
    }

    // Research buttons to turn red when money isn't enough - [done]
    // ResearchPanelEntry to have a Render() method (just like the clickGunEntry) - [done]
    // ResearchPanel to subscribe to _playerModel.PlayerStats.Gold Change and call ClickGunEntry.Render() - [done]
    // Refactor
    public class ResearchPanel : MonoBehaviour
    {
        public UpgradeBtnClick UpgradeBtnClick { get; private set; }

        private Upgrades.Upgrade[] _upgrades;

        private Upgrades _upgradesSo;

        private CanvasGroup _canvasGroup;
        private GameObject _researchPanelEntryPrefab;
        private Transform _content;

        private bool _isHidden;
        public bool IsHidden
        {
            get
            {
                return _isHidden;
            }

            set
            {
                _isHidden = value;
                if (_isHidden)
                {
                    Hide();
                }
                else
                {
                    Reveal();
                }
            }
        }

        [field: NonSerialized]
        private PlayerModel _playerModel;

        List<ResearchPanelEntry> _researchPanelEntries;

        public void Init(PlayerModel playerModel, Upgrades upgradesSO)
        {
            _researchPanelEntries = new List<ResearchPanelEntry>();

            _playerModel = playerModel;

            _playerModel.PlayerStats.PropertyChanged += HandlePlayerStatsPropertyChanged;

            _upgradesSo = upgradesSO;

            _upgrades = _upgradesSo.upgrades;

            _canvasGroup = GetComponent<CanvasGroup>();

            UpgradeBtnClick = new UpgradeBtnClick();

            _researchPanelEntryPrefab = Resources.Load<GameObject>("Prefabs/UI/ResearchPanel/ResearchPanelEntry");

            _content = transform.Find("ScrollView/Viewport/Content").GetComponent<Transform>();

            foreach (var upgrade in _upgrades)
            {
                GameObject entryGameObject = Instantiate(_researchPanelEntryPrefab, _content);

                ResearchPanelEntry script = entryGameObject.GetComponent<ResearchPanelEntry>();
                script.Init(playerModel, upgrade);
                script.UpgradeBtn.onClick.AddListener(() => { UpgradeBtnClick.Dispatch(new UpgradeBtnClickEventArgs(upgrade)); });

                upgrade.PropertyChanged += HandleUpgradeModelChanged;

                _researchPanelEntries.Add(script);
            }
            StartCoroutine(AutoSave());
        }

        private void HandlePlayerStatsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Gold")
            {
                Debug.Log("ResearchPanel: notified of PlayerStats.Gold change");
                foreach (var script in _researchPanelEntries)
                {
                    script.Render();
                }
            }
        }

        private void HandleUpgradeModelChanged(object sender, PropertyChangedEventArgs args)
        {
            Upgrades.Upgrade upgrade = (Upgrades.Upgrade)sender;
            Debug.Log("ResearchPanelView: HandleResearchViewUpgradeViewModelChanged: " +
                "upgrade.isActive: " + upgrade.IsActive);
        }

        private IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(5.0f);

                UpgradeData[] upgradesData = _upgradesSo.GetUpgradesData();
                string _upgradesSavePath = Path.Combine(Application.persistentDataPath, "upgradesSave.dat");
                ResourceLoader.Save<UpgradeData[]>(_upgradesSavePath, upgradesData);

                string upgradesPath = Path.Combine(Application.persistentDataPath, "upgrades.dat");
                ResourceLoader.Save<Upgrades.Upgrade[]>(upgradesPath, _upgrades);
            }
        }

        public void Hide()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Reveal()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}