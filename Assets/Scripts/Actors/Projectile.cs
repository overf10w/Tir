using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Projectile : MonoBehaviour
    {
        //public List<LayerMask> collisionMasks;

        //public LayerMask friendlyLayer;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        //public void SetTargetLayers(params LayerMask[] layerMasks)
        //{
        //    foreach (var layerMask in layerMasks)
        //    {
        //        collisionMasks.Add(layerMask);
        //    }
        //}

        //void OnTriggerEnter2D(Collider2D collision)
        //{
        //    int layer = collision.gameObject.layer;

        //    if (layer == LayerMask.NameToLayer("Projectile"))
        //    {
        //        DestroySelf();
        //        return;
        //    }
        //    // If Submarine instantiated a projectile and it hit another submarine
        //    if (layer == friendlyLayer && lifeTime >= 0.05f)
        //    {
        //        DestroySelf();
        //        return;
        //    }

        //    var entity = collision.gameObject.GetComponent<IDestroyable>();

        //    if (collisionMasks.Contains(layer))
        //    {
        //        if (entity != null)
        //        {
        //            entity.TakeDamage(0.0f);

        //            if (isSpawnedByPlayer && LayerMask.LayerToName(layer) != "Mine")
        //            {
        //                burstSound.source.Play();
        //            }

        //            DestroySelf();
        //        }
        //    }
        //}
    }
}