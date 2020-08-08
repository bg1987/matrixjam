using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class RandomDialogueTree
    {
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
            return dialogues[dialogueId];
        }

        public List<DialogueTree> contDialogues(string playerName){
            List<DialogueTree> cd = new List<DialogueTree>();
            DialogueTree t1 = new DialogueTree(n1);
            t1.addDialogue(new Dialogue(n1.name, "i don't! anyways, i was here first, so leave or wait outside!"));
            //1, "go away!", kill
            //2, "give up", time-15
            t1.addDialogue(new Dialogue(playerName, new int[] {}));
            cd.Add(t1);
            
            DialogueTree t2 = new DialogueTree(n1);
            t2.addDialogue(new Dialogue(n1.name, "good, since you ate then you do the dishes \nAdios"));
            //1, "ok"
            //2, "no way", kill + clean
            t2.addDialogue(new Dialogue(playerName, new int[] {}));
            cd.Add(t2);

            DialogueTree t3 = new DialogueTree(n2);
            t3.addDialogue(new Dialogue(n2.name, "i never clean"));
            cd.Add(t3);

            DialogueTree t4 = new DialogueTree(n2);
            t4.addDialogue(new Dialogue(n2.name, "shh... i'm thinking about life..."));
            //1, "leave"
            //2, "wait" - time-60, cont5
            t4.addDialogue(new Dialogue(playerName, new int[] {}));
            cd.Add(t4);

            DialogueTree t5 = new DialogueTree(n2);
            t5.addDialogue(new Dialogue("", "60 min later..."));
            t5.addDialogue(new Dialogue(n2.name, "i've finished \nyou can do whatever you want now"));
            cd.Add(t5);

            DialogueTree t6 = new DialogueTree(n1);
            t6.addDialogue(new Dialogue(n1.name, "so go and buy"));
            //1, "go to the supermarket" - death
            //2, "buy online" - time-30
            //3, no - kill
            t6.addDialogue(new Dialogue(playerName, new int[] {}));
            cd.Add(t6);

            DialogueTree t7 = new DialogueTree(n1);
            t7.addDialogue(new Dialogue(n1.name, "lalala, can't hear you"));
            //1, "wait" - time-30
            //2, "leave"
            t7.addDialogue(new Dialogue(playerName, new int[] {}));
            cd.Add(t7);
            return cd;
        }

        // generate random dialogue per room
        public List<DialogueTree> getKitchenDialogues(string playerName){
            List<DialogueTree> hd = new List<DialogueTree>();
            /// dialog 1
            DialogueTree t1 = new DialogueTree(n1);
            t1.addDialogue(new Dialogue(n1.name, "i'm making some pasta. \ndo you want some?"));
            //1, "yes" - cont2
            //2, "no" - nothing
            t1.addDialogue(new Dialogue(playerName, new int[] {}));
            hd.Add(t1);
            // end dialog 1
            /// dialog 2
            DialogueTree t2 = new DialogueTree(n1);
            t2.addDialogue(new Dialogue(n1.name, "we have no food.... \nwe need to buy groceries"));
            //1, "then you should go" - kill
            //2, "and?" - cont5
            t2.addDialogue(new Dialogue(playerName, new int[] {}));
            hd.Add(t2);
            // end dialog 2
            return hd;
        }
        public List<DialogueTree> getBathDialogues(string playerName){
            List<DialogueTree> hd = new List<DialogueTree>();
            /// dialog 1
            DialogueTree t1 = new DialogueTree(n1);
            t1.addDialogue(new Dialogue(n1.name, "i was here first!"));
            //1, "you do this every morning" - kill + cont1
            //2, "fine" - time-30
            t1.addDialogue(new Dialogue(playerName, new int[] {}));
            hd.Add(t1);
            // dialog 2
            DialogueTree t2 = new DialogueTree(n2);
            t2.addDialogue(new Dialogue(n2.name, "i'm singing in the rain~ \n   just singing in the rain~~"));
            //1, "when will you finish" - kill + cont7
            //2, "stop singing, you are out of tune" - kill + cont7
            //3, "leave"
            t2.addDialogue(new Dialogue(playerName, new int[] {}));
            hd.Add(t2);
            return hd;
        }
        public List<DialogueTree> getLivingRoomDialogues(string playerName){
            List<DialogueTree> hd = new List<DialogueTree>();
            /// dialog 1
            DialogueTree t1 = new DialogueTree(n1);
            t1.addDialogue(new Dialogue(n1.name, "hey, did you clean the house? \nit's your turn"));
            //1, "clean the house" - time-30
            //2, "it's Maya's turn, not mine" - cont3
            //3, "clean? i don't clean" - kill
            t1.addDialogue(new Dialogue(playerName, new int[] {}));
            hd.Add(t1);
            // end dialog 1
            /// dialog 2
            DialogueTree t2 = new DialogueTree(n1);
            t2.addDialogue(new Dialogue(n2.name, "..."));
            //1, "ignore, watch tv" - cont4
            //2, "whatcha doin?" - cont4
            t2.addDialogue(new Dialogue(playerName, new int[] {}));
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
            // - save the world - death, 
            // sing - im not batman im spider pig spider pig~ call ended
            f.addDialogue(new Dialogue("Batman", new int[] {}));
            return f;
        }

        public DialogueTree getCallMarthaDialogue(){
            Character n4 = new Character("Martha (mom)");
            string[] sentences = new string[] {"Superman, i thought batman killed you",
            "t why are you hiding in a shared appartment \ncome back home!!"};
            DialogueTree f = multipleSentences(n4.name, sentences);
            // player options - go back - death
            f.addDialogue(new Dialogue("Superman", new int[] {}));
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
