using System.Collections;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class FirstOptionPlayer : Player
    {

        public override bool IsHuman()
        {
            return false;
        }

        public override void YourTurn(TurnData turnData)
        {
            base.YourTurn(turnData);
            StartCoroutine(MakeDumbChoices(turnData));
        }

        private IEnumerator MakeDumbChoices(TurnData turnData)
        {
            yield return new WaitForSeconds(3);
            var unit = turnData._positionOptions.Keys.First(x => turnData._positionOptions[x].Count > 0);
            
            UIManager.ChoiceManager.PickNumber(unit.Value);
            var positions = turnData._positionOptions[unit];
            var position = positions[0];
            UIManager.ChoiceManager.PickSquare(new Vector2(position.GetX(), position.GetY()));
            UIManager.ChoiceManager.PickAttack(AttackDirection.square);
            
        }
    }
}