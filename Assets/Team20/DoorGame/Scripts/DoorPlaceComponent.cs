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
            if(placedDoor)
            {
                placedDoor.transform.position = this.transform.position;
                placedDoor.transform.rotation = this.transform.rotation;
            }
        }

        public DoorComponent PickDoor()
        {
            if (!placedDoor)
                return null;

            var door = placedDoor;
            placedDoor = null;

            if (wallCollider != null)
                wallCollider.enabled = true;

            door.gameObject.SetActive(false);
            return door;
        }

        public void PlaceDoor(DoorComponent door)
        {
            if (placedDoor != null)
                return;

            door.transform.position = this.transform.position;
            door.transform.rotation = this.transform.rotation;
            door.gameObject.SetActive(true);
            placedDoor = door;
            if (wallCollider != null)
                wallCollider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var component = other.GetComponent<CurrentDoorPlaceComponent>();
            if (!component)
                return;

            component.currentDoorPlace = this;
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            var component = other.GetComponent<CurrentDoorPlaceComponent>();
            if (!component)
                return;

            if(component.currentDoorPlace == this)
                component.currentDoorPlace = null;
        }
    }
}
