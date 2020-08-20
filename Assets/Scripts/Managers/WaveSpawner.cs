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
                    StartCoroutine(SpawnWave());
                }
            }
        }
        #endregion

        [SerializeField] private WaveSpawnerAlgorithm _algorithm;
        [SerializeField] private PlayerWaves _playerWaves;
        [SerializeField] private WaveCanvas _waveCanvas;
        [SerializeField] private Transform _waveSpawnPoint;

        private PlayerStats _playerStats;
        private Wave _wave;

        private int _cubesSpawned;
        private int _cubesDestroyed;

        public void Init(PlayerStats playerStats)
        {
            InitMessageHandler();
            _playerStats = playerStats;
            StartCoroutine(SpawnWave());
        }

        private IEnumerator SpawnWave()
        {
            yield return new WaitForSeconds(0.5f);

            var waves = _playerWaves.Waves;
            var wavePrefab = waves.PickRandom();

            _wave = Instantiate(wavePrefab, wavePrefab.transform.position, Quaternion.identity) as Wave;

            float waveHP = _algorithm.GetWaveHp(_playerStats.Level);
            float waveGold = _algorithm.GetWaveGold(_playerStats.Level);
            _wave.Init(waveHP, waveGold, _waveSpawnPoint);
            _wave.WaveHpChanged += WaveHpChangedHandler;

            _cubesSpawned = _wave.CubesNumber;
            _cubesDestroyed = 0;

            MessageBus.Instance.SendMessage(new Message() { Type = MessageType.WAVE_CHANGED, objectValue = _wave });

            //_waveCanvas.WaveHPText.text = waveHP.SciFormat().ToString();
            _waveCanvas.Render(waveHP);
        }

        private void WaveHpChangedHandler(object sender, EventArgs<float> hp)
        {
            //_waveCanvas.WaveHPText.text = hp.Val.SciFormat().ToString();
            _waveCanvas.Render(hp.Val);
        }
    }
}