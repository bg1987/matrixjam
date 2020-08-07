using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class WarpComponent : MonoBehaviour
    {
        DoorComponent currentDoor;
        bool isRightToDoor;
        Vector2 doorDirection;
        Vector2 enteringDirection;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!currentDoor || !currentDoor.Connected())
                return;

            if (IsRightToDoor() == isRightToDoor)
                return;

            Warp();
        }

        void Warp()
        {
            var doorToSelf = DoorToSelf();
            var doorToWarp = currentDoor.ConnectedDoor();
            var angle = Vector2.Angle(currentDoor.Direction(), doorToWarp.Direction());

            var playerComponent = GetComponent<PlayerComponent>();
            var movementComponent = GetComponent<MovementComponent>();
            if(playerComponent)
            {
                transform.position = doorToWarp.transform.position + new Vector3(doorToSelf.x, doorToSelf.y);
                playerComponent.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * playerComponent.velocity;

                playerComponent.resetHorizontal = true;
                this.transform.Translate(Time.deltaTime * playerComponent.velocity);
            }
            else if(movementComponent)
            {
                transform.position = doorToWarp.transform.position + new Vector3(doorToSelf.x, doorToSelf.y);
                movementComponent.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * movementComponent.velocity;
                this.transform.Translate(Time.deltaTime * movementComponent.velocity);
            }



            currentDoor = null;
        }

        Vector2 DoorToSelf()
        {
            Vector2 doorPosition = currentDoor.transform.position;
            Vector2 selfPosition = transform.position;
            return selfPosition - doorPosition;
        }

        bool IsRightToDoor()
        {
            return Vector2.Dot(DoorToSelf(), doorDirection) > 0;
        }

        public void OnEnterDoor(DoorComponent door)
        {
            currentDoor = door;
            doorDirection = door.Direction();
            isRightToDoor = IsRightToDoor();
        }

        public void OnExitDoor(DoorComponent door)
        {
            if (door == currentDoor)
            {
                currentDoor = null;
            }
        }
    }
}
