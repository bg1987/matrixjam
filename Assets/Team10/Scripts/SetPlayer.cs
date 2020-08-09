using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team
{
    public class SetPlayer : MonoBehaviour
    {
        public GameObject Player; // basic player
        public GameObject MatrixPlayer; // Matrix player
        public GameObject BatmanPlayer; // batman player
        public GameObject SupermanPlayer; // Superman player
        public GameObject Panel;
        private string Name;
        public Text textBox;

        // Start is called before the first frame update
        void Start()
        {
            Player.SetActive(false);
            MatrixPlayer.SetActive(false);
            BatmanPlayer.SetActive(false);
            SupermanPlayer.SetActive(false);
        }

        // Update is called once per frame
        public void OnSubmit()
        {
            Name =textBox.text.ToUpper();
            switch (Name)
            {
                case "MATRIX": 
                    Debug.Log(1);
                    MatrixPlayer.SetActive(true);
                    break;
                case "BATMAN":
                    Debug.Log(2); 
                    BatmanPlayer.SetActive(true);
                    break;
                case "SUPERMAN": 
                    Debug.Log(3);
                    SupermanPlayer.SetActive(true);
                    break;
                default: 
                    Player.SetActive(true); 
                    Debug.Log(Name);
                    break;
            }      
            Panel.SetActive(false);
            //read textbox and activate player acordinly
            //switchCase name 
        }
    }
}
