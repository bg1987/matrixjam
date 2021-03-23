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

        //Edge Creation
        private List<int> EdgesNormalSign = new List<int>(); //1 positive, -1 negative

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

            //UpdateEdgesAnchors();

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
    }
}
