using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Options : MonoBehaviour,ISelectionSelectListener
    {
        [SerializeField] MenuActivator optionsMenuActivator;

        public void ToggleOptionsMenu()
        {
            if (optionsMenuActivator.IsActivated)
                optionsMenuActivator.Deactivate();
            else
                optionsMenuActivator.Activate();
        }
        public void Select()
        {
            optionsMenuActivator.Activate();
        }
        public void Unselect()
        {
            optionsMenuActivator.Deactivate();
        }
        public void UnselectImmediately()
        {
            optionsMenuActivator.DeactivateImmediately();
        }
    }
}
