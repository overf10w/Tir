using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
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
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var oldCube = transform.GetChild(i);
                var newCube = Resources.Load<Cube>("Prefabs/Cube") as Cube;
                var kek = Instantiate(newCube, oldCube.transform) as Cube;
                kek.transform.SetParent(this.gameObject.transform);
                Destroy(oldCube.gameObject);
                cubesList.Add(kek.GetComponent<IDestroyable>());

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
}