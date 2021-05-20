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
        [SerializeField] TriangleSpriteUI triangleSpriteUI;
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI text;

        [SerializeField] Color flavorColor;
        private Coroutine changeAppearStateRoutine;
        Material material;

        [Header("Appearance")]
        [SerializeField] float blubBorderAppearValue = 0.03f;
        [SerializeField] float blubBorderDisappearValue = 0.25f;
        [SerializeField] AnimationCurve blubBorderAppearCurve = AnimationCurve.Linear( 0,0,1,1);
        [SerializeField] AnimationCurve blubBorderNoiseStrengthCurve = AnimationCurve.Linear( 0,0,1,1);
        [SerializeField] AnimationCurve textAppearCurve = AnimationCurve.Linear( 0,0,1,1);
        [SerializeField] float blubBorderNoiseScroll = 1f;
        float randomSeed = 1;
        float newRandomSeed = 1;
        private void Awake()
        {
            material = image.material;
            Color borderColor = flavorColor;
            material.SetColor("_BlubBorderColor", borderColor);

            text.color = flavorColor;
            randomSeed = Random.value;
            newRandomSeed = randomSeed;
        }
        public void SetInteractable(bool isInteractable)
        {
            button.interactable = isInteractable;
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
            newRandomSeed = Random.value;
            float appearProgress = image.color.a;

            float t = Mathf.InverseLerp(start, end, appearProgress);
            float randomSeedT = 0;
            while (t < 1)
            {
                float appearT = Mathf.Lerp(start, end, t);
                ExecuteAppear(appearT);

                randomSeed = Mathf.Lerp(randomSeed, newRandomSeed, randomSeedT);

                float timeStep = Time.deltaTime / duration;
                t += timeStep;
                randomSeedT += timeStep;

                yield return null;
            }
            ExecuteAppear(end);
        }
        void ExecuteAppear(float t)
        {
            Color imageColor = image.color;
            float blubBorderAppearT = blubBorderAppearCurve.Evaluate(t);
            imageColor.a = blubBorderAppearT;
            image.color = imageColor;

            Color textColor = text.color;
            textColor.a = textAppearCurve.Evaluate(t);
            text.color = textColor;

            var borderColor = flavorColor;
            borderColor.a = Mathf.Lerp(0, flavorColor.a, t);
            material.SetColor("_BlubBorderColor", borderColor);

            float blubBorderWidth = Mathf.Lerp(blubBorderDisappearValue, blubBorderAppearValue, blubBorderAppearCurve.Evaluate(t));
            material.SetFloat("_BlubBorderWidth", blubBorderWidth);

            Vector2 a = triangleSpriteUI.A;
            Vector2 b = triangleSpriteUI.B;
            Vector2 c = triangleSpriteUI.C;

            float noiseScroll = randomSeed + blubBorderNoiseScroll*Time.deltaTime;
            float noiseStrength = blubBorderNoiseStrengthCurve.Evaluate(1- blubBorderAppearT)*(1+ randomSeed*0.2f);
            a.x += (Mathf.PerlinNoise(noiseScroll, 0) * 2 - 1) * noiseStrength;
            a.y += (Mathf.PerlinNoise(noiseScroll, 0) * 2 - 1) * noiseStrength;
            material.SetVector("_A", a);

            b.x += (Mathf.PerlinNoise(noiseScroll, 0) * 2 - 1) * noiseStrength;
            b.y += (Mathf.PerlinNoise(noiseScroll, 0) * 2 - 1) * noiseStrength;
            material.SetVector("_B", b);
            c.x += (Mathf.PerlinNoise(noiseScroll, 0) * 2 - 1) * noiseStrength;
            c.y += (Mathf.PerlinNoise(noiseScroll, 0) * 2 - 1) * noiseStrength;
            material.SetVector("_C", c);
        }
    }
}
