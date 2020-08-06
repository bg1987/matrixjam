using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class DetectGround : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Ground"))
            {
                //Debug.Log(collision.transform.name);

                //player.grounded = true;
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            //player.grounded = false;
        }
    }
}
