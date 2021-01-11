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

        [SerializeField] TextMeshProUGUI gameIndexText;
        [SerializeField] TextMeshProUGUI entranceUsedText;
        [SerializeField] TextMeshProUGUI EnteredFromGameText;
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

            MatrixNodeData node = MatrixTraveler.Instance.currentGame;

            gameIndexText.SetText(node.index+ "");

            RefreshEntranceUsedText();

            EnteredFromGameText.SetText("Not implemented yet" + "");
        }
        void RefreshEntranceUsedText()
        {
            var entranceUsedString = "";
            MatrixPortData entrancePort = MatrixTraveler.Instance.enteredAt;
            // -1 means came from matrix start.
            if (entrancePort.id == -1)
            {
                var levelHolder = FindObjectOfType<LevelHolder>();
                if (levelHolder)
                    entranceUsedString = levelHolder.def_ent + "";
                else
                    Debug.Log("There should be a level holder in scene " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
            else
                entranceUsedString = entrancePort.id + "";

            entranceUsedText.SetText(entranceUsedString);
        }
    }
}
