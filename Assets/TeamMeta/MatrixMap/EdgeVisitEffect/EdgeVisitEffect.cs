using MatrixJam.TeamMeta.MatrixMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class EdgeVisitEffect : MonoBehaviour
    {
        [SerializeField] EdgeFirstVisitEffect firstVisitEffect;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void Play(Edge edge, float delay, bool isFirstVisit)
        {
            if (isFirstVisit)
                firstVisitEffect.Play(edge, delay);


        }
    }
}
