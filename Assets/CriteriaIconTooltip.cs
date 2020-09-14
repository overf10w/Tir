using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class CriteriaIconTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Reveal tooltip...");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Hide tooltip...");
        }
    }
}