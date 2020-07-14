using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ResearchPanelEntry : MonoBehaviour
    {
        public Button UpgradeBtn;

        [SerializeField]
        private TextMeshProUGUI name;
        [SerializeField]
        private TextMeshProUGUI description;
        [SerializeField]
        private TextMeshProUGUI price;

        public void Init(Upgrades.Upgrade upgrade)
        {
            upgrade.PropertyChanged += HandleUpgradeModelChanged;

            UpgradeBtn = transform.Find("UpgradeBtn").GetComponent<Button>();

            name.text = upgrade.name.ToString();
            description.text = upgrade.description.ToString();
            price.text = upgrade.price.ToString();

            if (!upgrade.IsActive)
            {
                price.color = Color.red;
                UpgradeBtn.interactable = false;
            }

            //name = transform.Find("NameTxt").GetComponent<TextMeshProUGUI>();
            //description = transform.Find("DescriptionTxt").GetComponent<TextMeshProUGUI>();
            //price = transform.Find("PriceTxt").GetComponent<TextMeshProUGUI>();
        }

        private void HandleUpgradeModelChanged(object sender, PropertyChangedEventArgs args)
        {
            Upgrades.Upgrade upgrade = (Upgrades.Upgrade)sender;
            Debug.Log(
                "ResearchPanelEntry: " +
                "upgrade.isActive: " + upgrade.IsActive
                );
            if (!upgrade.IsActive)
            {
                price.color = Color.red;
                UpgradeBtn.interactable = false;
                Debug.Log("ResearchPanelEntry: fading and disabling inactive upgrade");
            }
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