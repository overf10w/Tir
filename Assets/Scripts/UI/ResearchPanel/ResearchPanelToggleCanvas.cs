using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class ResearchPanelToggleCanvas : MonoBehaviour
    {
        [SerializeField] private Button _researchPanelToggleBtn;

        public void Awake()
        {
            _researchPanelToggleBtn.onClick.AddListener(() => { Debug.Log("ResearchPanelToggleCanvas: ToggleBtn Pressed"); });
        }
    }
}