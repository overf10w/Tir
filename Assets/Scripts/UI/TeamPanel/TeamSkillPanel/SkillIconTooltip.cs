using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class SkillIconTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameTxt;
        [SerializeField] private TextMeshProUGUI _valueText;

        public void Init(PlayerStat skill)
        {
            _nameTxt.text = skill.Name.ToString();
            _valueText.text = skill.Value.ToString();
        }
    }
}