using System.Collections;
using System.Collections.Generic;
using MatrixJam;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class EmptyGameStartHelper : StartHelper
    {
       
    public override void StartHelp(int num_ent)
    {
        // this is how the game starts
        Debug.Log("Player Entered through entrance number:" + num_ent);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    }
}
