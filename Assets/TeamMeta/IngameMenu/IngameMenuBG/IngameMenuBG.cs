using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta
{
    public class IngameMenuBG : MonoBehaviour
    {
        [SerializeField] Image image;
        Material material;
        private Coroutine changeAppearStateRoutine;
        
        // Start is called before the first frame update
        void Awake()
        {
            material = image.material;
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
        void ChangeAppearState(float duration,float start, float end)
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

            float appearProgress = material.GetFloat("_AppearProgress");

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
            material.SetFloat("_AppearProgress", t);
        }
    }
}
