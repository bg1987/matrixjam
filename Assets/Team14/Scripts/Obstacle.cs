using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team14
{
    public class ObstaclePayload
    {
        public readonly Obstacle Obstacle;
        public bool Successful;
        public TrainMove Move;
        public Transform MoveCue;

        public ObstaclePayload(Obstacle obstacle, bool successful, TrainMove move, Transform moveCue)
        {
            Obstacle = obstacle;
            Successful = successful;
            Move = move;
            MoveCue = moveCue;
        }
    }
    
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private TrainMove trainMove;
        [SerializeField] private Transform moveCue; // Where should actually do the move. Null = do when triggers

        public static event Action<ObstaclePayload> OnObstacleEvent;
  
        private bool isEnteredInsideTrigger = false;
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("entered");
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
