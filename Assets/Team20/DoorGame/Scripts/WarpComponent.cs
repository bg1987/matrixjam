using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class WarpComponent : MonoBehaviour
    {
        DoorComponent currentDoor;
        bool isRightToDoor;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!currentDoor)
                return;

            if (IsRightToDoor() == isRightToDoor)
                return;

            if (currentDoor.Connected())
            {
                Warp();
            }
            else
            {
                TryGoNextLevel();
            }
        }

        void TryGoNextLevel()
        {
            if (!currentDoor.currentPlace.goToNextLevel)
                return;

            if (!GetComponent<PlayerComponent>())
                return;

            SceneManagerComponent.instance.GoToNextLevel();
        }

        void Warp()
        {
            Debug.Log("Warp");
            var doorToSelf = DoorToSelf();
            var doorToWarp = currentDoor.ConnectedDoor();
            var angle = Vector2.Angle(currentDoor.Direction(), doorToWarp.Direction());

            var playerComponent = GetComponent<PlayerComponent>();
            var movementComponent = GetComponent<MovementComponent>();
            var enemyComponent = GetComponent<EnemyControllerComponent>();

            if(movementComponent)
            {
                transform.position = doorToWarp.transform.position + new Vector3(doorToSelf.x, doorToSelf.y);
                var originalVelocity = new Vector2(movementComponent.velocity.x, movementComponent.velocity.y);
                movementComponent.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * movementComponent.velocity;
                //this.transform.Translate(Time.deltaTime * movementComponent.velocity);

                var outDirection = doorToWarp.Direction();
                if(isRightToDoor)
                {
                    outDirection = -outDirection;
                }

                if(Mathf.Abs(outDirection.y) > 0.9 && doorToWarp.Direction().y == -currentDoor.Direction().y)
                {
                    if(outDirection.y > 0.9)
                    {
                        movementComponent.velocity = new Vector2(0f, movementComponent.velocity.y) * 0.8f;
                    }
                    movementComponent.velocity.x = originalVelocity.x;
                }

                if(playerComponent)
                {
                    playerComponent.resetHorizontal = true;
                }

                if(angle >= 170 && enemyComponent)
                {
                    enemyComponent.movementX *= -1f;
                }
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
            return Vector2.Dot(DoorToSelf(), currentDoor.Direction()) > 0;
        }

        public void OnEnterDoor(DoorComponent door)
        {
            currentDoor = door;
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
