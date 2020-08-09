using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
    public static class TrainMoves
    {
        private static readonly Dictionary<TrainMove, KeyCode[]> _keyDict = new Dictionary<TrainMove, KeyCode[]>
        {
            {TrainMove.Duck, new [] {KeyCode.DownArrow, KeyCode.S}},
            {TrainMove.Jump, new [] {KeyCode.UpArrow, KeyCode.W}},
            {TrainMove.Honk, new[] {KeyCode.Space}},
        };
        
        public static bool GetKeyDown(TrainMove move)
        {
            var keys = _keyDict[move];
            return keys.Any(Input.GetKeyDown);
        }
        
        public static bool GetKeyHold(TrainMove move)
        {
            var keys = _keyDict[move];
            return keys.Any(Input.GetKey);
        }

        public static bool GetKeyRelease(TrainMove move)
        {
            var keys = _keyDict[move];
            return keys.All(key => !Input.GetKey(key));
        }
    }
}