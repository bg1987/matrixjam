using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class Unit
    {
        private Player _owner;
        private int _value;
        private Position position;

        public Unit(Player player, int value)
        {
            _owner = player;
            _value = value;
        }

        public Position Position { get => position; set => position = value; }
    }

    public enum Direction
    {
        row,
        colum,
        square
    }

}
