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

            previousGameIndexText.SetText("Not implemented" + "");
        }
        void RefreshCurrentGame()
        {
            MatrixTravelData travelData = MatrixTraveler.Instance.travelData;
            MatrixNodeData currentGame = MatrixTraveler.Instance.GetCurrentGame();
            MatrixPortData lastUsedEntrance = MatrixTraveler.Instance.GetLastUsedEntrance();
            gameIndexText.SetText(currentGame.index.ToString());
            gameNameText.SetText(currentGame.name);
            gameVisitsText.SetText(travelData.GetGameVisitCount(currentGame).ToString());

            entranceUsedText.SetText(lastUsedEntrance.ToString());
            entranceUseCountText.SetText(travelData.GetEntranceVisitCount(lastUsedEntrance).ToString());
        }
    }
}
