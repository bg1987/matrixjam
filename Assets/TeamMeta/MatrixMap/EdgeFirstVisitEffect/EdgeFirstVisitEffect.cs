using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgeFirstVisitEffect : MonoBehaviour
    {
        [SerializeField] ParticleSystem EdgeStartSparksEffect;
        // Start is called before the first frame update
        void Start()
        {
            
        }
        public void Play(Edge edge, float delay)
        {
            StartCoroutine(PlayRoutine(edge, delay));
        }
        IEnumerator PlayRoutine(Edge edge, float delay)
        {
            yield return new WaitForSeconds(delay);
            var positionAndDirection = edge.CalculateDirectionAndPositionClosestToEdgeStart();

            var v1 = EdgeStartSparksEffect.transform.right;
            var v2 = positionAndDirection[1];

            var degrees = Mathf.Atan2(v2.y, v2.x) - Mathf.Atan2(v1.y, v1.x);
            degrees *= Mathf.Rad2Deg;
            Vector3 effectPosition = positionAndDirection[0];
            transform.position = effectPosition;
            transform.Rotate(0, 0, degrees);

            EdgeStartSparksEffect.Play(true);
        }

    }
}
