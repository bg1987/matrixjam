using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class Brackets : MonoBehaviour
    {
        Material material;

        [SerializeField] float baseFadeOutBracketsDistanceOffset = 0.1f;

        Color baseColor;
        Vector4 baseStartPos;
        float baseHeight;
        float baseBracketsDistance;
        float baseWidth;
        private Coroutine fadeRoutine;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;

            baseColor = material.GetColor("_Color");
            baseStartPos = material.GetVector("_StartPos");
            baseHeight = baseStartPos.y;

            baseBracketsDistance = material.GetFloat("_BracketsDistance");
            baseFadeOutBracketsDistanceOffset = baseBracketsDistance - baseFadeOutBracketsDistanceOffset;

            baseWidth = material.GetFloat("_Width");
        }
        public void Appear(float duration)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(FadeRoutine(duration, 0, 1));
        }
        public void Disappear(float duration)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);
            if (duration == 0)
            {
                DisappearInstantly();
                return;
            }
            fadeRoutine = StartCoroutine(FadeRoutine(duration, 1, 0));
        }
        void DisappearInstantly()
        {
            FadeExecute(1, 1, 0);
        }
        IEnumerator FadeRoutine(float duration, float startAlpha, float targetAlpha)
        {

            var color = material.GetColor("_Color");
            var currentAlpha = color.a;

            float t = Mathf.InverseLerp(startAlpha, targetAlpha, currentAlpha);

            //var fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            while (t < 1)
            {
                //float fadeT = fadeCurve.Evaluate(t);

                FadeExecute(t, startAlpha, targetAlpha);
                t += Time.deltaTime / duration;
                yield return null;
            }
            FadeExecute(1, startAlpha, targetAlpha);

        }
        void FadeExecute(float t, float startAlpha, float targetAlpha)
        {
            var color = baseColor;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            material.SetColor("_Color", color);

            float startHeight;
            float startBracketsDisance;
            float startWidth;

            float targetHeight;
            float targetBracketsDistance;
            float targetWidth;
            if (targetAlpha == 1)
            {
                startHeight = 0;
                startBracketsDisance = baseFadeOutBracketsDistanceOffset;
                startWidth = 0;

                targetHeight = baseHeight;
                targetBracketsDistance = baseBracketsDistance;
                targetWidth = baseWidth;
            }
            else
            {
                startHeight = baseHeight;
                startBracketsDisance = baseBracketsDistance;
                startWidth = baseWidth;

                targetHeight = 0;
                targetBracketsDistance = baseFadeOutBracketsDistanceOffset;
                targetWidth = 0;
            }

            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            material.SetColor("_Color", color);

            float height = Mathf.Lerp(startHeight, targetHeight, t);
            var StartPos = baseStartPos;
            StartPos.y = height;
            material.SetColor("_StartPos", StartPos);

            float bracketsDistance= Mathf.SmoothStep(startBracketsDisance, targetBracketsDistance, t);
            material.SetFloat("_BracketsDistance", bracketsDistance);

            //float width = Mathf.Lerp(startWidth, targetWidth, t);
            //material.SetFloat("_Width", width);
        }
    }
}
