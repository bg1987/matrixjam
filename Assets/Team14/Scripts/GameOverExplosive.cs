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
            stopExplosion = false;
            Array.ForEach(FindObjectsOfType<GameOverExplosive>(),
                    x => x.ExplodeSelf());
        }

        public static void StopExplosion()
        {
            stopExplosion = true;
        }

        private static bool stopExplosion = false;

        private bool exploded = false;
        private Vector3 direction;
        private Vector3 rotation;
        public float force = 8;

        private Vector3 oldposition;
        private Vector3 oldrotation;
        
        private void ExplodeSelf()
        {
            if (exploded)
            {
                Debug.Log("Already Exploded");
                return;
            }
            oldposition = transform.localPosition;
            oldrotation = transform.localEulerAngles;
            exploded = true;
            rotation = Random.insideUnitSphere;
            direction = rotation;
        }

        void Update()
        {
            if (stopExplosion && exploded)
            {
                transform.localPosition = oldposition;
                transform.localEulerAngles = oldrotation;
                exploded = false;
            }
            
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
