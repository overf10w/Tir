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
        private Upgrade _upgrade;

        public void Init(PlayerModel playerModel, Upgrade upgrade)
        {
            _playerModel = playerModel;
            _upgrade = upgrade;

            upgrade.PropertyChanged += UpgradeChangedHandler;

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
                _price.color = Color.red;
                UpgradeBtn.interactable = false;
            }
            else if (_playerModel.PlayerStats.Gold < _upgrade.Price)
            {
                _price.color = Color.red;
                UpgradeBtn.interactable = false;
            }
            else
            {
                _price.color = Color.green;
                UpgradeBtn.interactable = true;
            }
        }

        private void UpgradeChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            Upgrade upgrade = (Upgrade)sender;
            Render();
        }
    }
}