using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixStarter : StartHelper
    {
        [SerializeField] MatrixTraveler matrixTraveler;
        [Header("-1 => Select Random Game")]
        [SerializeField] int startingGameIndex = 0;
        // Start is called before the first frame update
        void Start()
        {
            if (startingGameIndex == -1)
                matrixTraveler.WarpToRandomGame();
            else
            {
                //ToDo Change -3 into something that makes sense
                matrixTraveler.WarpTo(startingGameIndex,-1);
            }

        }
    }
}
