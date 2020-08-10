using System.Collections;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class FirstOptionPlayer : Player
    {

        public override void YourTurn(TurnData turnData)
        {
            base.YourTurn(turnData);
            var turnObject = CreateDecision(turnData);
            StartCoroutine(UIManager.ChoiceManager.HandleAiChoice(ValidateTurnObject(turnObject)));
        }

        public static TurnObject CreateDecision(TurnData turnData)
        {
            var unit = turnData._positionOptions.Keys.First(x => turnData._positionOptions[x].Count > 0);
            var positions = turnData._positionOptions[unit];
            var position = positions[0];
            var turnObject = new TurnObject();
            turnObject.AttackDirection = AttackDirection.square;
            turnObject.ChosenUnit = unit;
            return turnObject;
        }
    }
}