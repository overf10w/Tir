using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelListUI : MonoBehaviour
{
    private ScrollRect scrollRect;
    public float step = 0.25f;
    public int levelsCount;

    public void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        levelsCount = transform.GetComponentsInChildren<Button>().Length;
        step = (1.0f / (float)(levelsCount - 1));
    }

    public void Increment()
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition + step, 0, 1);
    }

    public void Decrement()
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition - step, 0, 1);
    }

    public float HorizontalNormalizedPosition()
    {
        return scrollRect.horizontalNormalizedPosition;
    }

    public void SetLevel(int level)
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(step * level, 0, 1);
        Debug.Log("LevelListUI: SetLevel(): Level: " + level);
    }
}
