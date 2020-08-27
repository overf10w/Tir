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

        public void Init(PlayerModel playerModel, Upgrade upgrade)
        {
            _playerModel = playerModel;

            UpgradeBtn = GetComponent<Button>();

            _name.text = upgrade.Name.ToString();
            _description.text = upgrade.Description.ToString();
            _price.text = upgrade.Price.SciFormat();
            _image.sprite = upgrade.Icon;
        }

        public void Render(Upgrade upgrade)
        {
            if (!upgrade.IsActive)
            {
                ShowInactive(99f);
            }
            else if (_playerModel.PlayerStats.Gold < upgrade.Price / 10.0f)
            {
                ShowInactive(75f);
            }
            else if (_playerModel.PlayerStats.Gold < upgrade.Price)
            {
                gameObject.SetActive(true);
                ShowInactive(30f);
            }
            else
            {
                gameObject.SetActive(true);
                ShowActive();
            }
        }

        public void ShowInactive(float shadeStrength)
        {
            float maxShade = 100.0f;
            float normalizedStrength = Mathf.Clamp01(shadeStrength / maxShade);

            _price.color = Color.red;
            UpgradeBtn.interactable = false;
            _maskImage.color = new Color(1, 1, 1, normalizedStrength);
        }

        public void ShowActive()
        {
            _price.color = Color.green;
            UpgradeBtn.interactable = true;
            _maskImage.color = new Color(1, 1, 1, 0.0f);
        }
    }
}