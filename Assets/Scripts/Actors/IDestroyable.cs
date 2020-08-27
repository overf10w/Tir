using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ICube
    {
        void TakeDamage(float damage, bool impactByPlayer);
    }
}