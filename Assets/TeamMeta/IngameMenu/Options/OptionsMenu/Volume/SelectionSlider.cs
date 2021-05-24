using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class SelectionSlider : MonoBehaviour
    {
        [SerializeField] Selection selection;

        [Header("Debug")]
        [SerializeField] bool isSelectionHovered = false;
        [SerializeField] bool isSliderDragged = false;

        public void SelectionHover()
        {
            isSelectionHovered = true;
            selection.EnterHover();
        }
        public void SelectionUnhover()
        {
            isSelectionHovered = false;
            if(!isSliderDragged)
                selection.ExitHover();

        }
        public void SliderBeginDrag()
        {
            isSliderDragged = true;
            selection.EnterHover();
        }
        public void SliderEndDrag()
        {
            isSliderDragged = false;
            if (!isSelectionHovered)
                selection.ExitHover();
        }

    }
}
