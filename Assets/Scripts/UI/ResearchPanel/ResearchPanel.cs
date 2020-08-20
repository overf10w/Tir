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

        public UpgradeBtnClick UpgradeBtnClick { get; private set; }

        private UpgradesSO _upgradesSO;
        private Upgrade[] _upgrades;

        private CanvasGroup _canvasGroup;
        private GameObject _prefab;
        private Transform _content;

        private PlayerModel _playerModel;

        public void Init(PlayerModel playerModel, UpgradesSO upgradesSO)
        {
            UpgradeEntries = new Dictionary<Upgrade, ResearchPanelEntry>();

            _playerModel = playerModel;
            _upgradesSO = upgradesSO;
            _upgrades = _upgradesSO.Upgrades;
            
            _canvasGroup = GetComponent<CanvasGroup>();
            _prefab = Resources.Load<GameObject>("Prefabs/UI/ResearchPanel/ResearchPanelEntry");
            _content = transform.Find("ScrollView/Viewport/Content").GetComponent<Transform>();

            UpgradeBtnClick = new UpgradeBtnClick();

            _toggleCanvas.Init();

            _closeBtn.onClick.AddListener(() => { IsHidden = true; Debug.Log("closeBtn.OnClick Handler"); });

            foreach (var upgrade in _upgrades)
            {
                GameObject entryGO = Instantiate(_prefab, _content);

                ResearchPanelEntry entry = entryGO.GetComponent<ResearchPanelEntry>();
                entry.Init(playerModel, upgrade);
                entry.UpgradeBtn.onClick.AddListener(() => { UpgradeBtnClick.Dispatch(new UpgradeBtnClickEventArgs(upgrade)); });

                UpgradeEntries.Add(upgrade, entry);
            }

            Render();

            StartCoroutine(AutoSave());
        }

        public void UpdateView()
        {
            if (UpgradeEntries == null)
            {
                Debug.LogWarning("UpgradeEntries not found... Returning");
                return;
            }
            foreach(var upgrade in UpgradeEntries)
            {
                Upgrade upgrd = upgrade.Key;
                ResearchPanelEntry entry = upgrade.Value;
                if (!upgrd.IsActive)
                {
                    entry.ShowInactive(0.99f);
                    //entry.gameObject.SetActive(false);
                }
                else if (_playerModel.PlayerStats.Gold < upgrd.Price / 10.0f)
                {
                    entry.ShowInactive(0.75f);
                }
                else if (_playerModel.PlayerStats.Gold < upgrd.Price)
                {
                    entry.gameObject.SetActive(true);
                    entry.ShowInactive(0.3f);
                }
                else
                {
                    entry.gameObject.SetActive(true);
                    entry.ShowActive();
                }
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
            }
        }
    }
}