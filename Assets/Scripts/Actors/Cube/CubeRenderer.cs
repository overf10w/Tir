using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CubeRenderer : MonoBehaviour
    {
        private Cube _cube;

        private float _maxHP;
        private float _currHP;
        private float _hpScaleMultiplier;

        private CubeStat _cubeStat;

        private MeshRenderer _renderer;

        [SerializeField] private GameObject _sides;

        private Vector3 _cachedScale;
        private Vector3 _cachedPosition;

        private void Start()
        {
            _cachedScale = transform.localScale;
            _cachedPosition = transform.position;

            _cube = GetComponentInParent<Cube>();
            _cube.HpChanged += HandleHpChange;

            _cubeStat = Resources.Load<CubeStats>("SO/CubeStats").Stats;

            _renderer = GetComponent<MeshRenderer>();

            _renderer.material = _cubeStat.materials.PickRandom();

            _currHP = _maxHP = _cube.Health;
            _hpScaleMultiplier = transform.localScale.y / _maxHP;
        }

        private void HandleHpChange(object sender, CubeHpChangeEventArgs hp)
        {
            if (hp.Value <= 0)
            {
                _sides.SetActive(false);
                _renderer.enabled = false;
                return;
            }

            float deltaHp = _currHP - hp.Value;
            float deltaScale = deltaHp * _hpScaleMultiplier;

            Vector3 prevScale = transform.localScale;
            float deltaScaleY = prevScale.y - deltaScale;
            if (deltaScaleY <= 0)
            {
                transform.localScale = new Vector3(prevScale.x, 0, prevScale.z);
                transform.localPosition = new Vector3(transform.localPosition.x, _cachedPosition.y - (_cachedScale.y / 2.0f), transform.localPosition.z);
                return;
            }
            transform.localScale = new Vector3(prevScale.x, deltaScaleY, prevScale.z);

            Vector3 prevPos = transform.localPosition;
            float deltaPos = deltaScale / 2.0f;

            transform.localPosition = new Vector3(prevPos.x, prevPos.y - deltaPos, prevPos.z);

            _currHP = hp.Value;
        }
    }
}