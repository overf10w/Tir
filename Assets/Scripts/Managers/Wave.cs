﻿using System;
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
        #region MessageHandler
        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.CUBE_DEATH };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.CUBE_DEATH)
            {
                Cube cube = (Cube)message.objectValue;
                Cubes.Remove(cube);
            }
        }
        #endregion

        [SerializeField] private int _cubesNumber;
        public int CubesNumber => _cubesNumber;

        [SerializeField] private Transform _spawnGrid;

        public List<IDestroyable> Cubes { get; private set; }

        public float GlobalHP { get; set; }

        public void Init(float globalHP)
        {
            GlobalHP = globalHP;

            InitMessageHandler();

            Cubes = new List<IDestroyable>();
            SpawnCubes();
        }

        private void SpawnCubes()
        {
            float cubesCnt = _spawnGrid.childCount;

            //float cubeHP = GlobalHP

            for (int i = _spawnGrid.childCount - 1; i >= 0; i--)
            {
                var spawnTransform = _spawnGrid.GetChild(i);
                var prefab = Resources.Load<Cube>("Prefabs/Cube") as Cube;
                //float scaleMultiplier = prefab.transform
                var cube = Instantiate(prefab, spawnTransform.transform) as Cube;
                cube.Init(GlobalHP / cubesCnt);
                new CubeController(cube);
                cube.transform.SetParent(this.gameObject.transform);
                Cubes.Add(cube.GetComponent<IDestroyable>());

                Destroy(spawnTransform.gameObject);

                _cubesNumber++;
            }
        }
    }
}