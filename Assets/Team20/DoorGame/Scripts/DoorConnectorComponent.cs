using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class DoorConnectorComponent : MonoBehaviour
    {
        Connection connection = new Connection();
        // Start is called before the first frame update
        void Start()
        {
            
        }

        DoorPlaceComponent ClosestDoorPlace()
        {
            return this.GetComponent<CurrentDoorPlaceComponent>().currentDoorPlace;
        }

        DoorComponent ClosestDoor()
        {
            var doorPlace = ClosestDoorPlace();
            if (!doorPlace)
                return null;

            return doorPlace.placedDoor;
        }

        T FindClosest<T>() where T: UnityEngine.MonoBehaviour
        {
            var minDist = float.MaxValue;
            T bestDoor = null;
            foreach (var door in Object.FindObjectsOfType<T>())
            {
                if (!door.enabled)
                    continue;
                var dist = Vector3.Distance(door.transform.position, this.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    bestDoor = door;
                }
            }

            return bestDoor;
        }

        void ConnectDoor()
        {
            var door = ClosestDoor();
            if (door != null)
            {
                if (door.Connected())
                {
                    door.Disconnect();
                }
                else
                {
                    connection.AddDoor(door);
                }
            }
        }

        void FlipConnectionDirection()
        {
            var door = ClosestDoor();
            if (door && door.Connected())
                door.FlipDirection();
        }

        DoorComponent currentDoor;

        void PickPlaceDoor()
        {
            var doorPlace = ClosestDoorPlace();
            if (!doorPlace)
                return;

            if(!currentDoor)
            {
                currentDoor = doorPlace.PickDoor();
            }
            else
            {
                doorPlace.PlaceDoor(currentDoor);
                currentDoor = null;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.P))
            {
                ConnectDoor();
            }

            if (Input.GetKeyUp(KeyCode.O))
            {
                FlipConnectionDirection();
            }

            if (Input.GetKeyUp(KeyCode.I))
            {
                PickPlaceDoor();
            }
        }
    }
}
