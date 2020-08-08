using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MatrixJam.Team10
{
    public class DialogueManager : MonoBehaviour
    {
        public GameObject dialoguePanel;
        public TextMeshProUGUI nameDisplay;
        public TextMeshProUGUI textDisplay;
        public GameObject continueButton;
        public Queue<Dialogue> dialogues;
        public float typingSpeed;

        private string currSentence;

        // Start is called before the first frame update
        void Start()
        {
            dialogues = new Queue<Dialogue>();
        }

        public void StartDialogue(DialogueTree tree){
            Debug.Log("start a conversation");
            dialogues.Clear();
            dialogues = tree.getDialogues();
            dialoguePanel.SetActive(true);
            DisplayNextDialogue();
        }

        public void StartActionChoice(Dialogue actions){
            Debug.Log("do something");
            dialogues.Clear();
            dialogues.Enqueue(actions);
            dialoguePanel.SetActive(true);
            DisplayNextDialogue();
        }

        IEnumerator Type(string sentence){
            textDisplay.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        public void DisplayNextDialogue(){
            continueButton.SetActive(false);
            if(dialogues.Count == 0){
                EndDialogue();
                return;
            }
            Dialogue curr = dialogues.Dequeue();
            nameDisplay.text = curr.name;
            if(curr.choices.Length > 0){
                Debug.Log("implement buttons");
            }
            else{
                currSentence = curr.sentences;
                StartCoroutine(Type(curr.sentences));
            }
        }

        public void EndDialogue(){
            Debug.Log("End");
            dialoguePanel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(textDisplay.text == currSentence){
                continueButton.SetActive(true);
            }
        }


    }
}
