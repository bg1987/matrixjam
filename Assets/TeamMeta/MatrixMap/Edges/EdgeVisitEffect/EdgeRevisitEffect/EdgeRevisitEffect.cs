using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgeRevisitEffect : MonoBehaviour
    {
        [SerializeField, Min(0)] float colorTravelDuration = 1;
        [SerializeField] AnimationCurve colorTravelDurationCurve;
        [SerializeField] AnimationCurve colorTravelSizeCurve;

        [Header("Sparks Effect")]
        [SerializeField] ParticleSystem sparksEffect;
        float sparksDistanceAlongEdge;
        int sparksEdgePointIndex;
        List<Vector3> edgePoints;
        // Start is called before the first frame update
        void Start()
        {

        }
        public void Play(Edge edge, float delay)
        {
            StopAllCoroutines();
            StartCoroutine(PlayRoutine(edge, delay));
        }
        IEnumerator PlayRoutine(Edge edge, float delay)
        {
            yield return new WaitForSeconds(delay);

            Material material = edge.Material;

            sparksEffect.Play(true);
            edgePoints = edge.GetCurvePoints();
            sparksDistanceAlongEdge = 0;
            sparksEdgePointIndex = 0;

            sparksEffect.transform.localPosition = Vector3.zero;

            transform.position = edgePoints[sparksEdgePointIndex] + edge.transform.position;
            float t = 0;

            while (t<1)
            {
                Execute(t,material, edge);

                yield return null;
                t += Time.deltaTime / colorTravelDuration;
            }
            Execute(1, material, edge);
            //sparksEffect.Stop();
        }
        public void Execute(float t, Material material, Edge edge)
        {
            float travelProgress = colorTravelDurationCurve.Evaluate(t);
            material.SetFloat("_TravelProgress", travelProgress);

            float travelSize = colorTravelSizeCurve.Evaluate(t);
            material.SetFloat("_TravelSize", travelSize);

            MoveSparksEffect(travelProgress, material, edge.transform.position);
        }
        void MoveSparksEffect(float travelDistance, Material material, Vector3 originPosition)
        {
            var targetDistance = travelDistance * material.GetFloat("_EdgeLength");
            while (sparksDistanceAlongEdge < targetDistance - 0.01f)
            {
                Vector3 currentPosition = transform.position;
                Vector3 nextEdgePoint;

                nextEdgePoint = edgePoints[sparksEdgePointIndex + 1]+ originPosition;

                float distanceToNextPoint = Vector3.Distance(currentPosition, nextEdgePoint);
                float nextPointDistanceAlongEdge = sparksDistanceAlongEdge + distanceToNextPoint;

                if (nextPointDistanceAlongEdge < targetDistance)
                {
                    sparksDistanceAlongEdge = nextPointDistanceAlongEdge;

                    sparksEdgePointIndex++;

                    transform.position = nextEdgePoint;

                }
                else if (nextPointDistanceAlongEdge >= targetDistance)
                {
                    Vector3 direction = nextEdgePoint - currentPosition;
                    transform.LookAt(transform.position + direction);
                    direction.Normalize();
                    float extraDistanceToTarget = targetDistance - sparksDistanceAlongEdge;

                    transform.position += direction * extraDistanceToTarget;

                    sparksDistanceAlongEdge = targetDistance;

                    if (nextPointDistanceAlongEdge == targetDistance)
                    {
                        if (sparksEdgePointIndex<edgePoints.Count-1)
                        {
                            sparksEdgePointIndex++;
                        }
                    }
                }
            }
            if(travelDistance > 0.9f)
            {
                float targetPositionOffsetZ = -1f;
                float targetPositionOffsetX = -0.2f;
                float offsetT = Mathf.InverseLerp(0.9f, 1, travelDistance);
                float positionOffsetZ = Mathf.Lerp(0, targetPositionOffsetZ, offsetT);
                float positionOffset = Mathf.Lerp(0, targetPositionOffsetX, offsetT);
                var localPos = sparksEffect.transform.localPosition;
                localPos.z = targetPositionOffsetZ;
                sparksEffect.transform.localPosition = localPos;
            }
        }
        public float CalculateEffectDuration()
        {
            return colorTravelDuration;
        }
        public void Stop()
        {
            sparksEffect.Stop();
        }
    }
}
