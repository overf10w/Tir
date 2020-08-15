using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class SoundsMachine : MonoBehaviour
    {
        [SerializeField] private Sound[] _sounds;

        private System.Random rnd = new System.Random();

        public void Init()
        {
            SetSounds(_sounds);
        }

        private void SetSounds(params Sound[] sounds)
        {
            foreach (var s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.rolloffMode = AudioRolloffMode.Custom;
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.maxDistance = s.distance;
                s.source.spatialBlend = s.spatialBlend;

                if (s.playOnAwake)
                {
                    Play(s.name);
                }
            }
        }

        public async Task Play(string name)
        {
            Sound s = Array.Find(_sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogError("Sound " + name + " was not found!");
                return;
            }
            if (s.clip == null)
            {
                Debug.LogError("Sound clip isn't attached to " + name + " sound on " + gameObject.name);
            }
            if (s.source == null)
            {
                Debug.Log("SoundsMachine: source == null: returning...");
                await Task.Yield();
                return;
            }
            if (s.source.isPlaying)
            {
                return;
            }

            //s.source.PlayOneShot(s.clip);
            s.source.Play();

            while (s.source.isPlaying)
            {
                //Debug.Log("SoundsMachine: await Task.Yield: ");
                await Task.Yield();
            }

            //await UniTask.Delay(TimeSpan.FromSeconds(s.clip.length));
        }

        public async Task PlayForced(string name)
        {
            Sound s = Array.Find(_sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogError("Sound " + name + " was not found!");
                return;
            }
            if (s.clip == null)
            {
                Debug.LogError("Sound clip isn't attached to " + name + " sound on " + gameObject.name);
            }
            s.source.Stop();
            s.source.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(s.clip.length));
        }

        public async Task PlayRandomForced(params string[] names)
        {
            string name = names.PickRandom();

            Sound s = Array.Find(_sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogError("Sound " + name + " was not found!");
                return;
            }
            if (s.clip == null)
            {
                Debug.LogError("Sound clip isn't attached to " + name + " sound on " + gameObject.name);
            }
            s.source.Stop();
            s.source.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(s.clip.length));
        }

        public void Stop(string name)
        {
            Sound s = Array.Find(_sounds, sound => sound.name == name);
            s.source.Stop();
        }

        public bool IsPlaying(string name)
        {
            Sound s = Array.Find(_sounds, sound => sound.name == name);
            return s.source.isPlaying;
        }

        public void StopAll()
        {
            foreach (var sound in _sounds)
            {
                sound.source.Stop();
            }
        }

        public void PauseAll()
        {
            foreach (var sound in _sounds)
            {
                if (sound.source.isPlaying)
                {
                    sound.source.Pause();
                }
            }
        }

        public void UnPauseAll()
        {
            foreach (var sound in _sounds)
            {
                if (!sound.source.isPlaying)
                {
                    sound.source.UnPause();
                }
            }
        }
    }
}