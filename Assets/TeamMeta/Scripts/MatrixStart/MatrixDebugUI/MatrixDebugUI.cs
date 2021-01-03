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
            
            // -3 means came from matrix start. Yes should be refactored to at least make numeric sense
            entranceUsedText.SetText(SceneManager.SceneMang.Numentrence + "");

            EnteredFromGameText.SetText(lastConnection.scene_from + "");
            
        }
    }
}
