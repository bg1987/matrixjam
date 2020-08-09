using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class RoomTrigger : MonoBehaviour
    {
        public AudioSource bgSound;
        public string RoomName;

        private RandomDialogueTree t;

        void Start()
        {
            bgSound.loop = true;
            bgSound.Play(0);
            bgSound.Pause();
            startDialogueTree("player1");
        }
        void OnTriggerEnter2D(Collider2D c){
            bgSound.UnPause();
            TriggerRandomDialogue();
        }
        void OnTriggerExit2D(Collider2D c){
            bgSound.Pause();
        }

        public void startDialogueTree(string playerName){
            t = new RandomDialogueTree(playerName);
        }

        public void TriggerRandomDialogue(){
            DialogueTree a;
            bool dialog = t.getRandomDialog(RoomName, out a);
            if(dialog){
                FindObjectOfType<DialogueManager>().StartDialogue(a);
            }
        }
    }
}
