using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CubeRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject _sides;
        [SerializeField] private Outline _outline;

        [SerializeField] private float _outlineEffectDuration = 0.1f;
        private Coroutine _outlineRoutine;

        private Cube _cube;
        private CubeStat _cubeStat;
        private MeshRenderer _renderer;

        private float _maxHP;
        private float _currHP;
        private float _hpScaleMultiplier;

        private void Start()
        {
            _cube = GetComponentInParent<Cube>();
            _cube.HpChanged += HandleHpChange;
            _cubeStat = Resources.Load<CubeStats>("SO/CubeStats").Stats;

            _renderer = GetComponent<MeshRenderer>();
            _renderer.material = _cubeStat.materials.PickRandom();

            _currHP = _maxHP = _cube.Health;
            _hpScaleMultiplier = transform.localScale.y / _maxHP;
        }

        private bool _outlineRunning = false;

        private IEnumerator FadeRoutine(bool impactByPlayer)
        {
            if (_outlineRunning)
            {
                if (_outlineRoutine != null)
                {
                    StopCoroutine(_outlineRoutine);
                    _outlineRunning = false;
                }
                //_outline.enabled = true;
                yield return new WaitForSeconds(0.05f);
                //_outline.enabled = false;
            }
            else
            {
                if (_outlineRoutine != null)
                {
                    StopCoroutine(_outlineRoutine);
                    _outlineRunning = false;
                }
                _outline.enabled = true;
                if (impactByPlayer)
                {
                    _outline.color = 0;
                }
                else
                {
                    _outline.color = 1;
                }
                yield return new WaitForSeconds(0.05f);
                _outline.enabled = false;
            }

            _sides.SetActive(false);
            _renderer.enabled = false;
        }

        private void HandleHpChange(object sender, CubeHpChangeEventArgs hp)
        {
            if (hp.Value <= 0)
            {
                LeanTween.cancel(gameObject);
                StartCoroutine(FadeRoutine(hp.ImpactByPlayer));
                return;
            }

            float deltaHp = _currHP - hp.Value;

            float deltaScaleY = deltaHp * _hpScaleMultiplier;
            Vector3 prevScale = transform.localScale;
            float prevScaleY = prevScale.y;
            float newScaleY = prevScaleY - deltaScaleY;

            if (newScaleY <= 0)
            {
                return;
            }

            StartCoroutine(Lerp(prevScaleY, newScaleY, 0.06f, (float val) =>
                {
                    transform.localScale = new Vector3(prevScale.x, val, prevScale.z);
                })
            );

            Vector3 prevPos = transform.localPosition;
            float prevPosY = prevPos.y;
            float deltaPosY = deltaScaleY / 2.0f;
            float newPosY = prevPosY - deltaPosY;
            
            StartCoroutine(Lerp(prevPosY, newPosY, 0.06f, (float val) => 
                {
                    transform.localPosition = new Vector3(prevPos.x, val, prevPos.z);
                })
            );

            if (_outlineRoutine == null)
            {
                _outlineRoutine = StartCoroutine(ShowOutline(_outlineEffectDuration, hp.ImpactByPlayer));
            }
            else
            {
                StopCoroutine(_outlineRoutine);
                _outlineRoutine = StartCoroutine(ShowOutline(_outlineEffectDuration, hp.ImpactByPlayer));
            }
            _currHP = hp.Value;
        }

        private IEnumerator Lerp(float a, float b, float time, Action<float> callback)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                float value = Mathf.SmoothStep(a, b, elapsedTime / time);
                callback?.Invoke(value);
                yield return null;
            }
            callback?.Invoke(b);
        }

        private IEnumerator ShowOutline(float outlineEffectDuration, bool impactByPlayer)
        {
            _outlineRunning = true;
            _outline.enabled = true;
            if (impactByPlayer)
            {
                _outline.color = 0;
            }
            else
            {
                _outline.color = 1;
            }
            yield return new WaitForSeconds(outlineEffectDuration);
            _outline.color = 0; // TODO: work out this dirty hack =))
            _outline.enabled = false;
            _outlineRunning = false;
        }
    }
}