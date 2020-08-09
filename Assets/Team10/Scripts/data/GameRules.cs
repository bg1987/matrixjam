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
        private double workFactor;
        private int killFactor;

        public List<Choice> choices;
        public System.DateTime time = System.DateTime.Parse("9:00 AM");
        public Text Timer;
        public GameObject Panel;
        public RandomDialogueTree t;
        public string playerName;

        void Start()
        {
            t = new RandomDialogueTree("player1");
            declareChoices();
        }

        void Update(){
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
            choices = generalActions(); //10
            choices.AddRange(RoomActions()); //11
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

        public void DialogueMenu(Dialogue dialogue){
            FindObjectOfType<DialogueManager>().StartActionChoice(dialogue);
        }
        public void DialogueMenu(DialogueTree dialogue){
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }

        // actions that can be accessed from more then one room - 10
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
                Dialogue a = new Dialogue("I should play...", new int[] {6, 7, 8});
                DialogueMenu(a);
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
            //game-followup
            actions.Add(new Choice("MatrixJam", () => {
                time = time.AddMinutes(45);
            }));
            actions.Add(new Choice("A day in life 2020", () => {
                // insta death DEath
            }));
            actions.Add(new Choice("3", () => {
                time = time.AddMinutes(30);
            }));
            actions.Add(new Choice("4", () => {
                time = time.AddMinutes(15);
            }));
            return actions;
        }

        // total: 11
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
                time = time.AddMinutes(30);
                Panel.SetActive(true);
                Dialogue a = new Dialogue("", new int[] {13, 14});
                DialogueMenu(a);
                lastActionID = "hide";
            }));
            //hide-followup
            actions.Add(new Choice("go out", () => {
                Panel.SetActive(false);
                killFactor -= 1;
                lastActionID = "LGBT";
            }));
            actions.Add(new Choice("stay", () => {
                CheckRepeat("hide");
                time = time.AddMinutes(30);
                Dialogue a = new Dialogue("", new int[] {13, 14});
                DialogueMenu(a);
                lastActionID = "hide";
            }));
            //pc
            actions.Add(new Choice("work", () => {
                CheckRepeat("work");
                Dialogue a = new Dialogue("work for...", new int[] {17, 18, 19, 20});
                DialogueMenu(a);
                lastActionID = "work";
            }));
            actions.Add(new Choice("call friend", () => {
                CheckRepeat("call");
                //open talk interaction
                lastActionID = "call";
            }));
            //work-followup
            actions.Add(new Choice("30 Min", () => {
                time = time.AddMinutes(30);
                workFactor += 0.5;
            }));
            actions.Add(new Choice("1 Hour", () => {
                time = time.AddMinutes(60);
                workFactor += 1;
            }));
            actions.Add(new Choice("2 Hours", () => {
                time = time.AddMinutes(30*4);
                workFactor += 2;
            }));
            actions.Add(new Choice("4 Hours", () => {
                time = time.AddMinutes(30*8);
                workFactor += 4;
            }));
            return actions;
        }

        // total: 4
        private List<Choice> BathRoomActions(){
            List<Choice> actions = new List<Choice>();
            //toilet
            actions.Add(new Choice("number 2", () => {
                CheckRepeat("number2");
                time.AddMinutes(30);
                lastActionID = "number2";
            }));
            //shower + sink
            actions.Add(new Choice("shower", () => {
                CheckRepeat("shower");
                time.AddMinutes(30);
                lastActionID = "shower";
            }));
            actions.Add(new Choice("bubble bath", () => {
                CheckRepeat("shower");
                time.AddMinutes(120);
                lastActionID = "shower";
            }));
            actions.Add(new Choice("swim", () => {
                CheckRepeat("swim");
                Dialogue a = new Dialogue("playerName", "ouch! this is no place to swim...");
                DialogueMenu(a);
                cleanFactor += 1;
                time.AddMinutes(30);
                lastActionID = "swim";
            }));
            return actions;
        }

        // total: 5
        private List<Choice> KitchenActions(){
            List<Choice> actions = new List<Choice>();
            //ref
            actions.Add(new Choice("eat something", () => {
                CheckRepeat("eat");
                time.AddMinutes(30);
                hungerFactor += 1;
                if(playerName == "MATRIX"){
                    Dialogue a = new Dialogue("", new int[] {});
                    DialogueMenu(a);
                }
                //check for more data;
                lastActionID = "eat";
            }));
            actions.Add(new Choice("drink something", () => {
                CheckRepeat("drink");
                time.AddMinutes(5);
                lastActionID = "drink";
            }));
            //sink
            actions.Add(new Choice("do the dishes", () => {
                CheckRepeat("dishes");
                time.AddMinutes(10);
                lastActionID = "dishes";
            }));
            //ref-followup
            actions.Add(new Choice("blue pill", () => {
                lastActionID = "blue";
            }));
            actions.Add(new Choice("red pill", () => {
                lastActionID = "red";
            }));
            return actions;
        } 

        // total: 5
        private List<Choice> LivingRoomActions(){
            List<Choice> actions = new List<Choice>();
            //tv
            actions.Add(new Choice("watch news", () => {
                CheckRepeat("news");
                time.AddMinutes(30);
                //maybe play video?
                lastActionID = "news";
            }));
            //couch
            actions.Add(new Choice("sit", () => {
                CheckRepeat("sit");
                time.AddMinutes(10);
                lastActionID = "sit";
            }));
            //magazine
            actions.Add(new Choice("read articles", () => {
                CheckRepeat("read");
                time.AddMinutes(15);
                lastActionID = "read";
            }));
            actions.Add(new Choice("read news", () => {
                CheckRepeat("read");
                time.AddMinutes(20);
                lastActionID = "read";
            }));
            actions.Add(new Choice("solve puzzles", () => {
                time.AddMinutes(40);
                lastActionID = "puzzle";
            }));
            return actions;
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
    }
}

