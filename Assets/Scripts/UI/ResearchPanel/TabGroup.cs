using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TabGroup : MonoBehaviour
    {
        // The index of each tab page must correspond the one of a button, which is intented to open the page
        [SerializeField] private List<CanvasGroup> _pages;
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
                    _pages[i].alpha = 1;
                    _pages[i].interactable = true;
                    _pages[i].blocksRaycasts = true;
                }
                else
                {
                    _pages[i].alpha = 0;
                    _pages[i].interactable = false;
                    _pages[i].blocksRaycasts = false;
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