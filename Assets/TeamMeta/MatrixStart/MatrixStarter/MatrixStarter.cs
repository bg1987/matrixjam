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

        [Header("Start with custom history")]
        [SerializeField] bool shouldUseCustomHistory = false;
        [SerializeField] List<MatrixEdgeData> customHistory;
        // Start is called before the first frame update
        IEnumerator Start()
        {
            if (shouldUseCustomHistory && customHistory.Count > 0)
                LoadWithCustomHistory();
            yield return null;
            if (!startOnAwake)
                yield break;
            if(shouldUseCustomHistory && customHistory.Count>0)
                matrixTraveler.ReTravelToCurrentGame();
            else if (startingGameIndex == -1)
                StartRandomGame();
            else
            {
                StartGame(startingGameIndex);
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
        public void LoadWithCustomHistory()
        {
            matrixTraveler.travelData.Load(customHistory);
        }
    }
}
