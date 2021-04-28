using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class LineShader : MonoBehaviour
    {
        Material material;
        private Coroutine fadeRoutine;

        // Start is called before the first frame update
        void Awake()
        {
            Init();
        }
        void Init()
        {
            material = GetComponentInChildren<Renderer>().material;

        }
        public void SetStartColor(ColorHdr colorHdr)
        {
            if (material == null)
                Init();
            var startColor = colorHdr.color*Mathf.Pow(2, colorHdr.intensity);
            startColor.a = 1;
            material.SetColor("_Color", startColor);
        }
        public void Appear(float duration)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(FadeRoutine(duration,0,1));
        }
        public void Disappear(float duration)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);
            if(duration == 0)
            {
                DisappearInstantly();
                return;
            }
            fadeRoutine = StartCoroutine(FadeRoutine(duration,1,0));
        }
        void DisappearInstantly()
        {
            var color = material.GetColor("_Color");

            FadeExecute(1, color, 1, 0, 1);
        }
        IEnumerator FadeRoutine(float duration, float startAlpha, float targetAlpha)
        {

            var color = material.GetColor("_Color");
            var currentAlpha = color.a;
            var startFill = startAlpha;

            float t = 1 - Mathf.Abs(currentAlpha - targetAlpha);

            while (t < 1)
            {
                FadeExecute(t, color, startAlpha, targetAlpha, startFill);

                t += Time.deltaTime / duration;
                yield return null;
            }
            FadeExecute(1, color, startAlpha, targetAlpha, startFill);
        }
        void FadeExecute(float t,Color color,float startAlpha, float targetAlpha, float startFill)
        {
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            material.SetColor("_Color", color);

            var lineLength = Mathf.Lerp(startFill, targetAlpha, t);
            material.SetFloat("_Fill", lineLength);
        }
    }
}
