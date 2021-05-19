using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Selection : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI text;
        private Coroutine changeAppearStateRoutine;
        public void SetInteractable(bool isInteractable)
        {
            button.interactable = isInteractable;
        }
        public void Appear(float duration)
        {
            ChangeAppearState(duration, 0, 1);
        }
        public void Disappear(float duration)
        {
            ChangeAppearState(duration, 1, 0);
        }
        void ChangeAppearState(float duration, float start, float end)
        {
            if (changeAppearStateRoutine != null)
                StopCoroutine(changeAppearStateRoutine);
            if (duration == 0)
            {
                ExecuteAppear(end);
                return;
            }
            changeAppearStateRoutine = StartCoroutine(ChangeAppearStateRoutine(duration, start, end));
        }
        IEnumerator ChangeAppearStateRoutine(float duration, float start, float end)
        {

            //float appearProgress = material.GetFloat("_AppearProgress");
            float appearProgress = image.color.a;

            float t = Mathf.InverseLerp(start, end, appearProgress);

            while (t < 1)
            {
                float appearT = Mathf.Lerp(start, end, t);
                ExecuteAppear(appearT);

                t += Time.deltaTime / duration;
                yield return null;
            }
            ExecuteAppear(end);
        }
        void ExecuteAppear(float t)
        {
            Color imageColor = image.color;
            imageColor.a = t;
            image.color = imageColor;

            Color textColor = text.color;
            textColor.a = t;
            text.color = textColor;
            //material.SetFloat("_AppearProgress", t);
        }
    }
}
