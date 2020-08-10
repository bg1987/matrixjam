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
        public bool goToNextLevel = false;
        public bool canBeConnected = true;
        public bool canRemoveDoor = true;
        public Exit gameExit = null;
        public DoorComponent placedDoor = null;
        public Collider2D wallCollider = null;
        public SpriteRenderer spriteRenderer;
        public AudioSource doorAudio;
        public bool openedByPlayer;
        public Material setMaterial;
        Material originalMaterial;

        // Start is called before the first frame update
        void Start()
        {
            doorAudio = GameObject.Find("SFX_Manager").GetComponent<AudioSource>();
            if (placedDoor)
            {
                spriteRenderer.enabled = false;
                placedDoor.currentPlace = this;
                //placedDoor.gameObject.GetComponent<SpriteRenderer>().material = spriteRenderer.material;
                if (wallCollider != null)
                    wallCollider.enabled = false;
                placedDoor.transform.position = this.transform.position;
                placedDoor.transform.rotation = this.transform.rotation;
                placedDoor.transform.parent = this.transform;
                placedDoor.transform.localScale = Vector3.one;
            }
        }

        // Update is called once per frame
        void Update()
        {

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
            spriteRenderer.enabled = true;

            if (setMaterial)
            {
                door.gameObject.GetComponent<SpriteRenderer>().material = originalMaterial;
            }

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

            if (!canBeConnected && placedDoor.Connected())
                placedDoor.Disconnect();

            if (wallCollider != null)
                wallCollider.enabled = false;

            if(setMaterial)
            {
                var spr = placedDoor.gameObject.GetComponent<SpriteRenderer>();
                originalMaterial = spr.material;
                spr.material = setMaterial;
            }

            spriteRenderer.enabled = false;

            placedDoor.transform.SetParent(this.transform);
            placedDoor.transform.localScale = Vector3.one;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (placedDoor)
            {
                var animator = placedDoor.gameObject.GetComponent<Animator>();
                if (animator)
                {
                    if (other.CompareTag("Tag0"))
                        openedByPlayer = true;
                    else
                        openedByPlayer = false;
                    PlayCurrentDoorSound();
                    animator.SetBool("IsOpen", true);
                }
            }

            var component = other.GetComponent<CurrentDoorPlaceComponent>();
            if (!component)
                return;

            component.currentDoorPlace = this;
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (placedDoor)
            {
                var animator = placedDoor.gameObject.GetComponent<Animator>();
                if (animator)
                {
                    if (other.CompareTag("Tag0"))
                        openedByPlayer = true;
                    else
                        openedByPlayer = false;
                    PlayCurrentDoorSound();
                    animator.SetBool("IsOpen", false);
                }
            }

            var component = other.GetComponent<CurrentDoorPlaceComponent>();
            if (!component)
                return;

            if (component.currentDoorPlace != this)
                return;

            component.currentDoorPlace = null;
        }

        public void PlayCurrentDoorSound()
        {
            if(openedByPlayer)
                doorAudio.Play();
        }
    }
}
