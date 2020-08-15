using System;
using System.Collections;
using UnityEngine;


namespace Game
{
    public class CubeHpChangeEventArgs : EventArgs
    {
        public float Value { get; private set; }
        public float Diff { get; private set; }

        public CubeHpChangeEventArgs(float value, float diff)
        {
            Value = value;
            Diff = diff;
        }
    }

    public class Cube : MonoBehaviour, IDestroyable
    {
        #region IDestroyable
        public void TakeDamage(float damage)
        {
            DamageTaken?.Invoke(this, new EventArgs<float>(damage));
        }
        #endregion

        [SerializeField] private SoundsMachine _soundsMachine;
        public SoundsMachine SoundsMachine => _soundsMachine;

        [SerializeField] private ParticleMachine _particleMachine;
        public ParticleMachine ParticleMachine => _particleMachine;

        public event EventHandler<EventArgs<float>> DamageTaken;
        public event EventHandler<CubeHpChangeEventArgs> HpChanged;

        private float _health = 100.0f;
        public float Health 
        { 
            get => _health; 
            set 
            {
                float prevHealth = _health;
                _health = value;

                float diff = prevHealth - _health;

                if (_health < 0)
                {
                    diff = prevHealth;
                    _health = 0;
                }

                HpChanged?.Invoke(this, new CubeHpChangeEventArgs(_health, diff));
            } 
        }

        private float _gold;
        public float Gold => _gold;

        private CubeStat _cubeStat;

        private Transform _cachedTransform;
        private Vector4 _newVec;
        private Vector4 _cachedVec;

        private CoroutineQueue _takeDamageQueue;
        private CoroutineQueue _changeHPQueue;

        public void Init(float hp, float gold)
        {
            _takeDamageQueue = new CoroutineQueue(1, StartCoroutine);
            _changeHPQueue = new CoroutineQueue(1, StartCoroutine);

            _cachedTransform = transform;

            _cubeStat = Resources.Load<CubeStats>("SO/CubeStats").Stats;
            _gold = gold;
            _soundsMachine.Init();

            _health = hp;
        }

        public void ShowHealth(float health)
        {
            _changeHPQueue.Run(ChangeHpRoutine(health));
        }

        public void Destroy()
        {
            _changeHPQueue.Run(DestroyRoutine());
        }

        private IEnumerator ChangeHpRoutine(float health)
        {
            yield return new WaitForSeconds(_cubeStat.takeDamageEffectDuration);
        }

        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForEndOfFrame();
            MessageBus.Instance.SendMessage(new Message { Type = MessageType.CUBE_DEATH, objectValue = (Cube)this });
            _particleMachine.Spawn("DestroyParticles", transform);
            yield return new WaitForSeconds(0.15f);
            Destroy(this.gameObject);
        }
    }
}