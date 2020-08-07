using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class TurnManager
    {
        private List<Player> _playersList;
        int _turnIndex;

        public TurnManager(List<Player> players)
        {
            _turnIndex = 0;
            _playersList = players;
        }

        public void NextTurn(TurnData turnData)
        {
            AdvanceIndex();
            _playersList[1].YourTurn(turnData);
        }

        //advances the index keeping record of the players turn, reset if all
        //players had played
        private void AdvanceIndex()
        {
            _turnIndex += 1;
            if ( _turnIndex == _playersList.Count)
            {
                _turnIndex = 0;
            }
        }
    }
}
