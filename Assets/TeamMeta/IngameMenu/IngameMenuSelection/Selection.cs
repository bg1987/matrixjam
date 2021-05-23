using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Selection : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TriangleSpriteUI triangleSpriteUI;
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] EventTrigger eventTrigger;
        [SerializeField] Color flavorColor;
        [SerializeField] List<Object> selectionSelectListeners = new List<Object>();
        [SerializeField] List<Object> selectionAppearListeners = new List<Object>();
        Material material;

        float randomSeed = 1;
        float newRandomSeed = 1;

        [Header("Appearance")]
        [SerializeField] float blubBorderAppearValue = 0.03f;
        [SerializeField] float blubBorderDisappearValue = 0.25f;
        [SerializeField] float blubBorderFadeColorThreshold = 0.9f;
        [SerializeField] AnimationCurve blubBorderAppearCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] AnimationCurve blubBorderNoiseStrengthCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] AnimationCurve textAppearCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] float blubBorderNoiseScroll = 1f;
        private Coroutine changeAppearStateRoutine;

        [Header("Hover")]
        [SerializeField] float hoverBorderAlphaEnd = 0.7f;
        [SerializeField] float hoverAppearDuration = 0.2f;
        private bool isHovered = false;
        private Coroutine changeHoverStateRoutine;

        [Header("Flash")]
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private Color originalColor = Color.black;
        [SerializeField] private Color flashColor = Color.blue;

        //[Header("Select")]
        private bool isSelected = false;

        private void Awake()
        {
            material = image.material;
        }
        private void Start()
        {
            Init();
        }
        public void Init()
        {
            material = image.material;
            Color borderColor = flavorColor;
            material.SetColor("_BlubBorderColor", borderColor);
            var textColor = flavorColor;
            textColor.a = text.color.a;
            text.color = textColor;
            randomSeed = Random.value;
            newRandomSeed = randomSeed;

            DisappearBorder(0);
        }
        public void SetInteractable(bool isInteractable)
        {
            button.interactable = isInteractable;
            image.raycastTarget = isInteractable;
        }
        public void Appear(float duration)
        {
            foreach (var listenerObject in selectionAppearListeners)
            {
                var listener = listenerObject as ISelectionAppearListener;
                listener.Appear(duration);
            }
            ChangeAppearState(duration, 0, 1);
        }
        public void Disappear(float duration, System.Action<Selection> onComplete)
        {
            foreach (var listenerObject in selectionAppearListeners)
            {
                var listener = listenerObject as ISelectionAppearListener;
                if(duration == 0)
                    listener.DisappearImmediately();
                else
                    listener.Disappear(duration);
            }
            ExitSelect();
            ChangeAppearState(duration, 1, 0, onComplete);
        }
        void ChangeAppearState(float duration, float start, float end, System.Action<Selection> onComplete = null)
        {
            if (changeAppearStateRoutine != null)
                StopCoroutine(changeAppearStateRoutine);
            if (duration == 0)
            {
                ExecuteAppear(end);
                if (onComplete!=null)
                    onComplete.Invoke(this);
                return;
            }
            changeAppearStateRoutine = StartCoroutine(ChangeAppearStateRoutine(duration, start, end, onComplete));
        }
        IEnumerator ChangeAppearStateRoutine(float duration, float start, float end, System.Action<Selection> onComplete = null)
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
            changeAppearStateRoutine = null;
            if (onComplete != null)
                onComplete.Invoke(this);
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

            var blubBorderColor = material.GetColor("_BlubBorderColor");
            var borderColor = flavorColor;

            if (t< blubBorderFadeColorThreshold)
            {
                borderColor.a = Mathf.Lerp(0, flavorColor.a, t);
                material.SetColor("_BlubBorderColor", borderColor);
            }
            else
            {
                if (!isHovered && changeHoverStateRoutine==null)
                {
                    borderColor.a = Mathf.Lerp(flavorColor.a, 0, Mathf.InverseLerp(blubBorderFadeColorThreshold, 1, t));
                    material.SetColor("_BlubBorderColor", borderColor);
                }
            }

            float blubBorderWidth = Mathf.Lerp(blubBorderDisappearValue, blubBorderAppearValue, blubBorderAppearCurve.Evaluate(t));
            material.SetFloat("_BlubBorderWidth", blubBorderWidth);

            Vector2 a = triangleSpriteUI.A;
            Vector2 b = triangleSpriteUI.B;
            Vector2 c = triangleSpriteUI.C;

            float noiseScroll = randomSeed + blubBorderNoiseScroll*t;
            float noiseStrength = blubBorderNoiseStrengthCurve.Evaluate(1 - blubBorderAppearT) * (1 + randomSeed * 0.2f);
            a.x += (Mathf.PerlinNoise(noiseScroll * a.x, 0) * 2 - 1) * noiseStrength;
            a.y += (Mathf.PerlinNoise(noiseScroll * a.y, 0) * 2 - 1) * noiseStrength;
            material.SetVector("_A", a);

            b.x += (Mathf.PerlinNoise(noiseScroll * b.x, 0) * 2 - 1) * noiseStrength;
            b.y += (Mathf.PerlinNoise(noiseScroll * b.y, 0) * 2 - 1) * noiseStrength;
            material.SetVector("_B", b);
            c.x += (Mathf.PerlinNoise(noiseScroll * c.x, 0) * 2 - 1) * noiseStrength;
            c.y += (Mathf.PerlinNoise(noiseScroll * c.y, 0) * 2 - 1) * noiseStrength;
            material.SetVector("_C", c);
        }
        public void AppearBorder(float duration)
        {
            ChangeBorderColor(duration, 0, 1);
        }
        public void DisappearBorder(float duration)
        {
            ChangeBorderColor(duration, 1, 0);
        }

        public void Flash()
        {
            if (button.interactable == false)
                return;

            StartCoroutine(FlashRoutine(flashDuration));
        }
        IEnumerator FlashRoutine(float duration)
        {
            float t = 0;
            while (t < 1)
            {
                ExecuteFlash(t);

                t += Time.deltaTime / duration;
                yield return null;
            }
            ExecuteFlash(1);
        }
        void ExecuteFlash(float t)
        {
            float flashT = Mathf.Abs(Mathf.Abs((t * 2) - 1)-1);
            var targetColor = Color.Lerp(originalColor, flashColor, flashT);
            material.SetColor("_Color", targetColor);
        }
        void ChangeBorderColor(float duration, float start, float end)
        {
            if (changeHoverStateRoutine != null)
                StopCoroutine(changeHoverStateRoutine);
            if (duration == 0)
            {
                ExecuteHover(end);
                return;
            }
            changeHoverStateRoutine = StartCoroutine(ChangeBorderColorRoutine(duration, start, end));
        }
        IEnumerator ChangeBorderColorRoutine(float duration, float start, float end)
        {
            float effectProgress = material.GetColor("_BlubBorderColor").a;

            float t = Mathf.InverseLerp(start, end, effectProgress);
            while (t < 1)
            {
                float hoverT = Mathf.Lerp(start, end, t);
                ExecuteHover(hoverT);

                float timeStep = Time.deltaTime / duration;
                t += timeStep;

                yield return null;
            }
            ExecuteHover(end);
            changeHoverStateRoutine = null;
        }
        void ExecuteHover(float t)
        {
            var blubBorderColor = flavorColor;
            float blubBorderAlpha = Mathf.Lerp(0, hoverBorderAlphaEnd, t);
            blubBorderColor.a = blubBorderAlpha;
            material.SetColor("_BlubBorderColor", blubBorderColor);
        }
        public void EnterHover()
        {
            Debug.Log("Enter hover");

            isHovered = true;

            if (isSelected)
                return;

            AppearBorder(hoverAppearDuration);
        }
        public void ExitHover()
        {
            Debug.Log("Exit hover");
            
            isHovered = false;

            if (isSelected)
                return;

            DisappearBorder(hoverAppearDuration);
        }
        public void EnterSelect()
        {
            Debug.Log("Enter hover");
            if (isSelected)
                return;
            isSelected = true;
            foreach (var listenerObject in selectionSelectListeners)
            {
                var listener = listenerObject as ISelectionSelectListener;
                listener.Select();
            }
            AppearBorder(hoverAppearDuration);
        }
        public void ExitSelect()
        {
            if (!isSelected)
                return;
            isSelected = false;

            foreach (var listenerObject in selectionSelectListeners)
            {
                var listener = listenerObject as ISelectionSelectListener;
                listener.Unselect();
            }

            DisappearBorder(hoverAppearDuration);

            if(isHovered)
            {
                EnterHover();
            }
        }
        public void ExitSelectImmediately()
        {
            if (!isSelected)
                return;
            isSelected = false;

            foreach (var listenerObject in selectionSelectListeners)
            {
                var listener = listenerObject as ISelectionSelectListener;
                listener.UnselectImmediately();
            }
            DisappearBorder(0);
            if (isHovered)
            {
                EnterHover();
            }
        }

        public void Select()
        {

        }
        public void ToggleSelect()
        {
            if (isSelected)
                ExitSelect();
            else
                EnterSelect();
        }
    }
}
