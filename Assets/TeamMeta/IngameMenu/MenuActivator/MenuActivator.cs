using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class MenuActivator : MonoBehaviour
    {
        [SerializeField] MenuSelections menuSelections;

        bool isActivated = false;

        [SerializeField] private float appearDuration = 0.5f;
        [SerializeField] private float disappearDuration = 0.5f;

        public void Deactivate()
        {
            if (!isActivated)
                return;

            foreach (var selection in menuSelections.Selections)
            {
                selection.Disappear(disappearDuration, DeactivateSelection);
                selection.SetInteractable(false);
            }

            isActivated = false;
        }
        public void DeactivateImmediately()
        {
            foreach (var selection in menuSelections.Selections)
            {
                selection.Disappear(0, DeactivateSelection);
                selection.SetInteractable(false);
            }
            isActivated = false;
        }
        public void Activate()
        {
            if (isActivated)
                return;

            foreach (var selection in menuSelections.Selections)
            {
                selection.gameObject.SetActive(true);
                selection.Appear(appearDuration);
                selection.SetInteractable(true);
            }

            isActivated = true;
        }
        void DeactivateSelection(Selection selection)
        {
            selection.gameObject.SetActive(false);
        }
    }
}
