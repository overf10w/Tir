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

        public async void Show(string name, Transform transform, float duration)
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

            //if (e.particleSystem.isPlaying)
            //{
            //    return;
            //}

            //if (e.constantRotation)
            //{
            //    //e.particleSystem.transform.GetComponent<ParticleSystemConstantRotation>().enabled = true;
            //}
            GameObject obj = Instantiate(e.particleSystem.gameObject, transform.position, Quaternion.identity);

            obj.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);

            ParticleSystem particleSystem = obj.GetComponent<ParticleSystem>();
            particleSystem.Play();
            await Task.Delay(TimeSpan.FromSeconds(duration));
            particleSystem.Stop();
            await Task.Delay(TimeSpan.FromSeconds(15.0f));
            Destroy(obj);
        }

        public async void ShowLeanScale(string name, Transform transform, float duration, Vector3 from, Vector3 to)
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

            //if (e.particleSystem.isPlaying)
            //{
            //    return;
            //}

            //if (e.constantRotation)
            //{
            //    //e.particleSystem.transform.GetComponent<ParticleSystemConstantRotation>().enabled = true;
            //}
            GameObject obj = Instantiate(e.particleSystem.gameObject, transform.position, Quaternion.identity);

            obj.transform.localScale = from;
            obj.transform.LeanScale(to, duration);
            ParticleSystem particleSystem = obj.GetComponent<ParticleSystem>();
            particleSystem.Play();
            await Task.Delay(TimeSpan.FromSeconds(duration));
            particleSystem.Stop();
            await Task.Delay(TimeSpan.FromSeconds(25.0f));
            Destroy(obj);
        }


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