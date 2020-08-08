using UnityEngine;

namespace MatrixJam.Team4
{
    public class HumanPlayer : Player
    {
        
        public override Color Color()
        {
            return UnityEngine.Color.blue;
        }

        public override bool IsHuman()
        {
            return true;
        }

        public override void YourTurn(TurnData turnData)
        {
            base.YourTurn(turnData);
            
        }

        public override void EndTurn(TurnObject turnObject)
        {
            base.EndTurn(turnObject);
        }
    }
}