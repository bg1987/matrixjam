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
            var doorDirection = door.Direction();
            if(doorDirection.x == -1)
            {
                arrowRenderer.flipX = true;
            }
            else
            {
                arrowRenderer.flipX = false;
                transform.right = door.Direction();
            }

            //arrowRenderer.enabled = door.Connected();
            arrowRenderer.color = doorRenderer.color;
            arrowRenderer.flipX = door.Flipped();
        }
    }
}
