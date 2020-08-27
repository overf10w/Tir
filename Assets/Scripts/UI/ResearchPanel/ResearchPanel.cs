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

    // ModelView + View
    // 1. Raises events, to which UpgradesController subscribes
    // 2. A view that handles how upgrades info is shown
    public class ResearchPanel : MonoBehaviour
    {
        [SerializeField] private ResearchPanelToggleCanvas _toggleCanvas;
        public ResearchPanelToggleCanvas ResearchPanelToggleCanvas => _toggleCanvas;

        [SerializeField] private Button _closeBtn;

        public Dictionary<Upgrade, ResearchPanelEntry> UpgradeEntries { get; private set; }

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

        public event EventHandler<EventArgs> AutoSaveTriggered = (s, e) => { };

        public UpgradeBtnClick UpgradeBtnClick { get; private set; }

        private Upgrade[] _upgrades;

        private CanvasGroup _canvasGroup;
        private GameObject _prefab;
        private Transform _content;

        public void Init(PlayerModel playerModel, UpgradesSO upgradesSO)
        {
            UpgradeEntries = new Dictionary<Upgrade, ResearchPanelEntry>();
            UpgradeBtnClick = new UpgradeBtnClick();
            _upgrades = upgradesSO.Upgrades;

            _canvasGroup = GetComponent<CanvasGroup>();
            _prefab = Resources.Load<GameObject>("Prefabs/UI/ResearchPanel/ResearchPanelEntry");
            _content = transform.Find("ScrollView/Viewport/Content").GetComponent<Transform>();

            _toggleCanvas.Init();
            _closeBtn.onClick.AddListener(() => IsHidden = true);

            // TODO: move this to controller (?)
            foreach (var upgrade in _upgrades)
            {
                GameObject entryGO = Instantiate(_prefab, _content);

                ResearchPanelEntry entry = entryGO.GetComponent<ResearchPanelEntry>();
                entry.Init(playerModel, upgrade);
                entry.UpgradeBtn.onClick.AddListener(() => { UpgradeBtnClick.Dispatch(new UpgradeBtnClickEventArgs(upgrade)); });
                upgrade.PropertyChanged += UpgradeChangedHandler;

                UpgradeEntries.Add(upgrade, entry);
            }

            Render();
            UpdateView();

            StartCoroutine(AutoSave());
        }

        public void UpdateView()
        {
            if (UpgradeEntries == null)
            {
                Debug.LogWarning("UpgradeEntries not found... Returning");
                return;
            }
            foreach(var upgradeEntry in UpgradeEntries)
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
            UpgradeEntries.TryGetValue(upgrade, out entry);
            if (entry != null)
            {
                entry.Render(upgrade);
            }
        }
    }
}