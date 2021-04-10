using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgeRevisitEffect : MonoBehaviour
    {

        [SerializeField] [ColorUsage(true, true)] Color travelColor = Color.yellow;
        [SerializeField, Min(0)] float colorTravelDuration = 1;
        [SerializeField] AnimationCurve colorTravelDurationCurve;
        [SerializeField] AnimationCurve colorTravelSizeCurve;
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

            Material material = edge.Material;
            //material.

            float t = 0;

            while (t<1)
            {
                Execute(t,material);
                yield return null;
                t += Time.deltaTime / colorTravelDuration;
            }
            Execute(1, material);
        }
        public void Execute(float t, Material material)
        {
            float travelProgress = colorTravelDurationCurve.Evaluate(t);
            material.SetFloat("_TravelProgress", travelProgress);

            float travelSize = colorTravelSizeCurve.Evaluate(t);
            material.SetFloat("_TravelSize", travelSize);
        }
        public float CalculateEffectDuration()
        {
            return colorTravelDuration;
        }

    }
}
