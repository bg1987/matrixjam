using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta
{
    public class MainMenu : MonoBehaviour
    {
        // ---------------- Main menu
        
        [SerializeField]
        private List<Button> mainButtons;
        
        // submenus
        [SerializeField]
        private GameObject rules;
        [SerializeField]
        private GameObject credits;
        
        private void SetMainButtons(bool newInteractableStatus)
        {
            foreach (var button in mainButtons) {
                button.interactable = newInteractableStatus;
            }
        }
        
        
        
        
        // ---------------- Main menu button handlers
        
        public void ButtonHandler_PlayGame()
        {
            // <------ TODO: Start game here
        }
        
        public void ButtonHandler_ShowRules()
        {
            rules.SetActiveRecursively(true);
            SetMainButtons(false);
        }
        
        public void ButtonHandler_ShowCredits()
        {
            credits.SetActiveRecursively(true);
            SetMainButtons(false);
        }
        
        public void ButtonHandler_ExitGame()
        {
            Application.Quit();
        }
        
        
        
        
        // ---------------- Submenu button handlers
        
        public void ButtonHandler_BackToMenu()
        {
            rules.SetActiveRecursively(false);
            credits.SetActiveRecursively(false);
            SetMainButtons(true);
        }
    }
}
