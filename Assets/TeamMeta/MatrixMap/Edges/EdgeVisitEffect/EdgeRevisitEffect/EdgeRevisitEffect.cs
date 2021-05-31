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
        [SerializeField] float endEdgeColorHeight = 1.2f;
        [Header("Sparks Effect")]
        [SerializeField] ParticleSystem sparksEffect;
        [SerializeField] bool shouldSparksStopAtEnd = false;
        float sparksDistanceAlongEdge;
        int sparksEdgePointIndex;
        List<Vector3> edgePoints;
        Edge latestEdgeSubmitted;
        // Start is called before the first frame update
        void Start()
        {

        }
        public void SetTravelDuration(float duration)
        {
            colorTravelDuration = duration;
        }
        public void SetEndEdgeColorHeight(float value)
        {
            endEdgeColorHeight = value;
        }
        public void SetSparksStopAtEnd(bool value)
        {
            shouldSparksStopAtEnd = value;
        }
        public void Play(Edge edge, float delay)
        {
            //StopAllCoroutines();
            StartCoroutine(PlayRoutine(edge, delay));
        }
        IEnumerator PlayRoutine(Edge edge, float delay)
        {
            latestEdgeSubmitted = edge;
            if(delay>0)
                yield return new WaitForSeconds(delay);

            Material material = edge.Material;
            material.SetFloat("_EndEdgeColorHeight", endEdgeColorHeight);

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
            if(latestEdgeSubmitted.index == edge.index)
                if(shouldSparksStopAtEnd)
                    sparksEffect.Stop(true);
        }
        public void Execute(float t, Material material, Edge edge)
        {
            float colorTravelCurveT = colorTravelDurationCurve.Evaluate(t);

            float edgeLength = material.GetFloat("_EdgeLength");
            float edgeOffset = material.GetFloat("_EndEdgeOffset");
            float travelProgressEnd = (edgeLength - edgeOffset)/edgeLength;

            float startEdgeRadius = material.GetFloat("_StartEdgeRadius");
            float travelProgressStart = 1-(edgeLength - startEdgeRadius) / edgeLength;

            float travelProgress = Mathf.Lerp(travelProgressStart, travelProgressEnd, colorTravelCurveT);
            material.SetFloat("_TravelProgress", travelProgress);

            float travelSize = colorTravelSizeCurve.Evaluate(t);
            material.SetFloat("_TravelSize", travelSize);

            if (latestEdgeSubmitted.index == edge.index)
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
