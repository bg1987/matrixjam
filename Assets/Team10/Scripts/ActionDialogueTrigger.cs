using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class ActionDialogueTrigger : MonoBehaviour
    {
        public int[] choices;
        public GameObject BlockPanel;
        public GameObject DialoguePanelBlock;
        private RandomDialogueTree t = new RandomDialogueTree("player1");
        private bool isMouseOnMe = false;

        void OnMouseDown(){
            if(!BlockPanel.activeSelf && !DialoguePanelBlock.activeSelf){
                TriggerDialogue();
            }
        }

        public void TriggerDialogue(){
            Dialogue dialogue = new Dialogue("", choices);
            FindObjectOfType<DialogueManager>().StartActionChoice(dialogue);
        }

        public void TriggerRandomDialogue(){
            DialogueTree a;
            bool dialog = t.getRandomDialog("Bath", out a);
            if(dialog){
                FindObjectOfType<DialogueManager>().StartDialogue(a);
            }
        }
    }
}
