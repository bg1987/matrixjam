using Assets.TeamMeta.MatrixTravelTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixOver : MonoBehaviour
    {
        [SerializeField] Transitioner transitioner;

        public void Execute()
        {
            transitioner.TransitionMatrixOver();
        }
    }
}
