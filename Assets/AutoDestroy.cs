using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] private float _timer = 4.0f;

        private void Awake()
        {
            StartCoroutine(DestroyRoutine(_timer));
        }

        private IEnumerator DestroyRoutine(float timer)
        {
            yield return new WaitForSeconds(timer);
            Destroy(gameObject);
        }
    }
}