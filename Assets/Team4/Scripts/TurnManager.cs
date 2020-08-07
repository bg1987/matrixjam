using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class TurnManager
    {
        private List<Player> _playersList;
        int _turnIndex;
        public void NextTurn(TurnData turnData)
        {
            AdvanceIndex();
        }

        private void AdvanceIndex()
        {

        }
    }
}
