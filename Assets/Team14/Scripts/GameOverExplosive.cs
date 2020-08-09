using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatrixJam.Team14
{
    public class GameOverExplosive : MonoBehaviour
    {
        public static void Explode()
        {
            Array.ForEach(FindObjectsOfType<GameOverExplosive>(),
                    x => x.ExplodeSelf());
        }

        private bool exploded = false;
        private Vector3 direction;
        private Vector3 rotation;
        public float force = 8;
        
        private void ExplodeSelf()
        {
            exploded = true;
            rotation = Random.insideUnitSphere;
            direction = rotation;
        }

        void Update()
        {
            if (!exploded)
            {
                return;
            }

            transform.localPosition += direction;
            transform.Rotate(rotation);
            direction *= .8f;
        }


        [ContextMenu("Explode all")]
        private void ExplodeAll()
        {
            Explode();
        }

    }
}
