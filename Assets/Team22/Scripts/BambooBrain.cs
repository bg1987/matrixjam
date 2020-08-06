using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class BambooBrain : MonoBehaviour
    {
        public void Miss()
        {
            Destroy(gameObject);
            // TODO: ADD FUCKUP TO MANAGER
        }

        public void Slice()
        {
            // TODO: GAMEFEEL
            Destroy(gameObject);
            print("DESTROYED BAMBOO");
        }
    }
}
