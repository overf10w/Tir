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
            InitMessageHandler();

            cubesList = new List<IDestroyable>();
            SpawnCubes();
        }

        private void SpawnCubes()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var spawnTransform = transform.GetChild(i);
                var prefab = Resources.Load<Cube>("Prefabs/Cube") as Cube;
                var cube = Instantiate(prefab, spawnTransform.transform) as Cube;
                cube.Init();
                new CubeController(cube);
                cube.transform.SetParent(this.gameObject.transform);
                cubesList.Add(cube.GetComponent<IDestroyable>());

                Destroy(spawnTransform.gameObject);

                cubesNumber++;
            }
        }

        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.CubeDeath };
            MessageBus.Instance.AddSubscriber(msc);
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