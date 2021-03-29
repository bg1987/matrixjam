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

        Material modelMaterial;

        public List<Edge> startPortActiveEdges = new List<Edge>();

        List<Edge> startPortEdges = new List<Edge>();
        List<Edge> endPortEdges = new List<Edge>();


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

            var materialColor = modelMaterial.color;

            while (count < duration)
            {
                materialColor.a = Mathf.Lerp(0, 1, count / duration);
                modelMaterial.color = materialColor;

                //count += Time.fixedDeltaTime;
                //yield return new WaitForFixedUpdate();

                count += Time.unscaledDeltaTime;
                yield return null;
            }
            materialColor = modelMaterial.color;
            materialColor.a = 1;
            modelMaterial.color = materialColor;
        }
        public void Disappear()
        {
            var materialColor = modelMaterial.color;
            materialColor.a = 0;
            modelMaterial.color = materialColor;
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
