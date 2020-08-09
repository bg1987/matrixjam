using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team10
{
    public class SetPlayer : MonoBehaviour
    {
        public GameObject Player; // basic player
        public GameObject MatrixPlayer; // Matrix player
        public GameObject BatmanPlayer; // batman player
        public GameObject SupermanPlayer; // Superman player
        public GameObject Panel;
        public Text textBox;

        // Start is called before the first frame update
        void Start()
        {
            Player.SetActive(false);
            MatrixPlayer.SetActive(false);
            BatmanPlayer.SetActive(false);
            SupermanPlayer.SetActive(false);
            Panel.SetActive(true);
        }

        // Update is called once per frame
        public void OnSubmit()
        {
            string Name = textBox.text.ToUpper();
            switch (Name)
            {
                case "MATRIX":
                    MatrixPlayer.SetActive(true);
                    break;
                case "BATMAN":
                    BatmanPlayer.SetActive(true);
                    break;
                case "SUPERMAN":
                    SupermanPlayer.SetActive(true);
                    break;
                default: 
                    Player.SetActive(true);
                    break;
            }      
            Panel.SetActive(false);
            
            //pass name to dialogue manager in game manager
            FindObjectOfType<GameRules>().t = new RandomDialogueTree(Name);
        }
    }
}
