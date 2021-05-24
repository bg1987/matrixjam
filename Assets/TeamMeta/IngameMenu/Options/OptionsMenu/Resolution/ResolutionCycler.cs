using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class ResolutionCycler : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI resolutionText;
        [SerializeField] Resolution[] resolutions;
        [SerializeField] int resolutionIndex;
        [SerializeField] int appliedResolutionIndex;
        [SerializeField] MenuActivator applyMenu;
        private void Awake()
        {
            Resolution[] resolutions = Screen.resolutions;
            resolutionIndex = FindCurrentResolutionIndex(resolutions);
            appliedResolutionIndex = resolutionIndex;

            UpdateResolutionText(Screen.currentResolution);
        }
        
        public void SwitchToNextResolution()
        {
            SwitchResolution(1);
        }
        
        public void SwitchToPreviousResolution()
        {
            SwitchResolution(-1);

        }
        void SwitchResolution(int indexOffset)
        {
            Resolution[] resolutions = Screen.resolutions;
            this.resolutions = resolutions;
            resolutionIndex = (resolutionIndex + resolutions.Length + indexOffset) % resolutions.Length;

            var targetResolution = resolutions[resolutionIndex];

            UpdateResolutionText(targetResolution);

            if (resolutionIndex != appliedResolutionIndex)
                ActivateApplyMenu();
            else
                DeactivateApplyMenu();
        }
        public void UpdateResolutionText(Resolution resolution)
        {
            resolutionText.SetText(resolution.width + ":" + resolution.height);
        }
        
        public void ApplyResolution()
        {
            var resolution = resolutions[resolutionIndex];
            appliedResolutionIndex = resolutionIndex;

            Screen.SetResolution(resolution.width, resolution.height, true, resolution.refreshRate);

            DeactivateApplyMenu();
        }
        int FindCurrentResolutionIndex(Resolution[] resolutions)
        {
            int targetIndex = 0;
            var currentResolution = Screen.currentResolution;
            for (int i = 0; i < resolutions.Length; i++)
            {
                Resolution resolution = resolutions[i];

                if (currentResolution.width == resolution.width &&
                    currentResolution.height == resolution.height &&
                    currentResolution.refreshRate == resolution.refreshRate)
                {
                    targetIndex = i;
                    break;
                }
            }
            return targetIndex;
        }
        void ActivateApplyMenu()
        {
            if (applyMenu.IsActivated == false)
                applyMenu.Activate();
        }
        void DeactivateApplyMenu()
        {
            applyMenu.Deactivate();
        }
    }
}
