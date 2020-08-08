using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team22
{
    public class DialogueManager : MonoBehaviour
    {
        public float openingDelay = 2f;

        [TextArea]
        public string[] introDialogues;

        [TextArea]
        public string[] postTutorialDialogues;

        public Animator messageBoxAnimator;
        public Text messageBoxText;
        public AudioSource source;
        public AudioClip confirmSound;

        private string[] currentDialogue;
        private int dIndex = 0;
        private bool canDialogue = false;

        public static DialogueManager instance;

        private void Start()
        {
            messageBoxAnimator.gameObject.SetActive(false);
            currentDialogue = introDialogues;
            Invoke("ShowBox", openingDelay);

            instance = this;
        }

        private void ShowBox()
        {
            messageBoxAnimator.gameObject.SetActive(true);
            messageBoxText.text = currentDialogue[dIndex];
            source.PlayOneShot(confirmSound);

            canDialogue = true;
        }

        private void Update()
        {
            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape) && canDialogue)
            {
                NextDialogue();
            }

         }

        private void NextDialogue()
        {
            if(dIndex + 1 >= currentDialogue.Length)
            {
                // no more dialogues
               //messageBoxText.text = "";
               messageBoxAnimator.SetTrigger("Out");
               canDialogue = false;

               if (currentDialogue == introDialogues)
                {
                    GameManager.instance.StartTutorial();
                }

                if (currentDialogue == postTutorialDialogues)
                {
                    GameManager.instance.StartGame();
                }
            }
            else
            {
                dIndex++;
                messageBoxAnimator.SetTrigger("In");
                source.PlayOneShot(confirmSound);
                messageBoxText.text = currentDialogue[dIndex];
            }
        }

        public void EnableDialogues(int i)
        {
            if (i == 0)
                currentDialogue = postTutorialDialogues;

            canDialogue = true;
            dIndex = -1;
            NextDialogue();
        }

        public bool IsInDialogue()
        {
            return canDialogue;
        }
    }
}
