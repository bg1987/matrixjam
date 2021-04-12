using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class NodeFirstVisitEffect : MonoBehaviour
    {
        [SerializeField] List<ParticleSystem> particleSystems;

        public void Play(Node node, float delay)
        {
            StartCoroutine(PlayRoutine(node, delay));
            
        }
        IEnumerator PlayRoutine(Node node, float delay)
        {
            yield return new WaitForSeconds(delay);

            Vector3 effectPosition = node.transform.position;
            transform.position = effectPosition;

            foreach (var ps in particleSystems)
            {
                var main = ps.main;
                var startColor = main.startColor;
                
                if(main.startColor.mode==ParticleSystemGradientMode.Color)
                    startColor.color = node.ColorHdr1.color;
                else
                {
                    startColor.colorMin = node.ColorHdr1.color;
                    startColor.colorMax = node.ColorHdr2.color;
                }
                main.startColor = startColor;

                ps.Play(true);
            }
        }
        public float CalculateEffectDuration()
        {
            float longestPsDuration = 0;
            foreach (var ps in particleSystems)
            {
                float psDuration = 0;
                var main = ps.main;
                psDuration += main.startDelay.constantMax;
                psDuration += main.startLifetime.constantMax;

                if (psDuration > longestPsDuration)
                    longestPsDuration = psDuration;
            }

            return longestPsDuration;
        }
    }
}
