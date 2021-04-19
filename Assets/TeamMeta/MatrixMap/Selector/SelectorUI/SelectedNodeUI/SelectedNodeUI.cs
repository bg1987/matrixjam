using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class SelectedNodeUI : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;
        [SerializeField] TextMeshSizer textSizer;
        [SerializeField] TextMeshContainer textContainer;
        [SerializeField] GameObject container;
        // Start is called before the first frame update
        void Start()
        {
        }
        public void Activate()
        {
            container.SetActive(true);
        }
        public void deactivate()
        {
            container.SetActive(false);
        }
        public void SetNodeData(string name, int visitsCount, int DiscoveredEdgesCount, int totalEdgesCount)
        {
            string textString = "";
            
            textString += name;
            textString += "\n";

            textString += "Visits: "+ visitsCount;
            textString += "\n";

            textString += "Paths: "+DiscoveredEdgesCount+"\\"+totalEdgesCount;

            text.text = textString;

            textSizer.UpdateTextSize();
            textContainer.UpdateSize();
        }
    }
}
