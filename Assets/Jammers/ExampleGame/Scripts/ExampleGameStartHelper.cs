using System.Collections;
using System.Collections.Generic;
using MatrixJam;
using UnityEngine;

public class ExampleGameStartHelper : StartHelper
{
       
    public override void StartHelp(int num_ent)
    {
        // this is how the game starts
        MatrixJam.Team17.Game.App.OnStart();
    }

}
