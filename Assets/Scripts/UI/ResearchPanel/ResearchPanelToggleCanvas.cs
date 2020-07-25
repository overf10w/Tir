using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ToggleBtnClick
    {
        public PlayerView PlayerView;

        public void Dispatch()
        {
            if (PlayerView != null)
            {
                PlayerView.HandleResearchPanelToggleBtnClick();
            }
        }
    }

    public class ResearchPanelToggleCanvas : MonoBehaviour
    {
        [SerializeField] private Button _researchPanelToggleBtn;

        public ToggleBtnClick ToggleBtnClick { get; private set; }

        public void Init()
        {
            ToggleBtnClick = new ToggleBtnClick();
            _researchPanelToggleBtn.onClick.AddListener(() => { ToggleBtnClick.Dispatch(); });
        }
    }
}