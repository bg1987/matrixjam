using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class NodesUIs : MonoBehaviour
    {
        [SerializeField] NodeUI nodeUiPrefab;
        public List<NodeUI> uis { get; private set; } = new List<NodeUI>();
        private void Start()
        {
            Deactivate();
        }

        public void Init(List<Node> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                var nodeUI = Instantiate(nodeUiPrefab, transform);
                uis.Add(nodeUI);
            }
        }
        public void Deactivate()
        {
            foreach (var ui in uis)
            {
                ui.deactivate();
            }
        }
    }
}
