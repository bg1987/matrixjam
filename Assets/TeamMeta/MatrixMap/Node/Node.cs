using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class Node : MonoBehaviour
    {
        [SerializeField] GameObject model;
        [SerializeField] int index;

        [SerializeField] ColorHdr colorHdr1;
        [SerializeField] ColorHdr colorHdr2;
        public ColorHdr ColorHdr1 { get=> colorHdr1; }
        public ColorHdr ColorHdr2 { get=> colorHdr2; }

        Material modelMaterial;

        public List<Edge> startPortActiveEdges = new List<Edge>();

        List<Edge> startPortEdges = new List<Edge>();
        List<Edge> endPortEdges = new List<Edge>();

        [Header("Glow")]
        [SerializeField] float glowAddedIntensity1 = 1f;
        [SerializeField] float glowAddedIntensity2 = 1f;
        // Start is called before the first frame update
        void Awake()
        {
            modelMaterial = model.GetComponent<Renderer>().material;
            modelMaterial.SetFloat("_Seed", index);

            SetModelColors(colorHdr1, colorHdr2);
        }
        public void SetIndex(int value)
        {
            index = value;
        }
        public void SetModelColors(ColorHdr colorHdr1, ColorHdr colorHdr2)
        {
            this.colorHdr1 = colorHdr1;
            this.colorHdr2 = colorHdr2;
            if (!modelMaterial)
                return;
            modelMaterial.SetColor("_Color1", colorHdr1.color);
            modelMaterial.SetColor("_Color2", colorHdr2.color);
            modelMaterial.SetFloat("_ColorIntensity1", colorHdr1.intensity);
            modelMaterial.SetFloat("_ColorIntensity2", colorHdr2.intensity);
        }
        public void AddToStartPortEdges(Edge edge)
        {
            startPortEdges.Add(edge);
        }
        public void AddToStartPortActiveEdges(Edge edge)
        {
            startPortActiveEdges.Add(edge);
        }
        public void AddToEndPortEdges(Edge edge)
        {
            endPortEdges.Add(edge);
        }
        public void SetModelRadius(float radius)
        {
            model.transform.localScale *= radius;
        }
        public void Appear(float duration, float delay)
        {
            StartCoroutine(AppearRoutine(duration, delay));
        }
        IEnumerator AppearRoutine(float duration, float delay)
        {
            float count = 0;

            yield return new WaitForSeconds(delay);

            Color materialColor;
            while (count < duration)
            {
                materialColor = modelMaterial.color;
                materialColor.a = Mathf.Lerp(0, 1, count / duration);
                modelMaterial.color = materialColor;

                //count += Time.fixedDeltaTime;
                //yield return new WaitForFixedUpdate();

                count += Time.deltaTime;
                yield return null;
            }
            materialColor = modelMaterial.color;
            materialColor.a = 1;
            modelMaterial.color = materialColor;
        }
        public void Glow(float duration, float delay)
        {
            StartCoroutine(GlowRoutine(duration, delay));
        }
        IEnumerator GlowRoutine(float duration, float delay)
        {
            float count = 0;

            yield return new WaitForSeconds(delay);

            float originIntensity1 = colorHdr1.intensity;
            float originIntensity2 = colorHdr2.intensity;

            while (count < duration)
            {

                float intensity1 = Mathf.Lerp(originIntensity1, originIntensity1 + glowAddedIntensity1, count / duration);
                modelMaterial.SetFloat("_ColorIntensity1", intensity1);
                
                float intensity2 = Mathf.Lerp(originIntensity2, originIntensity2 + glowAddedIntensity2, count / duration);
                modelMaterial.SetFloat("_ColorIntensity2", intensity2);

                //count += Time.fixedDeltaTime;
                //yield return new WaitForFixedUpdate();

                count += Time.deltaTime;
                yield return null;
            }
            modelMaterial.SetFloat("_ColorIntensity1", originIntensity1 + glowAddedIntensity1);
            modelMaterial.SetFloat("_ColorIntensity2", originIntensity2 + glowAddedIntensity2);

        }
        public void Disappear()
        {
            var materialColor = modelMaterial.color;
            materialColor.a = 0;
            modelMaterial.color = materialColor;

            //Reset Glow Intensity
            modelMaterial.SetFloat("_ColorIntensity1", colorHdr1.intensity);
            modelMaterial.SetFloat("_ColorIntensity2", colorHdr2.intensity);
        }
        public void MoveTo(Vector3 target, float duration)
        {
            StartCoroutine(MoveToRoutine(target, duration));
        }
        IEnumerator MoveToRoutine(Vector3 target, float duration)
        {
            float count = 0;
            Vector3 startingPosition = transform.position;
            Vector3 targetPosition = target;

            yield return null;

            while (count < duration)
            {
                count += Time.deltaTime;
                transform.position = Vector3.Slerp(startingPosition, targetPosition, count / duration);

                yield return null;
            }
            transform.position = targetPosition;
        }

        
    }
}
