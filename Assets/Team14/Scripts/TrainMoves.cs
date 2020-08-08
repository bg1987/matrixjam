using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team14
{
    public static class TrainMoves
    {
        private static Dictionary<TrainMove, KeyCode> _moveDict = new Dictionary<TrainMove, KeyCode>
        {
            {TrainMove.Jump, KeyCode.UpArrow},
            {TrainMove.Duck, KeyCode.DownArrow},
            {TrainMove.Honk, KeyCode.Space},
        };

        public static KeyCode GetKey(TrainMove move)
        {
            return _moveDict[move];
        }
    }
}