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
            OnTakeDamage?.Invoke(this, new GenericEventArgs<float>(damage));
        }
        #endregion

        [SerializeField] private SoundsMachine _soundsMachine;
        public SoundsMachine SoundsMachine => _soundsMachine;

        public event EventHandler<GenericEventArgs<float>> OnTakeDamage;
        public event EventHandler<CubeHpChangeEventArgs> OnHpChange;

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

                OnHpChange?.Invoke(this, new CubeHpChangeEventArgs(_health, diff));
            } 
        }

        private float _gold = 2;
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

            //Debug.Log("Cube.cs: health: " + health);

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
            //Show(health);
            yield return new WaitForSeconds(_cubeStat.takeDamageEffectDuration);
            //yield return null;
        }

        //private void Show(float hp)
        //{
        //    if (hp <= 0)
        //    {
        //        return;
        //    }
        //    float wpDistance = _cachedTransform.localScale.y * 1.0f;
        //    float x = wpDistance - (wpDistance * hp / 10.0f);
        //}

        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForEndOfFrame();
            MessageBus.Instance.SendMessage(new Message { Type = MessageType.CUBE_DEATH, objectValue = (Cube)this });
            Destroy(this.gameObject);
        }
    }
}