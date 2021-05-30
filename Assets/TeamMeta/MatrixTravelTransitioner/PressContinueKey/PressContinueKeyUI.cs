using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixTravelTransition
{
    public class PressContinueKeyUI : MonoBehaviour
    {

        [SerializeField] TextMeshPro text;

        [SerializeField] float appearDuration = 0.5f;
        [SerializeField] float disappearDuration = 0.5f;
        [SerializeField] float lingerDuration = 2f;

        private bool isTextLingering = false;

        Coroutine appearRoutine;
        Coroutine disappearRoutine;
        Coroutine lingerRoutine;

        public void Reset()
        {
            StopAllCoroutines();
            ExecuteDisappear(1);
            isTextLingering = false;
        }
        public void SetKey(KeyCode keyCode)
        {
            text.SetText("Press " +"<i>"+ keyCode.ToString()+ "</i>" + " To Continue");
        }
        public void Appear()
        {
            if (lingerRoutine != null)
                StopCoroutine(lingerRoutine);
            lingerRoutine = StartCoroutine(LingerRoutine());

            if (appearRoutine != null)
                return;

            if (disappearRoutine != null)
            {
                StopCoroutine(disappearRoutine);
                disappearRoutine = null;
            }

            appearRoutine = StartCoroutine(AppearRoutine());
        }
        IEnumerator AppearRoutine()
        {
            float t = text.alpha;
            while (t < 1)
            {
                t += Time.deltaTime / appearDuration;
                text.alpha = t;
                yield return null;
            }
            text.alpha = 1;

            appearRoutine = null;
        }
        IEnumerator LingerRoutine()
        {
            isTextLingering = true;
            yield return new WaitForSeconds(lingerDuration);
            isTextLingering = false;
            lingerRoutine = null;

        }
        public void StopLingering()
        {
            if (lingerRoutine!= null)
            {
                StopCoroutine(lingerRoutine);
                lingerRoutine = null;
                isTextLingering = false;
            }
        }
        public void Disappear()
        {
            if (isTextLingering)
                return;
            if (disappearRoutine != null)
                return;

            disappearRoutine = StartCoroutine(DisappearRoutine());
        }
        public void DisappearInstantly()
        {
            ExecuteDisappear(1);
        }
        void ExecuteDisappear(float t)
        {
            text.alpha = 1 - t;
        }
        IEnumerator DisappearRoutine()
        {
            float t = 1 - text.alpha;
            while (t < 1)
            {
                t += Time.deltaTime / disappearDuration;
                ExecuteDisappear(t);
                yield return null;
            }
            ExecuteDisappear(1);

            disappearRoutine = null;
        }
        public bool IsVisible()
        {
            return text.alpha == 0 ? false : true;
        }
    }
}
