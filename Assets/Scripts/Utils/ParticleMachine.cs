using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UniRx;
using System;

namespace Game
{
    [System.Serializable]
    public class ParticleEffect
    {
        public string name;
        public ParticleSystem particleSystem;
        //public bool constantRotation;
    }

    public class ParticleMachine : MonoBehaviour
    {
        [SerializeField] private ParticleEffect[] particleEffects;

        public void Spawn(string name, Transform transform)
        {
            ParticleEffect e = Array.Find(particleEffects, effect => effect.name == name);
            if (e == null)
            {
                Debug.LogError("Particle " + name + " was not found on " + gameObject.name);
            }
            if (e.particleSystem == null)
            {
                Debug.LogError("ParticleSystem isn't attached to " + name + " particle effect on " + gameObject.name);
            }
            if (e.particleSystem.isPlaying)
            {
                return;
            }
            GameObject obj = Instantiate(e.particleSystem.gameObject, transform.position, Quaternion.identity);
            
        }

        public void Spawn(string name, Vector3 position)
        {
            ParticleEffect e = Array.Find(particleEffects, effect => effect.name == name);
            if (e == null)
            {
                Debug.LogError("Particle " + name + " was not found on " + gameObject.name);
            }
            if (e.particleSystem == null)
            {
                Debug.LogError("ParticleSystem isn't attached to " + name + " particle effect on " + gameObject.name);
            }
            if (e.particleSystem.isPlaying)
            {
                return;
            }
            GameObject obj = Instantiate(e.particleSystem.gameObject, position, Quaternion.identity);

        }

        public async Task Play(string name)
        {
            ParticleEffect e = Array.Find(particleEffects, effect => effect.name == name);

            if (e == null)
            {
                Debug.LogError("Particle " + name + " was not found on " + gameObject.name);
            }

            if (e.particleSystem == null)
            {
                Debug.LogError("ParticleSystem isn't attached to " + name + " particle effect on " + gameObject.name);
            }

            if (e.particleSystem.isPlaying)
            {
                return;
            }

            //if (e.constantRotation)
            //{
            //    //e.particleSystem.transform.GetComponent<ParticleSystemConstantRotation>().enabled = true;
            //}
            e.particleSystem.Play();
            await Task.Delay(TimeSpan.FromSeconds(e.particleSystem.main.duration));
        }

        public void SetSpeed(string name, float speed)
        {
            ParticleEffect e = Array.Find(particleEffects, effect => effect.name == name);
            if (e == null)
            {
                Debug.LogWarning("Particle " + name + " was not found!");
                return;
            }
            ParticleSystem.MainModule pMain = e.particleSystem.main;
            pMain.startSpeed = new ParticleSystem.MinMaxCurve(speed, speed + 2);
        }

        public void Stop(string name)
        {
            ParticleEffect e = Array.Find(particleEffects, effect => effect.name == name);

            if (e == null)
            {
                Debug.LogError("Particle " + name + " was not found on " + gameObject.name);
            }
            if (e.particleSystem == null)
            {
                Debug.LogError("ParticleSystem isn't attached to " + name + " particle effect on " + gameObject.name);
            }
            e.particleSystem.Stop();
        }
    }
}