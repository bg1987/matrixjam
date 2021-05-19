using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Resume : MonoBehaviour
    {
        [SerializeField] Activator ingameMenuActivator;

        public void ResumeGame()
        {
            ingameMenuActivator.Deactivate();
        }
    }
}
