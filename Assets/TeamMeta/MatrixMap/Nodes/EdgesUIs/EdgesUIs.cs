using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgesUIs : MonoBehaviour
    {
        [SerializeField] EdgeUI edgeUiPrefab;
        public List<EdgeUI> uis { get; private set; } = new List<EdgeUI>();
        private void Start()
        {
            Deactivate();
        }

        public void Init(List<Edge> edges)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                var edgeUI = Instantiate(edgeUiPrefab, transform);
                edgeUI.SetLineStartColor(edges[i].Material.GetColor("_Color"));
                uis.Add(edgeUI);
            }
        }
        public void Deactivate()
        {
            foreach (var ui in uis)
            {
                ui.Deactivate();
            }
        }
    }
}
