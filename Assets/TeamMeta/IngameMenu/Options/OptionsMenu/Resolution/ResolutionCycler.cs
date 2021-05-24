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
        private void Awake()
        {
            Resolution[] resolutions = Screen.resolutions;
            resolutionIndex = FindCurrentResolutionIndex(resolutions);
            UpdateResolutionText(Screen.currentResolution);
        }
        public void SwitchToNextResolution()
        {
            Resolution[] resolutions = Screen.resolutions;
            this.resolutions = resolutions;

            resolutionIndex = (resolutionIndex + 1) % resolutions.Length;
            var targetResolution = resolutions[resolutionIndex];

            Screen.SetResolution(targetResolution.width, targetResolution.height,true, targetResolution.refreshRate);
            UpdateResolutionText(targetResolution);
        }
        public void SwitchToPreviousResolution()
        {
            Resolution[] resolutions = Screen.resolutions;
            this.resolutions = resolutions;
            resolutionIndex =  (resolutionIndex + resolutions.Length - 1) % resolutions.Length;

            var targetResolution = resolutions[resolutionIndex];

            Screen.SetResolution(targetResolution.width, targetResolution.height, true, targetResolution.refreshRate);
            UpdateResolutionText(targetResolution);
        }
        public void UpdateResolutionText(Resolution resolution)
        {
            resolutionText.SetText(resolution.width + ":" + resolution.height);
        }
        public int FindCurrentResolutionIndex(Resolution[] resolutions)
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
    }
}
