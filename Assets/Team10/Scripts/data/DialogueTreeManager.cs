using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class RandomDialogueTree
    {
        private int NUM_OF_ACTIONS = 22;
        private Dictionary<string, List<DialogueTree>> randomDialogues;
        private List<DialogueTree> dialogues;
        private Character n1; // Mika
        private Character n2; //Maya
        private Character n3; //Raul
        private DialogueTree FriendCall;

        public RandomDialogueTree(string playerName){
            n1 = new Character("Mika");
            n2 = new Character("Maya");
            n3 = new Character("Raul");

            randomDialogues = new Dictionary<string, List<DialogueTree>>();
            randomDialogues.Add("Kitchen", getKitchenDialogues(playerName));
            randomDialogues.Add("Bath", getBathDialogues(playerName));
            randomDialogues.Add("LivingRoom", getLivingRoomDialogues(playerName));

            dialogues = contDialogues(playerName);

            switch(playerName){
                case "Batman":
                    FriendCall = getCallAlfredDialogue();
                    break;
                case "Superman":
                    FriendCall = getCallMarthaDialogue();
                    break;
                case "Matrix":
                    FriendCall = getCallMatrixDialogue();
                    break;
                case "Corona":
                    FriendCall = getCallFBIDialogue();
                    break;
                default:
                    FriendCall = getCallPokeDialogue(playerName);
                    break;
            }
        }

        public bool getRandomDialog(string room, out DialogueTree dialogue){
            List<DialogueTree> roomDialogues;
            dialogue = new DialogueTree();
            bool hasDialogues = randomDialogues.TryGetValue(room, out roomDialogues);
            System.Random rand = new System.Random();
            int r = rand.Next(roomDialogues.Count*10);
            Debug.Log(r + "-" + roomDialogues.Count);
            if(hasDialogues && roomDialogues.Count > 0 && r % 10 == 0){
                dialogue = roomDialogues[r / 10];
                roomDialogues.RemoveAt(r / 10);
                randomDialogues[room] = roomDialogues;
                return true;
            }
            return false;
        }

        public DialogueTree getDialogueById(int dialogueId){
            return dialogues[dialogueId - 1];
        }

        public List<DialogueTree> contDialogues(string playerName){
            List<DialogueTree> cd = new List<DialogueTree>();
            DialogueTree t1 = new DialogueTree(n1);
            t1.addDialogue(new Dialogue(n1.name, "i don't! anyways, i was here first, so leave or wait outside!"));
            t1.addDialogue(new Dialogue(playerName, new int[] {14+NUM_OF_ACTIONS, 15+NUM_OF_ACTIONS}));
            cd.Add(t1);
            
            DialogueTree t2 = new DialogueTree(n1);
            t2.addDialogue(new Dialogue(n1.name, "good, since you ate then you do the dishes \nAdios"));
            t2.addDialogue(new Dialogue(playerName, new int[] {16+NUM_OF_ACTIONS, 17+NUM_OF_ACTIONS}));
            cd.Add(t2);

            DialogueTree t3 = new DialogueTree(n2);
            t3.addDialogue(new Dialogue(n2.name, "i never clean"));
            cd.Add(t3);

            DialogueTree t4 = new DialogueTree(n2);
            t4.addDialogue(new Dialogue(n2.name, "shh... i'm thinking about life..."));
            t4.addDialogue(new Dialogue(playerName, new int[] {18+NUM_OF_ACTIONS, 19+NUM_OF_ACTIONS}));
            cd.Add(t4);

            DialogueTree t5 = new DialogueTree(n2);
            t5.addDialogue(new Dialogue("", "60 min later..."));
            t5.addDialogue(new Dialogue(n2.name, "i've finished \nyou can do whatever you want now"));
            cd.Add(t5);

            DialogueTree t6 = new DialogueTree(n1);
            t6.addDialogue(new Dialogue(n1.name, "so go and buy"));
            t6.addDialogue(new Dialogue(playerName, new int[] {20+NUM_OF_ACTIONS, 21+NUM_OF_ACTIONS, 22+NUM_OF_ACTIONS}));
            cd.Add(t6);

            DialogueTree t7 = new DialogueTree(n2);
            t7.addDialogue(new Dialogue(n2.name, "lalala, can't hear you"));
            t7.addDialogue(new Dialogue(playerName, new int[] {23+NUM_OF_ACTIONS, 24+NUM_OF_ACTIONS}));
            cd.Add(t7);
            return cd;
        }

        // generate random dialogue per room
        public List<DialogueTree> getKitchenDialogues(string playerName){
            List<DialogueTree> hd = new List<DialogueTree>();
            /// dialog 1
            DialogueTree t1 = new DialogueTree(n1);
            t1.addDialogue(new Dialogue(n1.name, "i'm making some pasta. \ndo you want some?"));
            t1.addDialogue(new Dialogue(playerName, new int[] {0, 1}));
            hd.Add(t1);
            // end dialog 1
            /// dialog 2
            DialogueTree t2 = new DialogueTree(n1);
            t2.addDialogue(new Dialogue(n1.name, "we have no food.... \nwe need to buy groceries"));
            t2.addDialogue(new Dialogue(playerName, new int[] {2, 3}));
            hd.Add(t2);
            // end dialog 2
            return hd;
        }
        public List<DialogueTree> getBathDialogues(string playerName){
            List<DialogueTree> hd = new List<DialogueTree>();
            /// dialog 1
            DialogueTree t1 = new DialogueTree(n1);
            t1.addDialogue(new Dialogue(n1.name, "i was here first!"));
            t1.addDialogue(new Dialogue(playerName, new int[] {4+NUM_OF_ACTIONS, 5+NUM_OF_ACTIONS}));
            hd.Add(t1);
            // dialog 2
            DialogueTree t2 = new DialogueTree(n2);
            t2.addDialogue(new Dialogue(n2.name, "i'm singing in the rain~ \n   just singing in the rain~~"));
            t2.addDialogue(new Dialogue(playerName, new int[] {6+NUM_OF_ACTIONS, 7+NUM_OF_ACTIONS, 8+NUM_OF_ACTIONS}));
            hd.Add(t2);
            return hd;
        }
        public List<DialogueTree> getLivingRoomDialogues(string playerName){
            List<DialogueTree> hd = new List<DialogueTree>();
            /// dialog 1
            DialogueTree t1 = new DialogueTree(n1);
            t1.addDialogue(new Dialogue(n1.name, "hey, did you clean the house? \nit's your turn"));
            t1.addDialogue(new Dialogue(playerName, new int[] {9, 10, 11}));
            hd.Add(t1);
            // end dialog 1
            /// dialog 2
            DialogueTree t2 = new DialogueTree(n1);
            t2.addDialogue(new Dialogue(n2.name, "..."));
            t2.addDialogue(new Dialogue(playerName, new int[] {12, 13}));
            hd.Add(t2);
            // end dialog 2
            return hd;
        }

        private DialogueTree multipleSentences(string name, string[] sentences){
            DialogueTree f = new DialogueTree();
            foreach (string s in sentences)
            {
                f.addDialogue(new Dialogue(name, s));
            }
            return f;
        }

        // generate start (intro) dialogue
        public DialogueTree getStarterDialogue(string playerName){
            DialogueTree starter = new DialogueTree();
            string[] sentences = new string[] {"morning! it's the start of another day..",
                "should i get some coffee and work from my computer? \nor netflix & chill in the living room?",
                "i dont want to meet my roommates, though",
                "let me tell you, they can be quite annoying sometimes",
                "well what should i do today?"};
            // if 42 - add full game explenation
            // now listen 42, this is a secret, but this game is not as simple as it's seems
            //...
            return multipleSentences(playerName, sentences);
        }

        //get static dialogues
        public DialogueTree getRoommate1Dialogue(){
            DialogueTree roommate1 = new DialogueTree(n1);
            roommate1.addDialogue(new Dialogue(n1.name, "what do you want?? i'm taking a nap here\ngo away!!!!! -_-\nzzz"));
            return roommate1;
        } 
        public DialogueTree getRoommate2Dialogue(){
            DialogueTree roommate2 = new DialogueTree(n2);
            roommate2.addDialogue(new Dialogue(n2.name, ""));
            return roommate2;
        }
        public DialogueTree getCallFriendDialogue(){
            return FriendCall;
        }
        public DialogueTree getCallAlfredDialogue(){
            Character n4 = new Character("alfred");
            string[] sentences = new string[] {"Batman, i have the keys to you bat cave \nyou can't hide from me",
            "the world is in danger - it's the corona virus \nget off the couch and come save the world"};
            DialogueTree f = multipleSentences(n4.name, sentences);
            f.addDialogue(new Dialogue("Batman", new int[] {25, 26}));
            return f;
        }

        public DialogueTree getCallMarthaDialogue(){
            Character n4 = new Character("Martha (mom)");
            string[] sentences = new string[] {"Superman, i thought batman killed you",
            "t why are you hiding in a shared appartment \ncome back home!!"};
            DialogueTree f = multipleSentences(n4.name, sentences);
            f.addDialogue(new Dialogue("Superman", new int[] {27}));
            return f;
        }

        public DialogueTree getCallMatrixDialogue(){
            Character n4 = new Character("^%#^%^*");
            string[] sentences = new string[] {"this is not reality *beep beep*",
            "come back to the real world *beep*", "i hid the *beep* in the refrigirator *beep* (as food)",
            "come back *beep* asap *beep, beep, beep* \n#breaking_the_4th_wall!!!!!"};
            DialogueTree f = multipleSentences(n4.name, sentences);
            return f;
        }

        public DialogueTree getCallFBIDialogue(){
            Character n4 = new Character("FBI");
            string[] sentences = new string[] {"this is FBI",
            "stop spreading your jerms around \n",
            "we will lock you in jail"};
            DialogueTree f = multipleSentences(n4.name, sentences);
            f.addDialogue(new Dialogue("FBI2", "we dont want our prisoners to die"));
            return f;
            //ending
                // Boss corona
                // we concoured the world
                // everyone is infected and zombiefied
                // good luck in your next mission
        }

        public DialogueTree getCallPokeDialogue(string playerName){
            Character n4 = new Character("Prof. Oak");
            string[] sentences = new string[] {"i just discovered a new cave filled with bat pokemons",
            "also caugth \n",
            "we will lock you in jail"};
            DialogueTree f = multipleSentences(n4.name, sentences);
            //
            f.addDialogue(new Dialogue(playerName, new int[] {}));
            return f;
        }
    }
}
