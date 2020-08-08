using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class ActionDialogueTrigger : MonoBehaviour
    {
        public Dialogue dialogue;
        private RandomDialogueTree t = new RandomDialogueTree("player1");
        public void TriggerDialogue(){
            FindObjectOfType<DialogueManager>().StartActionChoice(dialogue);
        }

        public void TriggerRandomDialogue(){
            // DialogueTree a;
            // bool dialog = t.getRandomDialog("Bath", out a);
            // if(dialog){
                // FindObjectOfType<DialogueManager>().StartDialogue(a);
            // }
            FindObjectOfType<DialogueManager>().StartDialogue(t.getCallAlfredDialogue());
        }
    }
}
