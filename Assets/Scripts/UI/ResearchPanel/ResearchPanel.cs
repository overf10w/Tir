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
        // TODO: 
        // 1. Populate it with SO: Upgrades.SO data - [done]
        // 2. Subscribe to buttons ResearchPanelEntry.onClick and fire event (add to event chain) - [done]
        // 3. PlayerView.cs: subscribe to ResearchPanel.cs.UpgradeBtnClick and fire event (add to event chain) - [done]
        // 4. PlayerController.cs: subscribe to PlayerView event and fire a Command on PlayerController.model (the Command being parametrized by upgrade event args) - [done]

        public UpgradeBtnClick UpgradeBtnClick { get; set; }

        private Upgrades.Upgrade[] upgrades;

        private GameObject researchPanelEntryPrefab;

        private Transform content;

        public void Init(Upgrades.Upgrade[] upgrades)
        {
            this.upgrades = upgrades;

            UpgradeBtnClick = new UpgradeBtnClick();

            researchPanelEntryPrefab = Resources.Load<GameObject>("Prefabs/UI/ResearchPanel/ResearchPanelEntry");

            content = transform.Find("ScrollView/Viewport/Content").GetComponent<Transform>();

            foreach (var upgrade in this.upgrades)
            {
                GameObject entryGameObject = Instantiate(researchPanelEntryPrefab, content);

                ResearchPanelEntry script = entryGameObject.GetComponent<ResearchPanelEntry>();

                script.Init(upgrade);

                script.UpgradeBtn.onClick.AddListener(() => { UpgradeBtnClick.Dispatch(new UpgradeBtnClickEventArgs(upgrade)); });

                upgrade.PropertyChanged += HandleUpgradeModelChanged;

                Debug.Log("ResearchView: upgrade: " + upgrade.name + ", desc: " + upgrade.description + ", price: " + upgrade.price + ", isActive: " + upgrade.IsActive.ToString());
            }

            StartCoroutine(AutoSave());
        }

        private void HandleUpgradeModelChanged(object sender, PropertyChangedEventArgs args)
        {

            Upgrades.Upgrade upgrade = (Upgrades.Upgrade)sender;

            //var  Array.Find<Upgrades.Upgrade>(upgrades, (el) => el == upgrade);

            Debug.Log(
                "ResearchPanelView: HandleResearchViewUpgradeViewModelChanged: " +
                "upgrade.isActive: " + upgrade.IsActive
                );


            //upgrades.
        }

        private IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(5.0f);

                string upgradesPath = Path.Combine(Application.persistentDataPath, "upgrades.dat");
                ResourceLoader.Save<Upgrades.Upgrade[]>(upgradesPath, this.upgrades);
            }
        }

        public void Show()
        {

        }

        public void Hide()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}