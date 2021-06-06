using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class TutorialStep : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;
        [SerializeField] TmpFader textFader;
        public bool isCompleted = false;
        public bool isInProgress = false;
        [SerializeField] private Color halfwayCompleteFadeColor = Color.blue;
        [SerializeField] private Color completeFadeColor = Color.blue;
        private Coroutine appearRoutine;

        // Start is called before the first frame update
        void Start()
        {
            //Disappear(0);
        }
        public void Appear(float duration, float characterDuration,float delay)
        {
            if (isInProgress || isCompleted)
                return;
            isInProgress = true;

            appearRoutine = StartCoroutine(AppearRoutine(duration,characterDuration,delay));
        }
        IEnumerator AppearRoutine(float duration, float characterDuration, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (isCompleted)
                yield break;
            textFader.FadeInLines(duration, characterDuration);
            
            yield return new WaitForSeconds(duration+ characterDuration);
            appearRoutine = null;

        }
        public void DisappearImmediately()
        {
            isInProgress = false;
            StopAllCoroutines();
            appearRoutine = null;
            textFader.FadeOutLines(0, 0);

        }

        public void Disappear(float duration, float characterDuration)
        {
            if (isCompleted)
                return;
            isInProgress = false;
            if (appearRoutine!=null)
            {
                StopCoroutine(appearRoutine);
                appearRoutine = null;
            }
            textFader.FadeOutLines(duration,characterDuration);
        }
        public void Complete(float duration, float characterDuration)
        {
            if(isInProgress)
            {
                StartCoroutine(CompleteRoutine(duration, characterDuration));
            }
            isCompleted = true;
        }
        IEnumerator CompleteRoutine(float duration, float characterDuration)
        {
            while (appearRoutine!=null)
                yield return null;
            textFader.FadeOutLines(duration, characterDuration);

            float t = 0;
            float colorChangeDuration = (duration+ characterDuration) / 2f;
            while (t<1)
            {
                text.color = Color.Lerp(Color.white, halfwayCompleteFadeColor, t);

                t += Time.deltaTime / colorChangeDuration;
                yield return null;
            }
            text.color = halfwayCompleteFadeColor;
            t = 0;
            while (t < 1)
            {
                text.color = Color.Lerp(halfwayCompleteFadeColor, completeFadeColor, t);

                t += Time.deltaTime / colorChangeDuration;
                yield return null;
            }
            text.color = completeFadeColor;
        }

    }
}
