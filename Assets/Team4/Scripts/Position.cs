using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class Position 
    {
        private int posX;
        private int posY;

        public void setPosition(int x, int y)
        {
            posX = x;
            posY = y;
        }

        public int getX()
        {
            return posX;
        }

        public int getY()
        {
            return posY;
        }
    }
}
