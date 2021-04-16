using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public interface ISelectable
    {
        void HoverEnter();
        void HoverExit();
        void Select();
        void Unselect();

    }
}
