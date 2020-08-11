using System.Collections;
using System.Collections.Generic;
using MatrixJam;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team10
{
    public class StartGame : StartHelper
    {

        public GameObject StartPanel;
        public Text textBox;

        public override void StartHelp(int num_ent)
        {
            base.StartHelp(num_ent);
            StartPanel.SetActive(true);
        }
        
        // Start is called before the first frame update
        void Start(){}

        private bool isSpecialCharacter(string playerName){
            //, "MAYA", "MIKA", "RAUL"
            string[] names = new string[] {"BATMAN", "CORONA", "SUPERMAN", "42", "MATRIX"};
            return System.Array.IndexOf(names, playerName) != -1;
        }

        public void OnClick()
        {
            // Player[] players = FindObjectsOfType<Player>();
            string Name = textBox.text.ToUpper();
            // if(isSpecialCharacter(Name)){
            //     foreach (Player p in players)
            //     {
            //         if(p.name != Name){
            //             p.gameObject.SetActive(false);
            //         }
            //     }
            // }
            // else{
            //     foreach (Player p in players)
            //     {
            //         if(p.name != "PLAYER"){
            //             p.gameObject.SetActive(false);
            //         }
            //     }
            // }

            RandomDialogueTree tr = new RandomDialogueTree(Name);
            GameRules g = FindObjectOfType<GameRules>();
            g.t = tr;
            g.playerName = Name;

            //and activate startDialogue
            StartPanel.SetActive(false);
            g.DialogueMenu(tr.getStarterDialogue(Name));
        }
    }
}
