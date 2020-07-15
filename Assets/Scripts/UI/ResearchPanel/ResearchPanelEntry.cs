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
        public Button UpgradeBtn { get; private set; }

        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _price;

        public void Init(Upgrades.Upgrade upgrade)
        {
            upgrade.PropertyChanged += HandleUpgradeModelChanged;

            UpgradeBtn = transform.Find("UpgradeBtn").GetComponent<Button>();

            _name.text = upgrade.Name.ToString();
            _description.text = upgrade.Description.ToString();
            _price.text = upgrade.Price.ToString();

            if (!upgrade.IsActive)
            {
                _price.color = Color.red;
                UpgradeBtn.interactable = false;
            }
        }

        private void HandleUpgradeModelChanged(object sender, PropertyChangedEventArgs args)
        {
            Upgrades.Upgrade upgrade = (Upgrades.Upgrade)sender;
            if (!upgrade.IsActive)
            {
                _price.color = Color.red;
                UpgradeBtn.interactable = false;
            }
        }
    }
}