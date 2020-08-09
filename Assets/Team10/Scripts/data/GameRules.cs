using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team10
{
    public class GameRules : MonoBehaviour
    {
        private int deathCheck;
        private int repeat;
        private string lastActionID;
        
        private int cleanFactor;
        private int hungerFactor;
        private int workFactor;
        private int killFactor;

        public List<Choice> choices;
        public System.DateTime time = System.DateTime.Parse("9:00 AM");
        public Text Timer;
        public GameObject Panel;
        public RandomDialogueTree t;

        void Start()
        {
            t = new RandomDialogueTree("player1");
            declareChoices();
        }

        void Update()
        {
            Timer.text = "c - " + cleanFactor + ", h - " + hungerFactor 
                + ", w - " + workFactor + ", k - " + killFactor + ", r - " 
                + repeat + ", lastAction - " + lastActionID + "\ntime-" + time.ToString();
            //update time display
            if((time.Hour > 12 && deathCheck < 1) || (time.Hour > 6 && deathCheck < 2)) //12-start noon, 18-start evening
            {
                Debug.Log(deathCheck);
                checkDeath();
                deathCheck += 1;
            }
            else if((time.Hour > 23 || time.Hour < 4) && deathCheck == 2){ //passed to the next day
                Debug.Log("end");
                endOfDay();
            }
        }

        public void declareChoices(){
            choices = generalActions(); //6
            choices.AddRange(RoomActions()); //7
            choices.AddRange(BathRoomActions()); //1
            choices.AddRange(DialogueChoiceActions());//28
        }

        //called by all actions with action id - check if action repeated
        //and do reapeat stuff if true -  modifies the repeted varibles and calls to end if > num?
        public void CheckRepeat(string id){
            if(lastActionID != id)
            {
                repeat = 0;
                return;
            }
            repeat += 1;
            if(repeat > 3){
                //death
                //if (lastActionID == 1)//sleep
                //paranoia death
            }
        }

        public void checkDeath(){
            if(killFactor > 3){
                //death
            }
        }

        public void endOfDay(){
            //private int cleanFactor; // if < 3 kill - end of day only
            //private int hungerFactor; // must eat 2 meals during the day if counter < 2 kill - end of day
            //private int workFactor;  // must reach 3 point by the end of the day to survive - end of day
            //private int killFactor; // 3 will kill 
            if(cleanFactor < 5){
                // clean death
            }
            else if(hungerFactor < 2){}
            else if(workFactor < 3){}
            else if(killFactor > 3){}
            else{
                // survive
            }
        }

        // actions that can be accessed from more then one room - 6
        public List<Choice> generalActions(){
            List<Choice> actions = new List<Choice>();
            // bed + couch
            actions.Add(new Choice("sleep", () => {
                CheckRepeat("sleep");
                time = time.AddMinutes(60);
                lastActionID = "sleep";
            }));
            actions.Add(new Choice("nap", () => {
                CheckRepeat("nap");
                time = time.AddMinutes(30);
                lastActionID = "nap";
            }));
            actions.Add(new Choice("jump", () => {
                CheckRepeat("jump");
                time = time.AddMinutes(30);
                lastActionID = "jump";
            }));

            // tv + pc
            actions.Add(new Choice("play games", () => {
                CheckRepeat("play");
                //open menu - day in life 2020 edition / metrix game / ... / ... 
                lastActionID = "play";
            }));
            actions.Add(new Choice("netflix&chill", () => {
                CheckRepeat("watch");
                time = time.AddMinutes(60);
                lastActionID = "watch";
            }));
            // bathroom + kitchen
            actions.Add(new Choice("wash hands", () => {
                CheckRepeat("washHands");
                time = time.AddMinutes(5);
                if(lastActionID == "number2")
                {
                    cleanFactor = cleanFactor +1;
                }
                lastActionID = "washHands";
            }));
            return actions;
        }

        private List<Choice> RoomActions(){
            List<Choice> actions = new List<Choice>();
            //bed
            actions.Add(new Choice("profess love", () => {
                CheckRepeat("loveBed");
                time = time.AddMinutes(60);
                lastActionID = "loveBed";
            }));
            //closet
            //black screen - options to hide(repeat) or leave
            actions.Add(new Choice("change clothes", () => {
                CheckRepeat("clothes");
                time = time.AddMinutes(10);
                lastActionID = "clothes";
            }));
            actions.Add(new Choice("hide", () => {
                Panel.SetActive(true);
                time = time.AddMinutes(30);
                lastActionID = "hide";
            }));
            actions.Add(new Choice("go out", () => {
                Panel.SetActive(false);
                killFactor = killFactor -1;
                lastActionID = "LGBT";
            }));
            actions.Add(new Choice("stay", () => {
                CheckRepeat("hide");
                time = time.AddMinutes(30);
                lastActionID = "hide";
            }));
            //pc
            actions.Add(new Choice("work", () => {
                CheckRepeat("work");
                //asks how long - 0.5, 1, 2, 4 - those are the points
                //open menu - 0.5, 1, 2, 4  choices
                lastActionID = "work";
            }));
            actions.Add(new Choice("call friend", () => {
                CheckRepeat("call");
                //open talk interaction
                lastActionID = "call";
            }));
            return actions;
        }

        private List<Choice> BathRoomActions(){
            List<Choice> actions = new List<Choice>();
            //toilet
            actions.Add(new Choice("number 2", () => {
                CheckRepeat("number2");
                time.AddMinutes(30);
                lastActionID = "number2";
            }));
            return actions;
        }
        public void DialogueMenu(Dialogue dialogue){
            FindObjectOfType<DialogueManager>().StartActionChoice(dialogue);
        }
        public void DialogueMenu(DialogueTree dialogue){
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }

        private List<Choice> DialogueChoiceActions(){
            List<Choice> actions = new List<Choice>();
            //kitchen
            //t1
            actions.Add(new Choice("yes", () => {
                DialogueMenu(t.getDialogueById(2));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("no", () => {
                lastActionID = "Talk";
            }));
            //t2
            actions.Add(new Choice("then you should go", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("and?", () => {
                DialogueMenu(t.getDialogueById(5));
                lastActionID = "Talk";
            }));
            //bath - 4+
            //t1
            actions.Add(new Choice("you do this every morning", () => {
                killFactor += 1;
                DialogueMenu(t.getDialogueById(1));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("fine", () => {
                time = time.AddMinutes(30);
                lastActionID = "Talk";
            }));
            //t2 
            actions.Add(new Choice("when will you finish?", () => {
                killFactor += 1;
                DialogueMenu(t.getDialogueById(7));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("stop! you're out of tune", () => {
                killFactor += 1;
                DialogueMenu(t.getDialogueById(7));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("leave", () => {
                lastActionID = "Talk";
            }));
            //living 9+
            //t1
            actions.Add(new Choice("clean the house", () => {
                time = time.AddMinutes(30);
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("it's Maya's turn, not mine", () => {
                DialogueMenu(t.getDialogueById(3));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("clean? i don't clean", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            //t2
            actions.Add(new Choice("ignore, watch tv", () => {
                DialogueMenu(t.getDialogueById(4));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("whatcha doin?", () => {
                DialogueMenu(t.getDialogueById(4));
                lastActionID = "Talk";
            }));
            //contDialogs 14+
            //t1
            actions.Add(new Choice("go away!", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("give up", () => {
                time = time.AddMinutes(15);
                lastActionID = "Talk";
            }));
            //t2
            actions.Add(new Choice("ok", () => {
                cleanFactor += 1;
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("no way", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            //t3 -18+
            actions.Add(new Choice("leave", () => {
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("wait", () => {
                time = time.AddMinutes(60);
                DialogueMenu(t.getDialogueById(5));
                lastActionID = "Talk";
            }));
            //t4
            actions.Add(new Choice("goto supermarket", () => {
                Debug.Log("Death - death");
            }));
            actions.Add(new Choice("buy online", () => {
                time = time.AddMinutes(30);
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("no", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            //t5
            actions.Add(new Choice("wait", () => {
                time = time.AddMinutes(30);
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("leave", () => {
                lastActionID = "Talk";
            }));
            //calling friend choices - 25+
            //batman
            actions.Add(new Choice("save the world", () => {
                Debug.Log("Death - death");
            }));
            actions.Add(new Choice("sing i'm not batman im spider-pigX3", () => {
                lastActionID = "Talk";
            }));
            //superman
            actions.Add(new Choice("go back", () => {
                Debug.Log("Death - death");
            }));
            // more - 28+
            return actions;
        }

        //can be acsessed from computer -> 2nd menu for work
        //modifies last action and time and points
        public void Work(int points){
            time.AddMinutes(points*30);
            workFactor = workFactor + points;
            // lastActionID = 5;  
        }

        //can be acsessed from computer or TV -> 2nd menu for Game
        //modifies last action and time and death acordinly
        public void Play(string gameName)
        {
            //update time;
            // lastActionID = 6; 
            if( gameName == "2020" ) 
            {
                //world collapses
            }
        }

        //can be acsessed from TV 
        //options - netflix/news/game
        public void openTV(){
            //open menu -  netflix/news/game
        }

        //can be acsessed from TV - > 2nd menu
        //modifies last action and time
        // public void WatchNews(){
        //     CheckRepeat(9);
        //     time.AddMinutes(60);
        //     lastActionID = 9; 
        // }

        ///can be acsessed from  
        //modifies last action and time
        // public void ReadMagazins(){
        //     CheckRepeat(10);
        //     time.AddMinutes(20);
        //     lastActionID = 10; 
        // }

        ///can be acsessed from kitchen
        //modifies last action and time
        // public void Eat(){
        //     CheckRepeat(13);
        //     time.AddMinutes(30);
        //     if (lastActionID == 11)
        //     {
        //         cleanFactor = cleanFactor +1;
        //     }
        //     lastActionID = 13; 
        // }

        ///can be acsessed from kitchen
        //modifies last action and time
        // public void Drink(){
        //     CheckRepeat(14);
        //     time.AddMinutes(5);
        //     lastActionID = 14; 
        // }

    }
}

