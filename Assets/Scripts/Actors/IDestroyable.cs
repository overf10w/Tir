using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IDestroyable
    {
        void TakeDamage(float damage);
    }
}