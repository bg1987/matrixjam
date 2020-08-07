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

        DoorComponent ClosestDoor()
        {
            var minDist = float.MaxValue;
            DoorComponent bestDoor = null;
            foreach (var obj in Object.FindObjectsOfType<DoorComponent>())
            {
                var door = obj as DoorComponent;
                var dist = Vector3.Distance(door.transform.position, this.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    bestDoor = door;
                }
            }

            return bestDoor;
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.P))
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

            if (Input.GetKeyUp(KeyCode.O))
            {
                var door = ClosestDoor();
                if (door.Connected())
                    door.FlipDirection();
            }
        }
    }
}
