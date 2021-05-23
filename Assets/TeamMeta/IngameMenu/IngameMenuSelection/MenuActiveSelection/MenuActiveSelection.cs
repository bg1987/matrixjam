using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class MenuActiveSelection : MonoBehaviour
    {
        [SerializeField] MenuSelections menuSelections;
        [SerializeField] Selection activeSelection;

        public void SelectSelection(Selection selection)
        {
            if (activeSelection == selection)
            {
                activeSelection.ExitSelect();
                activeSelection = null;
                return;
            }
            if (activeSelection != null)
                activeSelection.ExitSelect();

            activeSelection = selection;
            activeSelection.EnterSelect();
        }
        public void UnselectImmediately()
        {
            if (!activeSelection)
                return;
            activeSelection.ExitSelectImmediately();
            activeSelection = null;
        }
        public void Unselect()
        {
            if (!activeSelection)
                return;
            activeSelection.ExitSelect();
            activeSelection = null;
        }
    }
}
