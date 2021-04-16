using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class NodeSelectable : MonoBehaviour,ISelectable
    {
        [SerializeField] Node node;
        bool isSelected = false;
        bool isHovered = false;
        private void Awake()
        {
            node = GetComponent<Node>();
        }
        public void Select()
        {
            if (isSelected)
                return;
            Debug.Log("Select " + name);
            isSelected = true;
        }
        public void Unselect()
        {
            if (isSelected == false)
                return;

            Debug.Log("Unselect " + name);
            isSelected = false;
        }
        public void HoverEnter()
        {
            if (isSelected)
                return;
            if (isHovered)
                return;
            Debug.Log("Hover " + name);
            isHovered = true;
        }

        public void HoverExit()
        {
            if (!isHovered)
                return;
            Debug.Log("Unhover " + name);
            isHovered = false;
        }

        
    }
}
