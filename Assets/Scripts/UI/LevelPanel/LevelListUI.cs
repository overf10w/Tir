using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LevelListUI : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        public float step = 0.25f;
        public int levelsCount;

        public void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            levelsCount = transform.GetComponentsInChildren<Button>().Length;
            step = (1.0f / (float)(levelsCount - 1));
        }

        public void Increment()
        {
            _scrollRect.horizontalNormalizedPosition = Mathf.Clamp(_scrollRect.horizontalNormalizedPosition + step, 0, 1);
        }

        public void Decrement()
        {
            _scrollRect.horizontalNormalizedPosition = Mathf.Clamp(_scrollRect.horizontalNormalizedPosition - step, 0, 1);
        }

        public float HorizontalNormalizedPosition()
        {
            return _scrollRect.horizontalNormalizedPosition;
        }

        public void SetLevel(int level)
        {
            _scrollRect.horizontalNormalizedPosition = Mathf.Clamp(step * level, 0, 1);
            Debug.Log("LevelListUI: SetLevel(): Level: " + level);
        }
    }
}