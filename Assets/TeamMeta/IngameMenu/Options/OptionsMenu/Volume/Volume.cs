using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Volume : MonoBehaviour, ISelectionAppearListener
    {
        [SerializeField] Slider slider;
        [SerializeField] Image backgroundImage;
        [SerializeField] Image fillImage;
        [SerializeField] Image handleImage;

        [Header("Slider Appearance")]
        [SerializeField] Color appearBackgroundColor = Color.blue;
        [SerializeField] Color appearHandleColor = Color.blue;
        [SerializeField] List<Color> matrixColors = new List<Color>();
        [SerializeField] private AnimationCurve sliderAppearCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private AnimationCurve selectionAppearCurve = AnimationCurve.Linear(0, 0, 1, 1);

        float appearProgress = 0f;
        private Coroutine changeAppearStateRoutine;

        private void Awake()
        {
            backgroundImage.color = appearBackgroundColor;

            RefreshSliderFill();
            UpdateFillColor();
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

            //float t = Mathf.Lerp(start, end, backgroundImage.color.a);
            float t = Mathf.InverseLerp(start, end, appearProgress);
            while (t < 1)
            {
                float appearT = Mathf.Lerp(start, end, t);

                ExecuteAppear(appearT);

                float timeStep = Time.deltaTime / duration;
                t += timeStep;

                yield return null;
            }
            ExecuteAppear(end);
            changeAppearStateRoutine = null;
        }
        void ExecuteAppear(float t)
        {
            appearProgress = selectionAppearCurve.Evaluate(t);
            //Debug.Log(appearProgress);
            t = sliderAppearCurve.Evaluate(t);

            backgroundImage.color = CalculateImageAppearColor(t, 0, appearBackgroundColor.a, backgroundImage);
            fillImage.color = CalculateImageAppearColor(t, 0, 1, fillImage);
            handleImage.color = CalculateImageAppearColor(t, 0, appearHandleColor.a, handleImage);

        }
        Color CalculateImageAppearColor(float t, float start, float end, Image image)
        {
            Color imageColor = image.color;
            imageColor.a = Mathf.Lerp(start, end, t);

            return imageColor;
        }
        public void UpdateFillColor()
        {
            var fillImageColor = CalculateMatrixColorBasedOnFill(slider.value);
            fillImageColor.a = fillImage.color.a;
            fillImage.color = fillImageColor;

            fillImageColor.a = handleImage.color.a;
            handleImage.color = fillImageColor;
        }
        public void UpdateVolume()
        {
            AudioListener.volume = slider.value;
            UpdateFillColor();
        }
        public void RefreshSliderFill()
        {
            slider.value = AudioListener.volume;
        }
        Color CalculateMatrixColorBasedOnFill(float value)
        {
            float colorsCount = matrixColors.Count;
            Color matrixColor = matrixColors[0];
            for (int i = 1; i < colorsCount; i++)
            {
                float step = i / (colorsCount-1);
                float preStep = (i-1f) / (colorsCount-1);
                if (value<= step)
                {
                    float lerpT = Mathf.InverseLerp(preStep, step, value);

                    matrixColor = Color.Lerp(matrixColors[i - 1], matrixColors[i], lerpT);
                    break;
                }
            }
            return matrixColor;
        }
        public void DisappearImmediately()
        {
            ExecuteAppear(0);
        }
    }
}
