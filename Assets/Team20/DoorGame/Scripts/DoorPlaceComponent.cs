using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class DoorPlaceComponent : MonoBehaviour
    {
        public DoorComponent placedDoor = null;
        public Collider2D wallCollider = null;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public DoorComponent PickDoor()
        {
            if (!placedDoor)
                return null;

            var door = placedDoor;
            placedDoor = null;

            if (wallCollider != null)
                wallCollider.enabled = true;

            door.enabled = false;
            return door;
        }

        public void PlaceDoor(DoorComponent door)
        {
            if (placedDoor != null)
                return;

            door.transform.position = this.transform.position;
            door.transform.rotation = this.transform.rotation;
            door.enabled = true;
            placedDoor = door;
            if (wallCollider != null)
                wallCollider.enabled = false;
        }
    }
}
