using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnityEngine;

public class Wave : MessageHandler
{
    [HideInInspector]
    public int cubesNumber;
    public int index;

    private List<IDestroyable> cubesList;

    void Awake()
    {
        cubesList = new List<IDestroyable>();
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            cubesList.Add(transform.GetChild(i).GetComponent<IDestroyable>());
            cubesNumber++;
        }
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            Cube cube = (Cube)message.objectValue;
            cubesList.Remove(cube);
        }
    }

    public List<IDestroyable> Cubes
    {
        get { return cubesList; }
    }
}
