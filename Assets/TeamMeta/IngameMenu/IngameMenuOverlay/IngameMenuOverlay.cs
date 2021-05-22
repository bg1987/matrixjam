using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta
{
    public class IngameMenuOverlay : MonoBehaviour
    {
        [SerializeField] Image image;
        Material material;
        private Coroutine changeAppearStateRoutine;
        private Coroutine deactivateRoutine;
        
        // Start is called before the first frame update
        void Awake()
        {
            material = new Material(image.material);
            image.material = material;
        }
        public void Activate()
        {
            gameObject.SetActive(true);
            if (deactivateRoutine != null)
                StopCoroutine(deactivateRoutine);
        }
        public void Deactivate(float delay)
        {
            if (deactivateRoutine != null)
                StopCoroutine(deactivateRoutine);
            if (delay == 0)
            {
                gameObject.SetActive(false);
                return;
            }

            deactivateRoutine = StartCoroutine(DeactivateRoutine(delay));
        }
        IEnumerator DeactivateRoutine(float delay)
        {
            yield return null;
            yield return new WaitForSeconds(delay);
            yield return null;

            gameObject.SetActive(false);
            ExecuteAppear(0);
            deactivateRoutine = null;
        }
        public void SetInteractable(bool isInteractable)
        {
            image.raycastTarget = isInteractable;
        }
        public void Appear(float duration)
        {
            ChangeAppearState(duration, 0, 1);
        }
        public void Disappear(float duration)
        {
            ChangeAppearState(duration, 1, 0);
        }
        public void CoverWholeScreen(float duration)
        {
            CoverWholeScreen(duration, null);
        }
        public void CoverWholeScreen(float duration, AnimationCurve animationCurve)
        {
            ChangeAppearState(duration, 0, 2, animationCurve);
        }
        public void UncoverWholeScreen(float duration)
        {
            UncoverWholeScreen(duration, null);
        }
        public void UncoverWholeScreen(float duration, AnimationCurve animationCurve)
        {
            ChangeAppearState(duration, 2, 0, animationCurve);
        }
        void ChangeAppearState(float duration,float start, float end, AnimationCurve animationCurve = null)
        {
            if (changeAppearStateRoutine != null)
                StopCoroutine(changeAppearStateRoutine);
            if (duration == 0)
            {
                ExecuteAppear(end);
                return;
            }
            changeAppearStateRoutine = StartCoroutine(ChangeAppearStateRoutine(duration, start, end, animationCurve));
        }
        IEnumerator ChangeAppearStateRoutine(float duration, float start, float end, AnimationCurve animationCurve = null)
        {

            float appearProgress = material.GetFloat("_AppearProgress");

            float t = Mathf.InverseLerp(start, end, appearProgress);
            while (t < 1)
            {
                float effectT;
                if (animationCurve == null)
                    effectT = t;
                else
                    effectT = animationCurve.Evaluate(t);

                float appearT = Mathf.Lerp(start, end, effectT);

                ExecuteAppear(appearT);

                t += Time.deltaTime / duration;
                yield return null;
            }
            ExecuteAppear(end);
            changeAppearStateRoutine = null;
            if(end == 0)
                gameObject.SetActive(false);
        }
        void ExecuteAppear(float t)
        {
            material.SetFloat("_AppearProgress", t);
        }

    }
}
