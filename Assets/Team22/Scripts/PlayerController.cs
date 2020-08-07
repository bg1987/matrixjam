using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class PlayerController : MonoBehaviour
    {
        public TriggerController triggerController;
        public float missDelay = 1f;

        private bool canShoot = true;
        private float missTimer = 0;

        // Update is called once per frame
        void Update()
        {
            if (missTimer > 0)
                missTimer -= Time.deltaTime;
            else
                canShoot = true;

            if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                if(canShoot)
                {
                    CheckTrigger();
                }
            }
        }

        private void CheckTrigger()
        {
            // TODO: Animation
            bool inTrigger = triggerController.GetTriggerStatus();

            if (inTrigger)
            {
                // bamboo is inside trigger
                // Debug.LogWarning("HIT!");
                triggerController.DestroyInside();
            }
            else
            {
                // missed bamboo
                // Debug.LogWarning("MISS!");
                canShoot = false;
                missTimer = missDelay;
            }
        }
    }
}
