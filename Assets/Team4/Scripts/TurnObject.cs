using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class TurnObject
    {  
        //the unit the player chose
        private Unit _chosenUnit;
        //where the player chose to lay the unit, chosen position is the index of the move
        //out of the possible moves array (see class PositionOptions)
        private Position _chosenPosition;
        //the direction of the attack
        private AttackDirection _attackDirection;

        public Position ChosenPosition { get => _chosenPosition; set => _chosenPosition = value; }
        public AttackDirection AttackDirection { get => _attackDirection; set => _attackDirection = value; }
        public Unit ChosenUnit { get => _chosenUnit; set => _chosenUnit = value; }
    }

}
