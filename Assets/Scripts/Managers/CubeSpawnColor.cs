using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CubeSpawnColor : MonoBehaviour
    {
        [SerializeField] private Color _color;
        [SerializeField] private bool _random;

        private bool _flag = false;

        //public Color Color => _random ? Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f) : _color;

        public Color Color
        {
            get
            {
                if (_random)
                {
                    if (!_flag)
                    {
                        _flag = true;
                        _color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); 
                    }
                }
                else
                {
                    _flag = false;
                }
                return _color;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color;
            //Gizmos.DrawIcon(transform.position, "s");
            //Gizmos.DrawIcon(transform.position, "Sho", true);
            Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
        }
    }
}