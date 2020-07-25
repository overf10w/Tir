using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WaveSpawner : MessageHandler
    {
        #region MessageHandler
        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.CUBE_DEATH, MessageType.LEVEL_PASSED };
            MessageBus.Instance.AddSubscriber(msc);
        }

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.CUBE_DEATH)
            {
                _cubesDestroyed++;
                if (_cubesDestroyed == _cubesSpawned)
                {
                    MessageBus.Instance.SendMessage(new Message { Type = MessageType.LEVEL_PASSED });
                    Destroy(_wave.gameObject);
                    SpawnWave();
                }
            }
        }
        #endregion

        [SerializeField] private WaveSpawnerAlgorithm _algorithm;
        [SerializeField] private PlayerWaves _playerWaves;

        private PlayerStats _playerStats;

        private Wave _wave;

        private int _cubesSpawned;
        private int _cubesDestroyed;

        public void Init(PlayerStats playerStats)
        {
            InitMessageHandler();
            _playerStats = playerStats;
            SpawnWave();
        }

        private void SpawnWave()
        {
            var waves = _playerWaves.waves;
            var wavePrefab = waves.PickRandom();

            _wave = Instantiate(wavePrefab, wavePrefab.transform.position, Quaternion.identity) as Wave;

            //_wave.Init((_playerStats.Level + 1) * 10);
            _wave.Init(_algorithm.GetWaveHp(_playerStats.Level));

            _cubesSpawned = _wave.CubesNumber;
            _cubesDestroyed = 0;
            MessageBus.Instance.SendMessage(new Message() { Type = MessageType.WAVE_CHANGED, objectValue = _wave });
        }
    }
}