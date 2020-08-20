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

        // TODO: do we really need this method here?
        public void Render()
        {
            if (!_upgrade.IsActive)
            {
                ShowInactive(0.99f);
            }
            else if (_playerModel.PlayerStats.Gold < _upgrade.Price / 10.0f)
            {
                ShowInactive(0.75f);
            }
            else if (_playerModel.PlayerStats.Gold < _upgrade.Price)
            {
                ShowInactive(0.3f);
            }
            else
            {
                ShowActive();
            }
        }

        // TODO: make this method easier to understand
        public void ShowInactive(float normalizedStrength)
        {
            float strength = Mathf.Clamp01(normalizedStrength);

            _price.color = Color.red;
            UpgradeBtn.interactable = false;
            _maskImage.color = new Color(1, 1, 1, strength);
        }

        public void ShowActive()
        {
            _price.color = Color.green;
            UpgradeBtn.interactable = true;
            _maskImage.color = new Color(1, 1, 1, 0.0f);
        }

        private void UpgradeChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            Upgrade upgrade = (Upgrade)sender;
            Render();
        }
    }
}