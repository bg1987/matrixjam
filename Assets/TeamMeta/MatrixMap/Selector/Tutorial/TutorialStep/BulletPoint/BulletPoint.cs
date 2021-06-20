using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class BulletPoint : MonoBehaviour
    {
        [SerializeField] Renderer modelRenderer;
        [SerializeField] Renderer shadowRenderer;
        [SerializeField] AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0,0,1,1);

        Material modelMaterial;
        Material shadowMaterial;

        private float appearProgress = 0;

        private Coroutine changeAppearStateRoutine;
        private Coroutine changeColorRoutine;

        private void Awake()
        {
            modelMaterial = modelRenderer.material;
            shadowMaterial = shadowRenderer.material;

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

            if (duration == 0 || GetAppearProgress() == end)
            {
                ExecuteAppear(end);

                return;
            }
            changeAppearStateRoutine = StartCoroutine(ChangeAppearStateRoutine(duration, start, end));
        }

        private float GetAppearProgress()
        {
            return appearProgress;
        }
        IEnumerator ChangeAppearStateRoutine(float duration, float start, float end)
        {
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
            appearProgress = t;
            float alphaT = alphaCurve.Evaluate(t);

            Color modelColor = modelMaterial.GetColor("_Color");
            modelColor.a = alphaT;
            modelMaterial.SetColor("_Color",modelColor);
            float modelWidth = Mathf.Lerp(0, modelRenderer.transform.localScale.x, t);
            modelMaterial.SetFloat("_Width", modelWidth);

            Color shadowColor = shadowMaterial.GetColor("_Color");
            shadowColor.a = alphaT;
            shadowMaterial.SetColor("_Color", shadowColor);
            float shadowWidth = Mathf.Lerp(0, shadowRenderer.transform.localScale.x, t);
            shadowMaterial.SetFloat("_Width", shadowWidth);
        }

        public void ChangeColor(float duration, Color color)
        {
            if (changeColorRoutine != null)
                StopCoroutine(changeColorRoutine);

            if (duration == 0)
            {
                ExecuteColorChange(1, color, color);

                return;
            }

            changeColorRoutine = StartCoroutine(ColorChangeRoutine(duration, color));
        }
        IEnumerator ColorChangeRoutine(float duration, Color color)
        {
            var startColor = modelRenderer.material.GetColor("_Color");
            float t = 0;

            while (t < 1)
            {
                ExecuteColorChange(t, startColor, color);

                float timeStep = Time.deltaTime / duration;
                t += timeStep;

                yield return null;
            }
            ExecuteColorChange(1, startColor, color);

            changeAppearStateRoutine = null;
        }
        void ExecuteColorChange(float t,Color startColor, Color targetColor)
        {
            float alpha = modelMaterial.GetColor("_Color").a;

            Color color = Color.Lerp(startColor, targetColor,t);
            color.a = alpha;
            modelMaterial.SetColor("_Color",color);
        }
    }
}
