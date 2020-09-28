using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WaveRotation : MonoBehaviour
    {
        [SerializeField] private float speed = 10.0f;

        void Update()
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }
    }
}