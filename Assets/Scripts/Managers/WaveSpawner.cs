using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Game
{
    public class WaveSpawner : MessageHandler, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region MessageHandler
        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.CUBE_DEATH, MessageType.LEVEL_COMPLETE, MessageType.LEVEL_RESTARTED };
            MessageBus.Instance.AddSubscriber(msc);
        }

        private float _bossTimer = 0.0f;
        private float _bossTimerThreshold = 30.0f;

        public override void HandleMessage(Message message)
        {
            if (message.Type == MessageType.CUBE_DEATH)
            {
                _cubesDestroyed++;
                if (_cubesDestroyed == _cubesSpawned)
                {
                    if (WaveInd == 5)
                    {
                        if (_bossTimer <= _bossTimerThreshold)
                        {
                            MessageBus.Instance.SendMessage(new Message { Type = MessageType.LEVEL_COMPLETE });
                            _particleSpawner.ShowEpic(3.0f);
                            StopCoroutine(_bossTimerRoutine);
                            _bossTimer = 0.0f;
                            WaveInd = 1;
                        }
                        else
                        {
                            return;
                        }
                    }
                    Destroy(_wave.gameObject);
                    WaveInd++;
                    StartCoroutine(SpawnWave());
                }
            }
            else if (message.Type == MessageType.LEVEL_RESTARTED)
            {
                WaveInd = 1;
                if (_wave)
                {
                    Destroy(_wave.gameObject);
                }
                StartCoroutine(SpawnWave());
            }
        }
        #endregion

        [SerializeField] private WaveSpawnerAlgorithm _algorithm;
        [SerializeField] private PlayerWaves _playerWaves;
        [SerializeField] private WaveCanvas _waveCanvas;
        [SerializeField] private Transform _waveSpawnPoint;
        [SerializeField] private WaveParticleSpawner _particleSpawner;

        private Queue<Wave> _wavesToSpawn;

        private PlayerStats _playerStats;
        private Wave _wave;

        private int _cubesSpawned;
        private int _cubesDestroyed;

        private int _waveInd = 1;
        public int WaveInd { get => _waveInd; set { SetField(ref _waveInd, value); } }

        private IEnumerator _bossTimerRoutine;

        private event EventHandler<EventArgs> BossNotBeatenInTime = (s, e) => { };

        public void Init(PlayerStats playerStats)
        {
            InitMessageHandler();
            _playerStats = playerStats;
            _wavesToSpawn = new Queue<Wave>(_playerWaves.Waves.Shuffle());
            StartCoroutine(SpawnWave());

            BossNotBeatenInTime += WaveSpawner_BossNotBeatenInTime;
        }

        private void WaveSpawner_BossNotBeatenInTime(object sender, EventArgs e)
        {
            if (_wave)
            {
                Destroy(_wave.gameObject);
            }
            StartCoroutine(SpawnWave());
        }

        private bool _waveFlag = false;
        private IEnumerator SpawnWave()
        {
            if (_waveFlag)
            {
                yield break;
            }

            _waveFlag = true;
            yield return new WaitForSeconds(0.5f);

            var waves = _playerWaves.Waves;

            Wave wavePrefab;

            // TODO: more elegant solution needed (?)
            if (_wavesToSpawn.Count > 0)
            {
                wavePrefab = _wavesToSpawn.Dequeue();
            }
            else
            {
                _wavesToSpawn = new Queue<Wave>(_playerWaves.Waves.Shuffle());
                wavePrefab = _wavesToSpawn.Dequeue();
            }

            _wave = Instantiate(wavePrefab, wavePrefab.transform.position, Quaternion.identity) as Wave;

            float waveHP = _algorithm.GetWaveHp(_playerStats.Level);
            float waveGold = _algorithm.GetWaveGold(_playerStats.Level);

            if (WaveInd % 5 != 0)
            {
                _wave.Init(waveHP / 10.0f, waveGold / 5.0f, _waveSpawnPoint);
            } 
            else
            {
                //WaveInd = 0;
                _bossTimerRoutine = StartBossTimer();
                StartCoroutine(_bossTimerRoutine);
                _wave.Init(waveHP, waveGold, _waveSpawnPoint);
            }

            _wave.WaveHpChanged += WaveHpChangedHandler;

            _cubesSpawned = _wave.CubesNumber;
            _cubesDestroyed = 0;

            MessageBus.Instance.SendMessage(new Message() { Type = MessageType.WAVE_CHANGED, objectValue = _wave });
            _particleSpawner.Show(0.5f);

            _waveCanvas.Render(waveHP);

            yield return null;
            _waveFlag = false;
        }

        private IEnumerator StartBossTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                _bossTimer += 1.0f;
                if (_bossTimer >= _bossTimerThreshold)
                {
                    BossNotBeatenInTime?.Invoke(this, new EventArgs());
                    _bossTimer = 0;
                    yield break;
                }
            }
        }

        private void WaveHpChangedHandler(object sender, EventArgs<float> hp)
        {
            _waveCanvas.Render(hp.Val);
        }
    }
}