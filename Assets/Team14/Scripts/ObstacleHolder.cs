using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team14
{
    public class ObstacleHolder : MonoBehaviour
    {
        [SerializeField] private Obstacle obstacle;
        public Obstacle Obstacle => obstacle;
    }
}
