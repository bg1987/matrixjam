using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team22
{
    public class GameManager : MonoBehaviour
    {
        // stats
        private int hits = 0;
        private int misses = 0;
        // tutorial
        [Header("Tutorial")]
        [SerializeField]
        private bool tutorial = true;
        public GameObject tutorialUI;
        public Text tutorialHitsLeft;
        public int requiredTutorialHits = 6;
        public GameObject tutorialTimeline;
        public AudioClip tutorialComplete;
        // game
        [Header("Game")]
        public GameObject gameTimeline;
        // ui
        [Header("Dev")]
        public Text devText;


        private AudioSource source;
        public static GameManager instance;

        private void Awake()
        {
            instance = this;
            source = GetComponent<AudioSource>();

            //Setup();
        }

        public void Setup()
        {
            tutorialTimeline.SetActive(false);
            gameTimeline.SetActive(false);
            tutorialUI.SetActive(false);
        }

        public void Update()
        {
            if(devText != null)
            {
                devText.text = "HITS " + hits + "\nMISSES " + misses;
            }

            if(tutorial)
            {
                tutorialHitsLeft.text = (requiredTutorialHits - hits).ToString();
                if (hits >= requiredTutorialHits)
                {
                    tutorial = false;
                    source.PlayOneShot(tutorialComplete);
                    tutorialTimeline.SetActive(false);

                    Invoke("PostTutorialDialogue", 2f);
                }
            }
        }

        public void UpdateStats(int addHits, int addMiss)
        {
            hits += addHits;
            misses += addMiss;
        }

        public void StartTutorial()
        {
            tutorialTimeline.SetActive(true);
            tutorialUI.SetActive(true);
        }

        public void StartGame()
        {
            gameTimeline.SetActive(true);
            hits = 0;
            misses = 0;
        }

        void PostTutorialDialogue()
        {
            tutorialUI.SetActive(false);
            DialogueManager.instance.EnableDialogues(0);
        }
    }
}
