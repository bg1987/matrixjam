using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgeVisitEffect : MonoBehaviour
    {
        [SerializeField] EdgeFirstVisitEffect firstVisitEffect;
        public EdgeFirstVisitEffect FirstVisitEffect { get => firstVisitEffect; }
        [SerializeField] EdgeRevisitEffect revisitEffect;
        public EdgeRevisitEffect RevisitEffect { get => revisitEffect; }

        // Start is called before the first frame update
        void Start()
        {
            
        }
    }
}
