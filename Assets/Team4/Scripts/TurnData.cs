using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class TurnData
    {
        public TurnObject turnObject;
        private BoardData _boardData;
        public Dictionary<Unit, List<Position>> _positionOptions;

        public TurnData(BoardData boardData, Dictionary<Unit, List<Position>> positionOptions)
        {
            _boardData = boardData;
            _positionOptions = positionOptions;
            turnObject = new TurnObject();
        }
    }
}
