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
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneChange;
        }

        private void SceneChange(Scene arg0, LoadSceneMode arg1)
        {
            Refresh();
        }

        void Refresh()
        {
            playerData = PlayerData.Data;
            Connection lastConnection = playerData.LastCon;
            
            gameIndexText.SetText(playerData.current_level + "");
            entranceUsedText.SetText(lastConnection.portal_to + "");
            EnteredFromGameText.SetText(lastConnection.scene_from + "");
            
        }
    }
}
