using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class AlonAI : Player
    {
        
        // this is an AI which chooses a random unit and returns a random answer
        public override void YourTurn(TurnData turnData)
        {
            base.YourTurn(turnData);
            if (GameManager.PlayRandom)
            {
                var turnObject = MakeDecision(turnData);
                StartCoroutine(UIManager.ChoiceManager.HandleAiChoice(ValidateTurnObject(turnObject)));
            }
            else
            {
                var turnObject = FirstOptionPlayer.CreateDecision(turnData);//Got too lazy to switch the MonoBehavior
                StartCoroutine(UIManager.ChoiceManager.HandleAiChoice(ValidateTurnObject(turnObject)));
            }

        }

        private TurnObject MakeDecision(TurnData turnData)
        {
            var randomIndex = Random.Range(0, MyUnits.Count);
            var randomUnit = MyUnits[randomIndex];
            var unitOptions = turnData._positionOptions[randomUnit];
            var positionIndex = Random.Range(0, unitOptions.Count);
            var position = unitOptions[positionIndex];

            randomUnit.Position = position;
            var turnObject = new TurnObject();
            turnObject.ChosenUnit = randomUnit;

            randomIndex = Random.Range(0, 2);
            switch (randomIndex)
            {
                case 0:
                    turnObject.AttackDirection = AttackDirection.row;
                    break;
                case 1:
                    turnObject.AttackDirection = AttackDirection.square;
                    break;
                case 2:
                    turnObject.AttackDirection = AttackDirection.colum;
                    break;
            }

            return turnObject;
        }
    }
}
