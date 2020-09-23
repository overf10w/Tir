using System;
using System.Collections;
using UnityEngine;


namespace Game
{
    public class CubeHpChangeEventArgs : EventArgs
    {
        public float Value { get; private set; }
        public float Diff { get; private set; }
        public bool ImpactByPlayer { get; private set; }

        public CubeHpChangeEventArgs(float value, float diff, bool impactByPlayer)
        {
            Value = value;
            Diff = diff;
            ImpactByPlayer = impactByPlayer;
        }
    }

    public class Cube : MonoBehaviour, ICube
    {
        #region IDestroyable
        public void TakeDamage(float damage, bool impactByPlayer)
        {
            DamageTaken?.Invoke(this, new CubeTakeDamageEventArgs(damage, impactByPlayer));
        }
        #endregion

        [SerializeField] private SoundsMachine _soundsMachine;
        public SoundsMachine SoundsMachine => _soundsMachine;

        [SerializeField] private ParticleMachine _particleMachine;
        public ParticleMachine ParticleMachine => _particleMachine;

        public event EventHandler<CubeTakeDamageEventArgs> DamageTaken;
        public event EventHandler<CubeHpChangeEventArgs> HpChanged;

        public void SetHealth(float value, bool impactByPlayer)
        {
            float prevHealth = _health;
            _health = value;

            float diff = prevHealth - _health;

            if (_health < 0)
            {
                diff = prevHealth;
                _health = 0;
            }

            HpChanged?.Invoke(this, new CubeHpChangeEventArgs(_health, diff, impactByPlayer));
        }

        private float _health = 100.0f;
        public float Health 
        { 
            get => _health; 
            //set 
            //{
            //    float prevHealth = _health;
            //    _health = value;

            //    float diff = prevHealth - _health;

            //    if (_health < 0)
            //    {
            //        diff = prevHealth;
            //        _health = 0;
            //    }

            //    //HpChanged?.Invoke(this, new CubeHpChangeEventArgs(_health, diff));
            //} 
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

        public void ShowHealth()
        {
            _changeHPQueue.Run(ChangeHpRoutine());
        }

        public void Destroy()
        {
            _changeHPQueue.Run(DestroyRoutine());
        }

        private IEnumerator ChangeHpRoutine()
        {
            yield return new WaitForSeconds(_cubeStat.takeDamageEffectDuration);
        }

        private bool destroyRoutineFlag = false;

        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForEndOfFrame();
            MessageBus.Instance.SendMessage(new Message { Type = MessageType.CUBE_DEATH, objectValue = (Cube)this });
            _particleMachine.Spawn("DestroyParticles", transform);
            yield return new WaitForSeconds(0.15f);
            Destroy(gameObject);
        }
    }
}