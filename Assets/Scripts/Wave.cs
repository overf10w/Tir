using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Wave : MessageHandler
{
    [HideInInspector]
    public int cubesNumber;
    public int index;

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

    public override void HandleMessage(Message message) { }
}
