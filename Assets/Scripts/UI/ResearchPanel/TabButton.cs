using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Game
{
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private TabGroup _tabGroup;

        //public void Awake()
        //{
        //    _tabGroup.Subscribe(this);
        //}

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("TabButton: Hi!");
            if (_tabGroup)
            {
                _tabGroup.OnTabEnter(this);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_tabGroup)
            {
                _tabGroup.OnTabSelected(this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_tabGroup)
            {
                _tabGroup.OnTabExit(this);
            }
            Debug.Log("TabButton: Bye!");
        }
    }
}