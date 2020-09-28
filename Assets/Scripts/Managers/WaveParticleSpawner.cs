using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WaveParticleSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleMachine _particleMachine;
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private Transform _epicSpawnTransform;

        public void Init()
        {
            _particleMachine.Spawn("WaveChangedParticles", _spawnTransform);
        }

        public void Spawn()
        {
            _particleMachine.Spawn("WaveChangedParticles", _spawnTransform);
        }

        public void Show(float duration)
        {
            _particleMachine.Show("WaveChangedParticles", _spawnTransform, duration);
        }

        public void ShowEpic(float duration)
        {
            _particleMachine.ShowLeanScale("LevelChangedParticles", _epicSpawnTransform, duration, new Vector3(3.5f, 3.5f, 3.5f), new Vector3(28, 28, 28));
        }
    }
}