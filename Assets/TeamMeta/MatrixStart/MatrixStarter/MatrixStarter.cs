using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixStarter : StartHelper
    {
        [SerializeField] MatrixTraveler matrixTraveler;
        [Header("-1 => Select Random Game")]
        [SerializeField] bool startOnAwake = true;
        [SerializeField] int startingGameIndex = 0;
        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return null;
            if (startOnAwake)
            {
                if (startingGameIndex == -1)
                    StartRandomGame();
                else
                {
                    StartGame(startingGameIndex);
                }
            }
        }
        public void StartRandomGame()
        {
            matrixTraveler.WarpToRandomGame();
        }
        public void StartGame(int gameIndex)
        {
            matrixTraveler.WarpTo(startingGameIndex, -1);
        }
        public void LoadGame()
        {
            matrixTraveler.LoadFromDisk();
        }
    }
}
