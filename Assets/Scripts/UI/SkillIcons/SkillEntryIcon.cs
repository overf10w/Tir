using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SkillEntryIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        public Image Icon => _icon;

        [SerializeField] private TextMeshProUGUI _valueTxt;
        public TextMeshProUGUI ValueTxt => _valueTxt;
    }
}