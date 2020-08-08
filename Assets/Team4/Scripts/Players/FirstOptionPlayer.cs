using System.Collections;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class RandoPlayer : Player
    {

        public override bool IsHuman()
        {
            return false;
        }

        public override void YourTurn(TurnData turnData)
        {
            StartCoroutine(MakeDumbChoices(turnData));
        }

        private IEnumerator MakeDumbChoices(TurnData turnData)
        {
            yield return new WaitForSeconds(2);
            var keys = turnData._positionOptions.Keys;
            var unit = keys.ToArray()[0];
            UIManager.ChoiceManager.NumberChosen(unit.Value);
            var positions = turnData._positionOptions[unit];
            var position = positions[0];
            UIManager.ChoiceManager.SquareChosen(new Vector2(position.GetX(), position.GetY()));
            UIManager.ChoiceManager.PickAttack(AttackDirection.square);
            
        }
    }
}