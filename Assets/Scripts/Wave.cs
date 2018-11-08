using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [HideInInspector]
    public int cubesNumber;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            cubesNumber++;
        }
    }
}

