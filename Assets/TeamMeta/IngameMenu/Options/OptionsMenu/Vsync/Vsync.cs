using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Vsync : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI vsyncText;
        private void Awake()
        {
            RefreshText();
        }
        public void Toggle()
        {
            if (QualitySettings.vSyncCount == 1)
                TurnOff();
            else
                TurnOn();
            RefreshText();
        }
        void RefreshText()
        {
            if (QualitySettings.vSyncCount == 1)
                vsyncText.SetText("Vsync: On");
            else
                vsyncText.SetText("Vsync: Off");
        }
        void TurnOn()
        {
            QualitySettings.vSyncCount = 1;

        }
        void TurnOff()
        {
            QualitySettings.vSyncCount = 0;
        }
    }
}
