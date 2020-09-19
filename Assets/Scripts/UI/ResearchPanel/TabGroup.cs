using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _pages;

        [SerializeField] private List<TabButton> _tabButtons;

        private TabButton _selectedTab;

        public void OnTabEnter(TabButton tabButton)
        {
            ResetButtons();
            if (_selectedTab == null && tabButton != _selectedTab)
            {
                // tabButton.background.sprite = _tabHoverSprite;
            }
        }

        public void OnTabExit(TabButton tabButton)
        {
            ResetButtons();
        }

        public void OnTabSelected(TabButton tabButton)
        {
            _selectedTab = tabButton;
            ResetButtons();

            int index = _tabButtons.IndexOf(tabButton);
            for (int i = 0; i < _pages.Count; i++)
            {
                if (i == index)
                {
                    // TODO: SetActive causes lag spike, redo using canvasGroup.alpha/interactable/blockRaycasts
                    // https://forum.unity.com/threads/how-to-disable-hide-ui-elements-without-disabling-them.282100/#post-2669210
                    _pages[i].SetActive(true);
                }
                else
                {
                    // TODO: SetActive causes lag spike, redo using canvasGroup.alpha/interactable/blockRaycasts
                    // https://forum.unity.com/threads/how-to-disable-hide-ui-elements-without-disabling-them.282100/#post-2669210                    
                    _pages[i].SetActive(false);
                }
            }
        }

        private void ResetButtons()
        {
            foreach(var button in _tabButtons)
            {
                if (_selectedTab != null && button == _selectedTab)
                {
                    continue;
                }
                // Reset sprite of each button
            }
        }
    }
}