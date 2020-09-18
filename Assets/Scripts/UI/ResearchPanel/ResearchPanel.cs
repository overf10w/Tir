using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UpgradeBtnClickEventArgs : EventArgs
    {
        public Upgrade Upgrade { get; }
        public UpgradeBtnClickEventArgs(Upgrade upgrade)
        {
            Upgrade = upgrade;
        }
    }

    public class UpgradeBtnClick
    {
        public UpgradesController UpgradesController { get; set; }

        public void Dispatch(UpgradeBtnClickEventArgs clickInfo)
        {
            if (UpgradesController != null)
            {
                UpgradesController.UpgradeBtnClickHandler(clickInfo);
            }
        }
    }

    public class ResearchPanel : MonoBehaviour
    {
        [SerializeField] private ResearchPanelToggleCanvas _toggleCanvas;
        public ResearchPanelToggleCanvas ResearchPanelToggleCanvas => _toggleCanvas;

        [SerializeField] private Button _closeBtn;

        private Upgrade[] _upgrades;

        public Dictionary<Upgrade, ResearchPanelEntry> ActiveUpgradeEntries { get; private set; }
        public Dictionary<Upgrade, ResearchPanelEntry> CompletedUpgradeEntries { get; private set; }

        public event EventHandler<EventArgs> AutoSaveTriggered = (s, e) => { };
        public UpgradeBtnClick UpgradeBtnClick { get; private set; }

        private bool _isHidden = true;
        public bool IsHidden
        {
            get => _isHidden;

            set
            {
                _isHidden = value;
                Render();
            }
        }

        private CanvasGroup _canvasGroup;
        private GameObject _prefab;
        private Transform _activeUpgrades;
        private Transform _completedUpgrades;

        public void Init(PlayerModel playerModel, UpgradesSO upgradesSO)
        {
            ActiveUpgradeEntries = new Dictionary<Upgrade, ResearchPanelEntry>();
            CompletedUpgradeEntries = new Dictionary<Upgrade, ResearchPanelEntry>();

            UpgradeBtnClick = new UpgradeBtnClick();
            _upgrades = upgradesSO.Upgrades;

            _canvasGroup = GetComponent<CanvasGroup>();
            _prefab = Resources.Load<GameObject>("Prefabs/UI/ResearchPanel/ResearchPanelEntry");
            _activeUpgrades = transform.Find("ScrollView/Viewport/Content").GetComponent<Transform>();
            _completedUpgrades = transform.Find("CompletedUpgradesScrollView/Viewport/Content").GetComponent<Transform>();

            _toggleCanvas.Init();
            _closeBtn.onClick.AddListener(() => IsHidden = true);

            // TODO: move this to controller (?)
            foreach (var upgrade in _upgrades)
            {
                if (upgrade.IsActive)
                {
                    GameObject entryGO = Instantiate(_prefab, _activeUpgrades);

                    ResearchPanelEntry canvasEntry = entryGO.GetComponent<ResearchPanelEntry>();
                    canvasEntry.Init(playerModel, upgrade);
                    canvasEntry.UpgradeBtn.onClick.AddListener(() => { UpgradeBtnClick.Dispatch(new UpgradeBtnClickEventArgs(upgrade)); });
                    upgrade.PropertyChanged += UpgradeChangedHandler;

                    ActiveUpgradeEntries.Add(upgrade, canvasEntry);
                } 
                else
                {
                    GameObject entryGO = Instantiate(_prefab, _completedUpgrades);

                    ResearchPanelEntry canvasEntry = entryGO.GetComponent<ResearchPanelEntry>();
                    canvasEntry.Init(playerModel, upgrade);
                    canvasEntry.UpgradeBtn.onClick.AddListener(() => { UpgradeBtnClick.Dispatch(new UpgradeBtnClickEventArgs(upgrade)); });
                    upgrade.PropertyChanged += UpgradeChangedHandler;

                    CompletedUpgradeEntries.Add(upgrade, canvasEntry);
                }
            }

            Render();
            UpdateView();

            StartCoroutine(AutoSave());
        }

        public void UpdateView()
        {
            if (ActiveUpgradeEntries == null)
            {
                Debug.LogWarning("UpgradeEntries not found... Returning");
                return;
            }
            Upgrade upgradeToMove = null;
            ResearchPanelEntry entryToMove = null;
            foreach (var upgradeEntry in ActiveUpgradeEntries)
            {
                Upgrade upgrade = upgradeEntry.Key;
                ResearchPanelEntry entry = upgradeEntry.Value;

                if (!upgrade.IsActive)
                {
                    upgradeToMove = upgrade;
                    entryToMove = entry;
                }
                else
                {
                    entry.Render(upgrade);
                }
            }

            if (upgradeToMove != null)
            {
                entryToMove.transform.SetParent(_completedUpgrades);
                CompletedUpgradeEntries.Add(upgradeToMove, entryToMove);
                ActiveUpgradeEntries.Remove(upgradeToMove);
            }

            foreach (var upgradeEntry in CompletedUpgradeEntries)
            {
                Upgrade upgrade = upgradeEntry.Key;
                ResearchPanelEntry entry = upgradeEntry.Value;
                entry.Render(upgrade);
            }
        }

        private void Render()
        {
            if (_isHidden)
            {
                Hide();
            }
            else
            {
                Reveal();
            }
        }

        private void Hide()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Reveal()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.blocksRaycasts = true;
        }

        private IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(5.0f);
                AutoSaveTriggered?.Invoke(this, new EventArgs());
            }
        }

        private void UpgradeChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            Upgrade upgrade = (Upgrade)sender;

            ResearchPanelEntry entry;
            ActiveUpgradeEntries.TryGetValue(upgrade, out entry);
            if (entry != null)
            {
                entry.Render(upgrade);
            }
        }
    }
}