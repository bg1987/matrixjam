using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.IngameMenu
{
    public class Restart : MonoBehaviour
    {
        public void RestartGame()
        {
            MatrixTraveler.Instance.ReTravelToCurrentGame();
        }
    }
}
