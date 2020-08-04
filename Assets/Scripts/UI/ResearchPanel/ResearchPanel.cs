using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

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

        public List<ResearchPanelEntry> ResearchPanelEntries { get; private set; }

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

        public UpgradeBtnClick UpgradeBtnClick { get; private set; }

        private UpgradesSO _upgradesSO;
        private Upgrade[] _upgrades;

        private CanvasGroup _canvasGroup;
        private GameObject _prefab;
        private Transform _content;

        public void Init(PlayerModel playerModel, UpgradesSO upgradesSO)
        {
            ResearchPanelEntries = new List<ResearchPanelEntry>();
            _upgradesSO = upgradesSO;
            _upgrades = _upgradesSO.Upgrades;
            UpgradeBtnClick = new UpgradeBtnClick();

            _toggleCanvas.Init();

            _canvasGroup = GetComponent<CanvasGroup>();
            _prefab = Resources.Load<GameObject>("Prefabs/UI/ResearchPanel/ResearchPanelEntry");
            _content = transform.Find("ScrollView/Viewport/Content").GetComponent<Transform>();

            foreach (var upgrade in _upgrades)
            {
                GameObject entry = Instantiate(_prefab, _content);

                ResearchPanelEntry script = entry.GetComponent<ResearchPanelEntry>();
                script.Init(playerModel, upgrade);
                script.UpgradeBtn.onClick.AddListener(() => { UpgradeBtnClick.Dispatch(new UpgradeBtnClickEventArgs(upgrade)); });

                ResearchPanelEntries.Add(script);
            }

            Render();

            StartCoroutine(AutoSave());
        }

        public void UpdateView()
        {
            foreach (var script in ResearchPanelEntries)
            {
                script.Render();
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

        private IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(5.0f);

                // TODO: trigger 'save' event here

                // TODO: move the logic below to controller
                UpgradeData[] upgradesData = _upgradesSO.GetUpgradesData();
                string _upgradesSavePath = Path.Combine(Application.persistentDataPath, "upgradesSave.dat");
                ResourceLoader.Save<UpgradeData[]>(_upgradesSavePath, upgradesData);

                string upgradesPath = Path.Combine(Application.persistentDataPath, "upgrades.dat");
                ResourceLoader.Save<Upgrade[]>(upgradesPath, _upgrades);
            }
        }
    }
}