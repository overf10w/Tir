using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Cube : MonoBehaviour, IDestroyable
    {
        public CubeStats cubeStats;

        [SerializeField] private int gold = 2;
        [SerializeField] private float health = 100.0f;

        private Renderer renderer;

        private Transform cachedTransform;

        private Vector4 newVec;

        private Vector4 cachedVec;

        public void Awake()
        {
            cachedTransform = transform;

            cubeStats = Resources.Load<CubeStats>("SO/CubeStats");
            gold = cubeStats.stats.gold;
            health = cubeStats.stats.HP;

            renderer = GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.Log("SHFDSSDKL:F");
            }
            else
            {
                if (renderer.material.HasProperty("_PlanePoint"))
                {
                    cachedVec = renderer.material.GetVector("_PlanePoint");
                }
                newVec = new Vector4(cachedVec.x, transform.position.y + 1.0f * transform.localScale.y, cachedVec.z, cachedVec.w);
                cachedVec = newVec;
                renderer.material.SetVector("_PlanePoint", newVec);
            }
        }

        // TODO: this should be done via Coroutine Queue 
        // TODO: when the cube is being acted upon, its outline should be colored in a different color (ie white)
        public void TakeDamage(float damage)
        {
            health -= damage;
            Show(health);
            if (health <= 0.0f)
            {
                MessageBus.Instance.SendMessage(new Message { Type = MessageType.CubeDeath, objectValue = (Cube)this });
                Destroy(this.gameObject);
            }
        }

        public int Gold
        {
            get { return gold; }
        }

        public void Show(float hp)
        {
            float wpDistance = cachedTransform.localScale.y * 1.0f;
            float x = wpDistance - (wpDistance * hp / 10.0f);

            //Debug.Log("x: " + x + "hp: " + hp);

            if (hp <= 0)
            {
                return;
            }

            renderer.material.SetVector("_PlanePoint", new Vector4(cachedVec.x, cachedVec.y - x, cachedVec.z, cachedVec.w));
        }
    }
}