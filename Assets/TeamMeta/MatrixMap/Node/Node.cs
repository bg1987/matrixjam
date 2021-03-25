using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class Node : MonoBehaviour
    {
        [SerializeField] GameObject model;
        Material modelMaterial;

        public List<Edge> startPortActiveEdges = new List<Edge>();

        List<Edge> startPortEdges = new List<Edge>();
        List<Edge> endPortEdges = new List<Edge>();


        // Start is called before the first frame update
        void Awake()
        {
            modelMaterial = model.GetComponent<Renderer>().material;
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
