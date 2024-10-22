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
        // TODO (LP): rename this to _ancher
        [SerializeField] private Transform _center;
        [SerializeField] private float _cubeScaleMultiplier = 1.9f;

        public List<ICube> Cubes { get; private set; }

        public float WaveHP { get; private set; }
        public float WaveGold { get; private set; }
        public EventHandler<EventArgs<float>> WaveHpChanged { get; set; } = (s, e) => { };

        public void Init(float waveHp, float waveGold, Transform waveSpawnPoint)
        {
            WaveHP = waveHp;
            WaveGold = waveGold;

            InitMessageHandler();

            Cubes = new List<ICube>();
            SpawnCubes(waveSpawnPoint);
        }

        private void SpawnCubes(Transform waveSpawnPoint)
        {
            Vector3 centerLocalPos = _center.localPosition;
            Vector3 cubeScale = new Vector3(_cubeScaleMultiplier, _cubeScaleMultiplier, _cubeScaleMultiplier);
            Vector3 gridScale = _spawnGrid.localScale;

            Vector3 scaleMult = new Vector3(cubeScale.x * gridScale.x, cubeScale.y * gridScale.y, cubeScale.z * gridScale.z);

            Vector3 wavePosition = new Vector3(waveSpawnPoint.position.x - (centerLocalPos.x * scaleMult.x), waveSpawnPoint.position.y - (centerLocalPos.y * scaleMult.y), waveSpawnPoint.position.z - (centerLocalPos.z * scaleMult.z));

            _spawnGrid.position = wavePosition;
            _spawnGrid.localScale = scaleMult;

            _center.position = new Vector3(wavePosition.x + (centerLocalPos.x * scaleMult.x), wavePosition.y + (centerLocalPos.y * scaleMult.y), wavePosition.z + (centerLocalPos.z * scaleMult.z));

            float cubesCnt = _spawnGrid.childCount;

            for (int i = _spawnGrid.childCount - 1; i >= 0; i--)
            {
                var spawnTransform = _spawnGrid.GetChild(i);
                var prefab = Resources.Load<Cube>("Prefabs/Cube") as Cube;

                var cube = Instantiate(prefab) as Cube;
                cube.transform.position = spawnTransform.position;
                cube.transform.localScale = cubeScale;
                cube.transform.parent = spawnTransform;
                cube.transform.rotation = spawnTransform.rotation;

                Color cubecolor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                CubeSpawnColor cubeSpawnColor = spawnTransform.GetComponent<CubeSpawnColor>();
                if (cubeSpawnColor != null)
                {
                    cubecolor = cubeSpawnColor.Color;
                }

                cube.Init(WaveHP / cubesCnt, WaveGold / cubesCnt, cubecolor);
                new CubeController(cube);
                cube.HpChanged += CubeTakeDamageHandler;
                Cubes.Add(cube.GetComponent<ICube>());

                _cubesNumber++;
            }
        }

        private void CubeTakeDamageHandler(object sender, CubeHpChangeEventArgs e)
        {
            WaveHP -= e.Diff;
            WaveHpChanged?.Invoke(this, new EventArgs<float>(WaveHP));
        }
    }
}