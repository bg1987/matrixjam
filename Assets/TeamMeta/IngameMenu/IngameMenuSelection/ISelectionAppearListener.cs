using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public interface ISelectionAppearListener
    {
        void Appear(float duration);
        void Disappear(float duration);
        void DisappearImmediately();
    }
}