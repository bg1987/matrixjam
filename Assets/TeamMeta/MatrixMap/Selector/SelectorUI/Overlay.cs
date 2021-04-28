using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class Overlay : MonoBehaviour
    {
        [SerializeField] FillScreen fillScreen;
        [SerializeField] GameObject model;
        [SerializeField] float zPositionWhenActivated = -1;
        [SerializeField] Collider _collider;

        Material material;
        Color originColor;

        private Coroutine fadeRoutine;

        // Start is called before the first frame update
        void Awake()
        {
            material = model.GetComponent<Renderer>().material;
            originColor = material.GetColor("_Color");
            fillScreen.Fill();
            Deactivate();
            BringToFront();

        }
        public void Activate()
        {
            _collider.enabled = true;
            fillScreen.enabled = true;
            //Appear();
            BringToFront();
        }
        public void Deactivate()
        {
            _collider.enabled = false;
            Disappear(0);
            fillScreen.enabled = false;
        }
        public void Appear(float duration)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(FadeRoutine(duration, 0, originColor.a));
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
            fadeRoutine = StartCoroutine(FadeRoutine(duration, originColor.a, 0));
        }
        void DisappearInstantly()
        {
            FadeExecute(1, originColor.a, 0);
        }
        IEnumerator FadeRoutine(float duration, float startAlpha, float targetAlpha)
        {

            var color = material.GetColor("_Color");
            var currentAlpha = color.a;

            float t = Mathf.InverseLerp(startAlpha, targetAlpha, currentAlpha);

            var fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            while (t < 1)
            {
                float fadeT = fadeCurve.Evaluate(t);

                FadeExecute(fadeT, startAlpha, targetAlpha);
                t += Time.smoothDeltaTime / duration;
                yield return null;
            }
            FadeExecute(1, startAlpha, targetAlpha);

        }
        void FadeExecute(float t, float startAlpha, float targetAlpha)
        {
            var color = originColor;
            color.a = Mathf.SmoothStep(startAlpha, targetAlpha, t);
            material.SetColor("_Color", color);
        }
        void BringToFront()
        {
            var targetPosition = transform.position;
            targetPosition.z = zPositionWhenActivated;
            transform.position = targetPosition;
        }
    }
}
