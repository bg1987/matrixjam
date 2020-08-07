using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class TurnData
    {
        public TurnObject turnObject;
        private BoardData _boardData;
        private PositionOptions _positionOptions;

        public TurnData(BoardData boardData, PositionOptions positionOptions)
        {
            _boardData = boardData;
            _positionOptions = positionOptions;
            turnObject = new TurnObject();
        }
    }
}
