using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private TrainMove trainMove;
  
        private int point = 0;
        private bool isEnteredInsideTrigger = false;
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("entered");
            point++;
            isEnteredInsideTrigger = true;
        }
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("exit");
            isEnteredInsideTrigger = false;

        }
        private void Update()
        {
            if (!isEnteredInsideTrigger) return;
            var keyCode = TrainMoves.GetKey(trainMove);
            
            if (Input.GetKeyDown(keyCode))
            {
                Debug.Log("Jump");
            }
        }
    }
}
