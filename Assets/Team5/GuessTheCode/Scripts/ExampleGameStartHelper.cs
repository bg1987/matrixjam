using UnityEngine;

namespace MatrixJam.Team5
{
    public class ExampleGameStartHelper : StartHelper
    {
        public override void StartHelp(int num_ent)
        {
            // this is how the game starts
            Debug.Log("start: " + num_ent);
        }
    }
}