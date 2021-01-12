using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MatrixJam.TeamMeta
{
    public class MatrixDebugUI : MonoBehaviour
    {
        PlayerData playerData;

        [Header("Current Game")]
        [SerializeField] TextMeshProUGUI gameIndexText;
        [SerializeField] TextMeshProUGUI gameNameText;
        [SerializeField] TextMeshProUGUI gameVisitsText;
        [SerializeField] TextMeshProUGUI entranceUsedText;
        [SerializeField] TextMeshProUGUI entranceUseCountText;
        [Header("Previous Game")]
        [SerializeField] TextMeshProUGUI previousGameIndexText;
        [SerializeField] TextMeshProUGUI previousGameNameText;
        [SerializeField] TextMeshProUGUI exitUsedText;
        [SerializeField] TextMeshProUGUI exitUseCountText;

        // Start is called before the first frame update
        void Start()
        {
            playerData = PlayerData.Data;

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneChange;
        }

        private void SceneChange(Scene arg0, LoadSceneMode arg1)
        {
            Refresh();
        }

        void Refresh()
        {
            StartCoroutine(DelayedRefreshRoutine());
        }
        IEnumerator DelayedRefreshRoutine()
        {
            yield return null;
            yield return null;

            MatrixTravelData travelData = MatrixTraveler.Instance.travelData;
            RefreshCurrentGame();
            RefreshPreviousGame();
            //previousGameIndexText.SetText("Not implemented" + "");
        }
        void RefreshCurrentGame()
        {
            MatrixTravelData travelData = MatrixTraveler.Instance.travelData;
            MatrixNodeData currentGame = MatrixTraveler.Instance.GetCurrentGame();
            MatrixPortData lastUsedEntrance = MatrixTraveler.Instance.GetLastUsedEntrance();
            gameIndexText.SetText(currentGame.index.ToString());
            gameNameText.SetText(currentGame.name);
            gameVisitsText.SetText(travelData.GetGameVisitCount(currentGame).ToString());

            entranceUsedText.SetText(lastUsedEntrance.id.ToString());
            entranceUseCountText.SetText(travelData.GetEntranceVisitCount(lastUsedEntrance).ToString());
        }
        void RefreshPreviousGame()
        {
            MatrixTravelData travelData = MatrixTraveler.Instance.travelData;
            IReadOnlyList<MatrixEdgeData> history = travelData.GetHistory();
            if(history.Count==0)
            {
                return;
            }
            MatrixEdgeData lastEdge = history[history.Count - 1];
            MatrixPortData exitPort = lastEdge.startPort;

            string previousGameIndexString;
            string previousGameNameString;
            string exitUsedString;
            string exitUseCountString;

            if (exitPort.nodeIndex == -1)
            {
                previousGameIndexString = "";
                previousGameNameString = "";
            }
            else
            {
                MatrixNodeData previousGame = MatrixTraveler.Instance.matrixGraphData.nodes[lastEdge.startPort.nodeIndex];
                previousGameIndexString = previousGame.index.ToString();
                previousGameNameString = previousGame.name;
            }
            if(exitPort.id== -1)
            {
                exitUsedString = "";
                exitUseCountString = "";
            }
            else
            {
                exitUsedString = exitPort.id.ToString();
                exitUseCountString = travelData.GetExitVisitCount(exitPort).ToString();

            }

            previousGameIndexText.SetText(previousGameIndexString);
            previousGameNameText.SetText(previousGameNameString);
            exitUsedText.SetText(exitUsedString);
            exitUseCountText.SetText(exitUseCountString);
        }
    }
}
