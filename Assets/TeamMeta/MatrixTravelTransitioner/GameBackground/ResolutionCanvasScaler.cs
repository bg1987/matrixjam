using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta
{
    public class ResolutionCanvasScaler : CanvasScaler
    {
        private const float kLogBase = 2;
        Vector2 screenSize;
        public void SetScreenResolution(Vector2 screenResolution)
        {
            screenSize = screenResolution;
        }
        public void UpdateThisFrame()
        {
            Update();
        }
        protected override void HandleScaleWithScreenSize()
        {
            //Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            if(screenSize.x==0 && screenSize.y == 0)
            {
                screenSize = new Vector2(Screen.width, Screen.height);
            }
            float scaleFactor = 0;
            switch (m_ScreenMatchMode)
            {
                case ScreenMatchMode.MatchWidthOrHeight:
                    {
                        // We take the log of the relative width and height before taking the average.
                        // Then we transform it back in the original space.
                        // the reason to transform in and out of logarithmic space is to have better behavior.
                        // If one axis has twice resolution and the other has half, it should even out if widthOrHeight value is at 0.5.
                        // In normal space the average would be (0.5 + 2) / 2 = 1.25
                        // In logarithmic space the average is (-1 + 1) / 2 = 0
                        float logWidth = Mathf.Log(screenSize.x / m_ReferenceResolution.x, kLogBase);
                        float logHeight = Mathf.Log(screenSize.y / m_ReferenceResolution.y, kLogBase);
                        float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, m_MatchWidthOrHeight);
                        scaleFactor = Mathf.Pow(kLogBase, logWeightedAverage);
                        break;
                    }
                case ScreenMatchMode.Expand:
                    {
                        scaleFactor = Mathf.Min(screenSize.x / m_ReferenceResolution.x, screenSize.y / m_ReferenceResolution.y);
                        break;
                    }
                case ScreenMatchMode.Shrink:
                    {
                        scaleFactor = Mathf.Max(screenSize.x / m_ReferenceResolution.x, screenSize.y / m_ReferenceResolution.y);
                        break;
                    }
            }

            SetScaleFactor(scaleFactor);
            SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);

        }
    }
}
