using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class ActionDialogueTrigger : MonoBehaviour
    {
        public Dialogue dialogue;

        public void TriggerDialogue(){
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
    }
}
