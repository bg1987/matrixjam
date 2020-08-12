using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class UpdateDirectionArrowComponent : MonoBehaviour
    {
        SpriteRenderer arrowRenderer;
        SpriteRenderer doorRenderer;
        DoorComponent door;
        // Start is called before the first frame update
        void Start()
        {
            door = GetComponentInParent<DoorComponent>();
            doorRenderer = GetComponentInParent<SpriteRenderer>();
            arrowRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.right = door.ArrowDirection();
            /*if (doorDirection.y * doorDirection.y == 1 || doorDirection.x == -1)
            {
                arrowRenderer.flipX = true;
            }
            else
            {
                arrowRenderer.flipX = false;
            }*/

            //arrowRenderer.enabled = door.Connected();
            //arrowRenderer.flipX = door.Flipped();
        }
    }
}
