using System;
using System.Collections;
using UnityEngine;


namespace Game
{
    public class Cube : MonoBehaviour, IDestroyable
    {
        #region IDestroyable
        public void TakeDamage(float damage)
        {
            OnTakeDamage?.Invoke(this, new GenericEventArgs<float>(damage));
        }
        #endregion

        public event EventHandler<GenericEventArgs<float>> OnTakeDamage;
        public event EventHandler<GenericEventArgs<float>> OnHpChange;

        private float _health = 100.0f;
        public float Health 
        { 
            get => _health; 
            set 
            { 
                _health = value;
                OnHpChange?.Invoke(this, new GenericEventArgs<float>(_health));
            } 
        }

        private int _gold = 2;
        public int Gold => _gold;

        private CubeStat _cubeStat;

        private Transform _cachedTransform;
        private Vector4 _newVec;
        private Vector4 _cachedVec;

        private CoroutineQueue _takeDamageQueue;
        private CoroutineQueue _changeHPQueue;

        public void Init(float health)
        {
            _takeDamageQueue = new CoroutineQueue(1, StartCoroutine);
            _changeHPQueue = new CoroutineQueue(1, StartCoroutine);

            _cachedTransform = transform;

            _cubeStat = Resources.Load<CubeStats>("SO/CubeStats").Stats;
            _gold = _cubeStat.gold;

            Debug.Log("Cube.cs: health: " + health);

            _health = health;
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
            Show(health);
            yield return new WaitForSeconds(_cubeStat.takeDamageEffectDuration);
        }

        private void Show(float hp)
        {
            if (hp <= 0)
            {
                return;
            }

            float wpDistance = _cachedTransform.localScale.y * 1.0f;
            float x = wpDistance - (wpDistance * hp / 10.0f);
        }

        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForEndOfFrame();
            MessageBus.Instance.SendMessage(new Message { Type = MessageType.CUBE_DEATH, objectValue = (Cube)this });
            Destroy(this.gameObject);
        }

        private IEnumerator TakeDamageRoutine(float damage)
        {
            OnTakeDamage?.Invoke(this, new GenericEventArgs<float>(_cubeStat.takeDamageEffectDuration));
            yield return new WaitForSeconds(0.5f);
            
            _health -= damage;
            Show(_health);
            if (_health <= 0.0f)
            {
                MessageBus.Instance.SendMessage(new Message { Type = MessageType.CUBE_DEATH, objectValue = (Cube)this });
                Destroy(this.gameObject);
            }
        }
    }
}