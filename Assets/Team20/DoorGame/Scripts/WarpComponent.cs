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
        public float setIgnoreHorizontal = 0.25f;
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
            var playerComponent = GetComponent<PlayerComponent>();
            var doorToSelf = DoorToSelf();
            var doorToWarp = currentDoor.ConnectedDoor();
            var angle = Vector2.Angle(currentDoor.Direction(), doorToWarp.Direction());
            
            transform.position = doorToWarp.transform.position + new Vector3(doorToSelf.x, doorToSelf.y);
            playerComponent.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * playerComponent.velocity;
            //playerComponent.ignoreHorizontalFor = setIgnoreHorizontal;
            playerComponent.resetHorizontal = true;
            this.transform.Translate(Time.deltaTime * playerComponent.velocity);
            //transform.rotation = doorToWarp.transform.rotation * Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
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
