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
            Connection lastConnection = playerData.LastCon;
            gameIndexText.SetText(playerData.current_level + "");

            RefreshEntranceUsedText();

            EnteredFromGameText.SetText(lastConnection.scene_from + "");
            
        }
        void RefreshEntranceUsedText()
        {
            var entranceUsedString = "";
            // -3 means came from matrix start. Yes should be refactored to at least make numeric sense
            if (SceneManager.SceneMang.Numentrence == -3)
            {
                var levelHolder = FindObjectOfType<LevelHolder>();
                if (levelHolder)
                    entranceUsedString = levelHolder.def_ent + "";
                else
                    Debug.Log("There should be a level holder in scene " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
            else
                entranceUsedString = SceneManager.SceneMang.Numentrence + "";

            entranceUsedText.SetText(entranceUsedString);
        }
    }
}
