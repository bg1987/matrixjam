using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta
{
    public class CreditsRoll : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect scroller;

        // Update is called once per frame
        void Update()
        {
            scroller.velocity = new Vector2(0f, 10f);
        }
    }
}
