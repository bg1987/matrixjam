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
        private Coroutine appearRoutine;

        // Start is called before the first frame update
        void Start()
        {
            //Disappear(0);
        }
        public void Appear(float duration, float characterDuration,float delay)
        {
            if (isInProgress)
                return;
            isInProgress = true;

            StopAllCoroutines();

            appearRoutine = StartCoroutine(AppearRoutine(duration,characterDuration,delay));
        }
        IEnumerator AppearRoutine(float duration, float characterDuration, float delay)
        {
            yield return new WaitForSeconds(delay);
            textFader.FadeInLines(duration, characterDuration);
        }

        public void Disappear(float duration, float characterDuration)
        {
            isInProgress = false;

            StopAllCoroutines();
            textFader.FadeOutLines(duration,characterDuration);
        }
        public void Complete(float duration, float characterDuration)
        {
            if(isInProgress)
                Disappear(duration, characterDuration);

            isInProgress = false;
            isCompleted = true;
        }
    }
}
