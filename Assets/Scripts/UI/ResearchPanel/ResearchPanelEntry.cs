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

        [SerializeField] private Transform _content;

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


            InitCriteriaSlots(upgrade);
        }

        private void InitCriteriaSlots(Upgrade upgrade)
        {
            int childInd = 0;
            for (int i = 0; i < upgrade.Criterias.Length; i++)
            {
                var crit = upgrade.Criterias[i];
                //GameObject criteriaIcon = new GameObject("Icon");
                //Image img = criteriaIcon.AddComponent<Image>();
                //img.sprite = crit.TargetStat.Icon;
                //criteriaIcon.transform.parent = _content;

                Transform iconTransform = _content.GetChild(childInd++);

                Image icon = iconTransform.GetComponent<Image>();
                icon.sprite = crit.TargetStat.Icon;
                Color color = icon.color;

                if (!crit.Satisfied)
                {
                    color.a = 0.2f;
                }
                
                icon.color = color;

                //icon.color.a = 0.2f;
            }
        }

        public void Render(Upgrade upgrade)
        {
            ShowCriterias(upgrade);

            if (!upgrade.IsActive)
            {
                Fade0100(99f);
            }
            else if (_playerModel.PlayerStats.Gold < upgrade.Price / 10.0f)
            {
                Fade0100(75f);
            }
            else if (_playerModel.PlayerStats.Gold < upgrade.Price || !upgrade.CriteriasFulfilled)
            {
                gameObject.SetActive(true);
                Fade0100(30f);
            }
            else
            {
                gameObject.SetActive(true);
                ShowActive();
            }
        }

        private void ShowCriterias(Upgrade upgrade)
        {
            int childInd = 0;
            for (int i = 0; i < upgrade.Criterias.Length; i++)
            {
                var crit = upgrade.Criterias[i];
                Transform iconTransform = _content.GetChild(childInd++);

                Image icon = iconTransform.GetComponent<Image>();
                //icon.sprite = crit.TargetStat.Icon;
                Color color = icon.color;

                if (!crit.Satisfied)
                {
                    color.a = 0.2f;
                } 
                else
                {
                    color.a = 1.0f;
                }

                icon.color = color;

                //icon.color.a = 0.2f;
            }
        }

        public void Fade0100(float shadeStrength)
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