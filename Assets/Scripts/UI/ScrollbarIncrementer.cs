using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarIncrementer : MonoBehaviour
{
    public ScrollRect levelsHolder;
    public Button theOtherButton;
    public float step = 0.25f;
    public int levelsCount;

    public void Start()
    {
        levelsCount = levelsHolder.transform.GetComponentsInChildren<Button>().Length;
        step = (1.0f / (float) (levelsCount - 1));
    }

    public void Increment()
    {
        if (levelsHolder == null || theOtherButton == null) throw new Exception("Setup ScrollbarIncrementer first!");
        levelsHolder.horizontalNormalizedPosition = Mathf.Clamp(levelsHolder.horizontalNormalizedPosition + step, 0, 1);
        GetComponent<Button>().interactable = levelsHolder.horizontalNormalizedPosition != 1;
        theOtherButton.interactable = true;
    }

    public void Decrement()
    {
        if (levelsHolder == null || theOtherButton == null) throw new Exception("Setup ScrollbarIncrementer first!");
        levelsHolder.horizontalNormalizedPosition = Mathf.Clamp(levelsHolder.horizontalNormalizedPosition - step, 0, 1);
        GetComponent<Button>().interactable = levelsHolder.horizontalNormalizedPosition != 0; ;
        theOtherButton.interactable = true;
    }
}