using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CubeRenderer : MonoBehaviour
    {
        private Cube cube;

        private float maxHP;

        private float currHP;

        private float hpScaleMultiplier;

        private void Start()
        {
            cube = GetComponentInParent<Cube>();
            cube.OnHpChange += HandleHpChange;

            currHP = maxHP = cube.Health;
            hpScaleMultiplier = transform.localScale.y / maxHP;
        }

        private void HandleHpChange(object sender, GenericEventArgs<float> hp)
        {
            if (currHP <= 0)
            {
                return;
            }

            Debug.Log("CubeRenderer: HandleHPChange");
            float deltaHp = currHP - hp.val;
            float deltaScale = deltaHp * hpScaleMultiplier;

            Vector3 prevScale = transform.localScale;
            transform.localScale = new Vector3(prevScale.x, prevScale.y - deltaScale, prevScale.z);

            Vector3 prevPos = transform.localPosition;
            float deltaPos = deltaScale / 2.0f;
            transform.localPosition = new Vector3(prevPos.x, prevPos.y - deltaPos, prevPos.z);

            currHP = hp.val;
        }
    }
}