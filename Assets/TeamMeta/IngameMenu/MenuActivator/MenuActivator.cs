using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class MenuActivator : MonoBehaviour
    {
        [SerializeField] MenuSelections menuSelections;
        [SerializeField] MenuActiveSelection menuActiveSelection;
        [SerializeField] private List<MenuActivator> subMenus;

        bool isActivated = false;
        public bool IsActivated { get => isActivated; }

        [SerializeField] private float appearDuration = 0.5f;
        [SerializeField] private float disappearDuration = 0.5f;

        private void Start()
        {
            DeactivateImmediately();
        }
        public void Deactivate()
        {
            if (!isActivated)
                return;

            if (menuActiveSelection)
                menuActiveSelection.Unselect();

            foreach (var selection in menuSelections.Selections)
            {
                selection.Disappear(disappearDuration, DeactivateSelection);
                selection.SetInteractable(false);
            }
            foreach (var subMenu in subMenus)
            {
                subMenu.Deactivate();
            }
            isActivated = false;
        }
        public void DeactivateImmediately()
        {
            if (menuActiveSelection)
                menuActiveSelection.UnselectImmediately();

            foreach (var selection in menuSelections.Selections)
            {
                selection.Disappear(0, DeactivateSelection);
                selection.SetInteractable(false);
            }
            foreach (var subMenu in subMenus)
            {
                subMenu.DeactivateImmediately();
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
