using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CubeRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject _sides;

        private Cube _cube;
        private CubeStat _cubeStat;
        private MeshRenderer _renderer;

        private float _maxHP;
        private float _currHP;
        private float _hpScaleMultiplier;

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

        private IEnumerator FadeRoutine()
        {
            yield return new WaitForSeconds(0.05f);
            _sides.SetActive(false);
            _renderer.enabled = false;
        }

        private void HandleHpChange(object sender, CubeHpChangeEventArgs hp)
        {
            if (hp.Value <= 0)
            {
                // TODO: remove this, as this causes serious bugs
                //LeanTween.value(0, 1, 0.05f).setOnComplete(() =>
                //{
                //    _sides.SetActive(false);
                //    _renderer.enabled = false;
                //    return;
                //});

                //_sides.SetActive(false);
                //_renderer.enabled = false;
                //return;

                StartCoroutine(FadeRoutine());
            }

            float deltaHp = _currHP - hp.Value;

            float deltaScaleY = deltaHp * _hpScaleMultiplier;
            Vector3 prevScale = transform.localScale;
            float prevScaleY = prevScale.y;
            float newScaleY = prevScaleY - deltaScaleY;
            if (newScaleY <= 0)
            {
                transform.localScale = new Vector3(prevScale.x, 0, prevScale.z);
                transform.localPosition = new Vector3(transform.localPosition.x, _cachedPosition.y - (_cachedScale.y / 2.0f), transform.localPosition.z);
                return;
            }

            // TODO: remove this code as it causes serious bugs
            LeanTween.value(prevScaleY, newScaleY, 0.06f).setOnUpdate((float val) =>
            {
                transform.localScale = new Vector3(prevScale.x, val, prevScale.z);
            });
            
            //transform.localScale = new Vector3(prevScale.x, newScaleY, prevScale.z);

            Vector3 prevPos = transform.localPosition;
            float prevPosY = prevPos.y;
            float deltaPosY = deltaScaleY / 2.0f;
            float newPosY = prevPosY - deltaPosY;
            LeanTween.value(prevPosY, newPosY, 0.06f).setOnUpdate((float val) =>
            {
                transform.localPosition = new Vector3(prevPos.x, val, prevPos.z);
            });
            //transform.localPosition = new Vector3(prevPos.x, prevPos.y - deltaPosY, prevPos.z);

            _currHP = hp.Value;
        }
    }
}