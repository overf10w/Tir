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

    public class ResearchPanel : MonoBehaviour
    {
        public UpgradeBtnClick UpgradeBtnClick { get; private set; }

        private Upgrades.Upgrade[] _upgrades;

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

        public void Init(Upgrades.Upgrade[] upgrades)
        {
            _upgrades = upgrades;

            _canvasGroup = GetComponent<CanvasGroup>();

            UpgradeBtnClick = new UpgradeBtnClick();

            _researchPanelEntryPrefab = Resources.Load<GameObject>("Prefabs/UI/ResearchPanel/ResearchPanelEntry");

            _content = transform.Find("ScrollView/Viewport/Content").GetComponent<Transform>();

            foreach (var upgrade in _upgrades)
            {
                GameObject entryGameObject = Instantiate(_researchPanelEntryPrefab, _content);

                ResearchPanelEntry script = entryGameObject.GetComponent<ResearchPanelEntry>();
                script.Init(upgrade);
                script.UpgradeBtn.onClick.AddListener(() => { UpgradeBtnClick.Dispatch(new UpgradeBtnClickEventArgs(upgrade)); });

                upgrade.PropertyChanged += HandleUpgradeModelChanged;
            }
            StartCoroutine(AutoSave());
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