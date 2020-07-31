using System;
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

        [field: NonSerialized]
        private PlayerModel _playerModel;

        [field: NonSerialized]
        private Upgrades.Upgrade _upgrade;

        public void Init(PlayerModel playerModel, Upgrades.Upgrade upgrade)
        {
            _playerModel = playerModel;
            _upgrade = upgrade;

            upgrade.PropertyChanged += HandleUpgradeModelChanged;

            UpgradeBtn = GetComponent<Button>();

            _name.text = upgrade.Name.ToString();
            _description.text = upgrade.Description.ToString();
            _price.text = upgrade.Price.SciFormat();

            Render();
        }

        public void Render()
        {
            if (!_upgrade.IsActive)
            {
                Debug.Log("THAT'S THA CASE(1)");
                _price.color = Color.red;
                UpgradeBtn.interactable = false;
            }
            else if (_playerModel.PlayerStats.Gold < _upgrade.Price)
            {
                Debug.Log("THAT'S THA CASE(2)");
                _price.color = Color.red;
                UpgradeBtn.interactable = false;
            }
            else
            {
                Debug.Log("THAT'S THA CASE(3)");
                _price.color = Color.green;
                UpgradeBtn.interactable = true;
            }
        }

        private void HandleUpgradeModelChanged(object sender, PropertyChangedEventArgs args)
        {
            Upgrades.Upgrade upgrade = (Upgrades.Upgrade)sender;
            // RENDER() METHOD
            Render();
            // ENDOF RENDER() METHOD
        }

        public void Render(PlayerModel model, Upgrades.Upgrade upgrade)
        {
            Debug.Log("WeaponPanelEntry: Render()");

            Color green;
            Color red;

            ColorUtility.TryParseHtmlString("#CCFFC8", out green);
            ColorUtility.TryParseHtmlString("#FF807C", out red);

            if (model.PlayerStats.Gold >= upgrade.Price)
            {
                _price.color = Color.green;
                UpgradeBtn.interactable = true;
            }
            else
            {
                _price.color = Color.red;
                UpgradeBtn.interactable = false;
            }

            _price.color = Color.red;
            UpgradeBtn.interactable = false;
        }

    }
}