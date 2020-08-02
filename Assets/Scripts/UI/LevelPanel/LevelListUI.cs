using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LevelListUI : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        private float _step = 0.25f;
        private int _levelsCount;

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _levelsCount = transform.GetComponentsInChildren<Button>().Length;
            _step = (1.0f / (float)(_levelsCount - 1));
        }

        public void Increment()
        {
            _scrollRect.horizontalNormalizedPosition = Mathf.Clamp(_scrollRect.horizontalNormalizedPosition + _step, 0, 1);
        }

        public void Decrement()
        {
            _scrollRect.horizontalNormalizedPosition = Mathf.Clamp(_scrollRect.horizontalNormalizedPosition - _step, 0, 1);
        }

        public float HorizontalNormalizedPosition()
        {
            return _scrollRect.horizontalNormalizedPosition;
        }

        public void SetLevel(int level)
        {
            _scrollRect.horizontalNormalizedPosition = Mathf.Clamp(_step * level, 0, 1);
        }
    }
}