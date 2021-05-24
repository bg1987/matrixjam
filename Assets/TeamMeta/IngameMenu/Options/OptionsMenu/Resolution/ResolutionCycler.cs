using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Resolution[] resolutions = GetHighestRefreshRateResolutions();
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
            Resolution[] resolutions = GetHighestRefreshRateResolutions();
            this.resolutions = resolutions;
            resolutionIndex = (resolutionIndex + resolutions.Length + indexOffset) % resolutions.Length;

            var targetResolution = resolutions[resolutionIndex];

            UpdateResolutionText(targetResolution);

            if (resolutionIndex != appliedResolutionIndex)
                ActivateApplyMenu();
            else
                DeactivateApplyMenu();
        }
        Resolution[] GetHighestRefreshRateResolutions()
        {
            var highestRefreshPerResolutionDictionary = new Dictionary<Vector2,Vector2>(); //< width, height, refreshRate, index >
            var i = 0;
            for (i = 0; i < Screen.resolutions.Length; i++)
            {
                var resolution = Screen.resolutions[i];
                Vector2 resolutionVector = new Vector2(resolution.width, resolution.height);
                if (highestRefreshPerResolutionDictionary.ContainsKey(resolutionVector))
                {
                    int highestRefreshRate = (int)highestRefreshPerResolutionDictionary[resolutionVector].x;
                    if(resolution.refreshRate> highestRefreshRate)
                    {
                        highestRefreshPerResolutionDictionary[resolutionVector] = new Vector2(resolution.refreshRate, i);
                    }
                }
                else
                    highestRefreshPerResolutionDictionary.Add(resolutionVector, new Vector2(resolution.refreshRate, i));
                i++;
            }

            i = 0;

            Resolution[] resolutions = new Resolution[highestRefreshPerResolutionDictionary.Count];
            foreach (var keyValuePair in highestRefreshPerResolutionDictionary)
            {
                int resolutionIndex = (int)keyValuePair.Value.y;
                var resolution = Screen.resolutions[resolutionIndex];
                resolutions[i] = resolution;
                i++;
            }
            return resolutions;
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
            int targetIndex = resolutions.Length-1;
            var currentResolution = Screen.currentResolution;
            for (int i = 0; i < resolutions.Length; i++)
            {
                Resolution resolution = resolutions[i];

                if (currentResolution.width == resolution.width &&
                    currentResolution.height == resolution.height)
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
        public void ResetToCurrentlyAppliedResolution()
        {
            resolutionIndex = appliedResolutionIndex;
            UpdateResolutionText(resolutions[resolutionIndex]);
        }
    }
}
