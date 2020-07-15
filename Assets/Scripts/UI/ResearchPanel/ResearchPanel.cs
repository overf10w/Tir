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

        private GameObject _researchPanelEntryPrefab;
        private Transform _content;

        public void Init(Upgrades.Upgrade[] upgrades)
        {
            _upgrades = upgrades;

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
    }
}