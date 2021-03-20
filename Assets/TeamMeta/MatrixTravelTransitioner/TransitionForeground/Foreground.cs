using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta.MatrixTravelTransition
{
    public class Foreground : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [SerializeField] RawImage rawImage;

        // Start is called before the first frame update
        void Awake()
        {
            container.SetActive(false);
        }
        public void Appear()
        {
            rawImage.material.SetFloat("_Dissolve", 0);
            container.SetActive(true);
        }
        public void Disappear(float duration)
        {
            StopAllCoroutines();
            StartCoroutine(DisappearRoutine(duration));
        }
        IEnumerator DisappearRoutine(float duration)
        {
            Material material = rawImage.material;
            float count = 0;
            //Debug.Break();
            yield return null;
            //Debug.Log(count / duration);
            while (count < duration)
            {
                float t = count / duration;
                material.SetFloat("_Dissolve", t);
                count += Time.unscaledDeltaTime;
                yield return null;

            }
            material.SetFloat("_Dissolve", 1);
            yield return null;

            material.SetFloat("_Dissolve", 0);
            container.SetActive(false);
        }
    }
}
