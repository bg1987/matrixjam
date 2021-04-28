using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class SquareTarget : MonoBehaviour
    {
        
        Material material;
        float baseLineWidth;
        float baseLineLength;

        private Coroutine fadeRoutine;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
            baseLineWidth = material.GetFloat("_LineWidth");
            baseLineLength = material.GetFloat("_LineLength");
        }
        public void Appear(float duration)
        {
            if (fadeRoutine!=null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(AppearRoutine(duration));
        }
        IEnumerator AppearRoutine(float duration)
        {
            baseLineLength = transform.lossyScale.x;

            var color = material.GetColor("_Color");
            var startAlpha = color.a;
            var startLineLength = material.GetFloat("_LineLength");
            var startLineWidth = material.GetFloat("_LineWidth");

            float t = startAlpha;
            while (t < 1)
            {
                color.a = Mathf.Lerp(0, 1, t);
                material.SetColor("_Color", color);

                var lineLength = Mathf.Lerp(0, baseLineLength, t);
                material.SetFloat("_LineLength", lineLength);
                
                //var lineWidth = Mathf.Lerp(startLineWidth, baseLineWidth, t);
                //material.SetFloat("_LineWidth", lineWidth);

                t += Time.deltaTime/duration;
                yield return null;
            }
            color.a = 1;
            material.SetColor("_Color", color);
            material.SetFloat("_LineLength", baseLineLength);
            //material.SetFloat("_LineWidth", baseLineWidth);
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
            fadeRoutine = StartCoroutine(DisappearRoutine(duration));
        }
        IEnumerator DisappearRoutine(float duration)
        {
            baseLineLength = transform.lossyScale.x;

            var color = material.GetColor("_Color");
            var startAlpha = color.a;
            var startLineLength = material.GetFloat("_LineLength");
            var startLineWidth = material.GetFloat("_LineWidth");

            float t = 1 - startAlpha;
            while (t < 1)
            {
                color.a = Mathf.Lerp(1, 0, t);
                material.SetColor("_Color", color);

                var lineLength = Mathf.Lerp(baseLineLength, 0, t);
                material.SetFloat("_LineLength", lineLength);

                //var lineWidth = Mathf.Lerp(startLineWidth, 0, t);
                //material.SetFloat("_LineWidth", lineWidth);

                t += Time.deltaTime / duration;
                yield return null;
            }
            color.a = 0;
            material.SetColor("_Color", color);
            material.SetFloat("_LineLength", 0);
            //material.SetFloat("_LineWidth", 0);
        }
        void DisappearInstantly()
        {
            var color = material.GetColor("_Color");

            color.a = 0;
            material.SetColor("_Color", color);
            material.SetFloat("_LineLength", 0);
            //material.SetFloat("_LineWidth", 0);
        }

    }
}
