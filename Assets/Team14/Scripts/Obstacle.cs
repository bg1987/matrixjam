using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Dictionary<TrainMove, List<Obstacle>> CurrObstacles;

        private bool _succeeded;

        public TrainMove Move => trainMove;
        public Transform MoveCue => moveCue;
        
        public static event Action<ObstaclePayload> OnObstacleEvent;
        
        private void Awake()
        {
            CurrObstacles = Enum
                .GetValues(typeof(TrainMove))
                .Cast<TrainMove>()
                .ToDictionary(
                    move => move, 
                    move => new List<Obstacle>()
                );
        }

        public void OnPressedInZone()
        {
            _succeeded = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            Debug.Log("entered");
            CurrObstacles[trainMove].Add(this);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            Debug.Log("exit");
            CurrObstacles[trainMove].Remove(this);
            
            
            if (!_succeeded) OnObstacleEvent?.Invoke(new ObstaclePayload(this, false, Move, moveCue));
        }
    }
}
