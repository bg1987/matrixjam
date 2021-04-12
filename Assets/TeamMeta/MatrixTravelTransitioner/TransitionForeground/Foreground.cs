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
        [SerializeField] Image edgesDissolveImage;
        [SerializeField, Range(0,1)] float edgesDissolveColor1Value = 0.4f;
        [SerializeField, Range(0,1)] float edgesDissolveColor2MaxValue = 0.4f;
        Material edgesDissolveMat;
        // Start is called before the first frame update
        void Awake()
        {
            container.SetActive(false);
        }
        public void Appear()
        {
            rawImage.material.SetFloat("_Dissolve", 0);
            edgesDissolveImage.material.SetFloat("_Dissolve", 0);
            container.SetActive(true);
        }
        public void Disappear(float duration, ColorHdr colorHdr1, ColorHdr colorHdr2)
        {
            StopAllCoroutines();
            StartCoroutine(DisappearRoutine(duration, colorHdr1, colorHdr2));
        }
        IEnumerator DisappearRoutine(float duration, ColorHdr colorHdr1, ColorHdr colorHdr2)
        {
            Material material = rawImage.material;
            if(edgesDissolveMat== null)
            {
                edgesDissolveMat = new Material(edgesDissolveImage.material);
                edgesDissolveImage.material = edgesDissolveMat;
            }
            float edgeColorAlpha = 43f / 255f;
            Color.RGBToHSV(colorHdr1.color, out float h1, out float s1, out float v1);
            Color color1 = Color.HSVToRGB(h1, s1, edgesDissolveColor1Value);
            color1.a = edgeColorAlpha;

            Color.RGBToHSV(colorHdr2.color, out float h2, out float s2, out float v2);
            Color color2;
            if (v2 > edgesDissolveColor2MaxValue)
            {
                color2 = Color.HSVToRGB(h2, s2, edgesDissolveColor2MaxValue);

            }
            else
                color2 = colorHdr2.color;

            color2.a = edgeColorAlpha;

            edgesDissolveMat.SetColor("_EdgeColor1", color1);
            edgesDissolveMat.SetColor("_EdgeColor2", color2);
            float count = 0;
            //Debug.Break();
            yield return null;
            //Debug.Log(count / duration);
            while (count < duration)
            {
                float t = count / duration;
                material.SetFloat("_Dissolve", t);
                edgesDissolveMat.SetFloat("_Dissolve", t);
                
                count += Time.deltaTime;
                yield return null;

            }
            material.SetFloat("_Dissolve", 1);
            edgesDissolveMat.SetFloat("_Dissolve", 1);

            yield return null;

            material.SetFloat("_Dissolve", 0);
            edgesDissolveMat.SetFloat("_Dissolve", 0);
            container.SetActive(false);
        }
    }
}
