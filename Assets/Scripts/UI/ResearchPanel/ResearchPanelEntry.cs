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
        [SerializeField] private Image _image;
        [SerializeField] private Image _maskImage;


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
            _image.sprite = upgrade.Icon;

            Render();
        }

        public void Render()
        {
            if (!_upgrade.IsActive)
            {
                _price.color = Color.red;
                UpgradeBtn.interactable = false;
                _maskImage.color = new Color(1, 1, 1, 0.8f);
                gameObject.SetActive(false);
            }
            else if (_playerModel.PlayerStats.Gold < _upgrade.Price)
            {
                _price.color = Color.red;
                UpgradeBtn.interactable = false;
                _maskImage.color = new Color(1, 1, 1, 0.8f);
            }
            else
            {
                _price.color = Color.green;
                UpgradeBtn.interactable = true;
                _maskImage.color = new Color(1, 1, 1, 0.0f);
            }
        }

        private void UpgradeChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            Upgrade upgrade = (Upgrade)sender;
            Render();
        }
    }
}