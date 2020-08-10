using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team22
{
    public class DialogueManager : MonoBehavior
    {
        public float openingDelay = 2f;
        public string[] introDialogues;
        public Animator messageBoxAnimator;
        public Text messageBoxText;
        public AudioClip confirmSound;
    }
}
