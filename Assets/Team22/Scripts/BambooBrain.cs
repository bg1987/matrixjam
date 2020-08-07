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

            GameManager.instance.UpdateStats(0, 1);
        }

        public void Slice()
        {
            Destroy(gameObject);
            GameManager.instance.UpdateStats(1, 0);
        }
    }
}
