using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

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
        [SerializeField] private BoxCollider trigger; // For Gizmos

        public static Dictionary<TrainMove, List<Obstacle>> CurrObstacles;

        private bool _succeeded;

        public TrainMove Move => trainMove;
        public Transform MoveCue => moveCue;
        
        public static event Action<ObstaclePayload> OnObstacleEvent;
        
        private void Awake()
        {
            GameManager.ResetEvent += OnGameReset;
            CurrObstacles = Enum
                .GetValues(typeof(TrainMove))
                .Cast<TrainMove>()
                .ToDictionary(
                    move => move, 
                    move => new List<Obstacle>()
                );
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.0f, 1.0f, 0.25f, 0.3f);
            Gizmos.DrawCube(trigger.transform.position + trigger.center, trigger.size);
        }

        private void OnDestroy()
        {
            GameManager.ResetEvent -= OnGameReset;
        }

        public void OnPressedInZone()
        {
            _succeeded = true;
        }

        /// <summary>
        /// Returns true if was in an obstacle zone
        /// </summary>
        public static Obstacle HandleMovePressed(TrainMove move)
        {
            var obstacles = CurrObstacles[move];
            var obs = obstacles.FirstOrDefault();

            Assert.IsTrue(obstacles.Count <= 1, "More than one obstacle should not overlap!");
            if (!obs) return null;
            
            obs.OnPressedInZone();
            return obs;
        }

        private void OnGameReset()
        {
            _succeeded = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            CurrObstacles[trainMove].Add(this);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            CurrObstacles[trainMove].Remove(this);

            if (!_succeeded) OnObstacleEvent?.Invoke(new ObstaclePayload(this, false, Move, moveCue));
        }
    }
}
