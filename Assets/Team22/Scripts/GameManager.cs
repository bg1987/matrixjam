using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team22
{
    public class GameManager : MonoBehaviour
    {
        // stats
        private int hits = 0;
        private int misses = 0;
        // ui
        public Text devText;

        public static GameManager instance;

        private void Awake()
        {
            instance = this;
        }

        public void Update()
        {
            if(devText != null)
            {
                devText.text = "HITS " + hits + "\nMISSES " + misses;
            }
        }

        public void UpdateStats(int addHits, int addMiss)
        {
            hits += addHits;
            misses += addMiss;
        }
    }
}
