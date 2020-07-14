using System;
using System.Collections;
using UnityEngine;


namespace Game
{
    // Cube ModelView
    public class Cube : MonoBehaviour, IDestroyable
    {
        private CubeStat cubeStats;

        private int _gold = 2;

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

        public int Gold
        {
            get => _gold;
        }

        //private Renderer renderer;

        private Transform cachedTransform;

        private Vector4 newVec;

        private Vector4 cachedVec;

        private CoroutineQueue takeDamageQueue;
        private CoroutineQueue changeHPQueue;

        public event EventHandler<GenericEventArgs<float>> OnTakeDamage;
        public event EventHandler<GenericEventArgs<float>> OnHpChange;


        public void Init()
        {
            takeDamageQueue = new CoroutineQueue(1, StartCoroutine);
            changeHPQueue = new CoroutineQueue(1, StartCoroutine);

            cachedTransform = transform;

            cubeStats = Resources.Load<CubeStats>("SO/CubeStats").stats;
            _gold = cubeStats.gold;
            _health = cubeStats.HP;

            //renderer = GetComponent<Renderer>();
            //if (renderer == null)
            //{
            //    Debug.Log("SHFDSSDKL:F");
            //}
            //else
            //{
            //    if (renderer.material.HasProperty("_PlanePoint"))
            //    {
            //        cachedVec = renderer.material.GetVector("_PlanePoint");
            //    }
            //    newVec = new Vector4(cachedVec.x, transform.position.y + 1.0f * transform.localScale.y, cachedVec.z, cachedVec.w);
            //    cachedVec = newVec;
            //    renderer.material.SetVector("_PlanePoint", newVec);
            //}
        }

        public void TakeDamage(float damage)
        {
            //Debug.Log("Cube.cs: TakeDamage()");
            Health -= damage;
            OnTakeDamage?.Invoke(this, new GenericEventArgs<float>(damage));
        }

        public void ShowHealth(float health)
        {
            changeHPQueue.Run(ChangeHpRoutine(health));
        }

        private IEnumerator ChangeHpRoutine(float health)
        {
            Show(health);
            yield return new WaitForSeconds(cubeStats.takeDamageEffectDuration);
        }

        public void Destroy()
        {
            changeHPQueue.Run(DestroyRoutine());
        }

        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForEndOfFrame();
            MessageBus.Instance.SendMessage(new Message { Type = MessageType.CubeDeath, objectValue = (Cube)this });
            Destroy(this.gameObject);
        }

        private IEnumerator TakeDamageRoutine(float damage)
        {
            OnTakeDamage?.Invoke(this, new GenericEventArgs<float>(cubeStats.takeDamageEffectDuration));
            yield return new WaitForSeconds(0.5f);
            
            _health -= damage;
            Show(_health);
            if (_health <= 0.0f)
            {
                MessageBus.Instance.SendMessage(new Message { Type = MessageType.CubeDeath, objectValue = (Cube)this });
                Destroy(this.gameObject);
            }
        }



        public void Show(float hp)
        {
            if (hp <= 0)
            {
                return;
            }

            float wpDistance = cachedTransform.localScale.y * 1.0f;
            float x = wpDistance - (wpDistance * hp / 10.0f);
            //renderer.material.SetVector("_PlanePoint", new Vector4(cachedVec.x, cachedVec.y - x, cachedVec.z, cachedVec.w));
        }
    }
}