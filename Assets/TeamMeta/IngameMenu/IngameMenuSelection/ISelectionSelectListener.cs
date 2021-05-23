using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public interface ISelectionSelectListener
    {
        void Select();
        void Unselect();
        void UnselectImmediately();
    }
}
