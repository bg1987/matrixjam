using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class Position 
    {
        private int posX;
        private int posY;

        public void SetPosition(int x, int y)
        {
            posX = x;
            posY = y;
        }

        public int GetX()
        {
            return posX;
        }

        public int GetY()
        {
            return posY;
        }
    }
}
