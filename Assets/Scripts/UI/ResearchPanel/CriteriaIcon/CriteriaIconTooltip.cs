using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class CriteriaIconTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statText;
        public TextMeshProUGUI StatText => _statText;


        [SerializeField] private TextMeshProUGUI _statsListText;
        public TextMeshProUGUI StatsListText => _statsListText;


        [SerializeField] private TextMeshProUGUI _valueText;
        public TextMeshProUGUI ValueText => _valueText;

    }
}