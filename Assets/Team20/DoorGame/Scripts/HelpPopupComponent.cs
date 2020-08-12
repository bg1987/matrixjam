using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class HelpPopupComponent : MonoBehaviour
    {
        Canvas canvas;
        // Start is called before the first frame update
        void Start()
        {
            canvas = this.GetComponent<Canvas>();
        }

        // Update is called once per frame
        void Update()
        {
            if(canvas.enabled)
            {
                if(Input.anyKeyDown)
                {
                    canvas.enabled = false;
                }
            }
            else if(Input.GetKeyDown(KeyCode.H))
            {
                canvas.enabled = true;
            }
        }
    }
}
