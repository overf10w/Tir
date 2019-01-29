using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarIncrementer : MonoBehaviour
{
    public ScrollRect levelList;
    private LevelListUI levelListScript;
    public Button theOtherButton;

    public void Start()
    {
        levelListScript = levelList.GetComponent<LevelListUI>();
    }

    public void Increment()
    {
        if (levelListScript == null || theOtherButton == null) throw new Exception("Setup ScrollbarIncrementer first!");
        levelListScript.Increment();
        GetComponent<Button>().interactable = levelListScript.HorizontalNormalizedPosition() <= 0.999f;
        theOtherButton.interactable = true;
    }

    public void Decrement()
    {
        if (levelListScript == null || theOtherButton == null) throw new Exception("Setup ScrollbarIncrementer first!");
        levelListScript.Decrement();    
        GetComponent<Button>().interactable = levelListScript.HorizontalNormalizedPosition() >= 0.001f;
        theOtherButton.interactable = true;
    }
}