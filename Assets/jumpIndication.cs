using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team
{
    public class jumpIndication : MonoBehaviour
    {
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
            if (isEnteredInsideTrigger)
            {
                onFailed();
            }
           
            isEnteredInsideTrigger = false;

        }

        private void Update()
        {
            if (!isEnteredInsideTrigger) return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jump");
            }
        }

        void onFailed()
        {
            
        }
    }
}
