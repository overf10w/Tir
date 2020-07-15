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

        private void Start()
        {
            _cube = GetComponentInParent<Cube>();
            _cube.OnHpChange += HandleHpChange;

            _currHP = _maxHP = _cube.Health;
            _hpScaleMultiplier = transform.localScale.y / _maxHP;
        }

        private void HandleHpChange(object sender, GenericEventArgs<float> hp)
        {
            if (_currHP <= 0)
            {
                return;
            }

            float deltaHp = _currHP - hp.Val;
            float deltaScale = deltaHp * _hpScaleMultiplier;

            Vector3 prevScale = transform.localScale;
            transform.localScale = new Vector3(prevScale.x, prevScale.y - deltaScale, prevScale.z);

            Vector3 prevPos = transform.localPosition;
            float deltaPos = deltaScale / 2.0f;
            transform.localPosition = new Vector3(prevPos.x, prevPos.y - deltaPos, prevPos.z);

            _currHP = hp.Val;
        }
    }
}