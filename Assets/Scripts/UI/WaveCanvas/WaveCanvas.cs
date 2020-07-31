using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class WaveCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveHPText;
        public TextMeshProUGUI WaveHPText => _waveHPText;
    }
}