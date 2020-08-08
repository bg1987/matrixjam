using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public enum PlaceDirection
    {
        Left,
        Right,
        Bot,
        Top
    }

    public class DoorPlaceComponent : MonoBehaviour
    {
        public Transform nextLevelTransform;
        public bool canBeConnected = true;
        public bool canRemoveDoor = true; 
        public DoorComponent placedDoor = null;
        public Collider2D wallCollider = null;
        public PlaceDirection placeDirection = PlaceDirection.Right;
        public Material[] materials = new Material[(int)PlaceDirection.Top + 1];

        // Start is called before the first frame update
        void Start()
        {
            if(placedDoor)
            {
                placedDoor.currentPlace = this;
                placedDoor.gameObject.GetComponent<SpriteRenderer>().material = materials[(int)placeDirection];
                if (wallCollider != null)
                    wallCollider.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //var material = placedDoor.GetComponent<SpriteRenderer>().material;
            //var id = material.GetFloat("sc")
            if(placedDoor)
            {
                placedDoor.transform.position = this.transform.position;
                placedDoor.transform.rotation = this.transform.rotation;
            }
        }

        public DoorComponent PickDoor()
        {
            if (!placedDoor || !canRemoveDoor)
                return null;

            var door = placedDoor;
            placedDoor = null;

            if (wallCollider != null)
                wallCollider.enabled = true;

            door.gameObject.SetActive(false);
            door.currentPlace = null;
            return door;
        }

        public void PlaceDoor(DoorComponent door)
        {
            if (placedDoor != null)
                return;

            door.currentPlace = this;
            door.transform.position = this.transform.position;
            door.transform.rotation = this.transform.rotation;
            door.gameObject.SetActive(true);

            placedDoor = door;
            placedDoor.gameObject.GetComponent<SpriteRenderer>().material = materials[(int)placeDirection];

            if (!canBeConnected && placedDoor.Connected())
                placedDoor.Disconnect();

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
