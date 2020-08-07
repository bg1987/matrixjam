using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class ConnectionColorManager
    {
        static List<Color> colors = new List<Color>() { Color.red, Color.blue, Color.gray, Color.cyan, Color.yellow };
        public static Color NextColor()
        {
            var index = colors.Count - 1;
            var color = colors[index];
            colors.RemoveAt(index);
            return color;
        }

        public static void ReturnColor(Color color)
        {
            colors.Add(color);
        }
    }

    public class Connection
    {
        DoorComponent firstDoor;
        Color color;

        public Connection()
        {
            
        }

        public void AddDoor(DoorComponent door)
        {
            if (door == null)
                return;

            if (firstDoor == door)
            {
                firstDoor.ResetColor();
                firstDoor = null;
                return;
            }

            if (firstDoor == null)
            {
                firstDoor = door;
                color = ConnectionColorManager.NextColor();
            }
            else
            {
                firstDoor.Connect(door);
                firstDoor = null;
            }

            door.SetColor(color);
        }
    }

    public class DoorComponent : MonoBehaviour
    {
        DoorComponent connectedDoor;
        Color defaultColor = Color.white;
        bool flipDirection = false;

        public float Width()
        {
            return GetComponent<BoxCollider2D>().bounds.extents.x;
        }

        public void FlipDirection()
        {
            flipDirection = !flipDirection;
        }

        public bool Flipped()
        {
            return flipDirection;
        }

        public Vector2 Direction()
        {
            return this.transform.right * (Flipped() ? -1f : 1f);
        }

        Color GetColor()
        {
            return GetComponent<SpriteRenderer>().color;
        }

        public DoorComponent ConnectedDoor() { return connectedDoor; }

        public void ResetColor()
        {
            SetColor(defaultColor);
        }

        public void SetColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
        }

        public void Connect(DoorComponent door)
        {
            connectedDoor = door;
            door.connectedDoor = this;
        }

        public bool Connected() { return connectedDoor != null; }

        public void Disconnect()
        {
            ConnectionColorManager.ReturnColor(GetColor());

            SetColor(defaultColor);
            connectedDoor.SetColor(connectedDoor.defaultColor);

            connectedDoor.connectedDoor = null;
            connectedDoor = null;
        }

        // Start is called before the first frame update
        void Start()
        {
            SetColor(defaultColor);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var warp = collision.gameObject.GetComponent<WarpComponent>();
            if(warp)
                warp.OnEnterDoor(this);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var warp = collision.gameObject.GetComponent<WarpComponent>();
            if (warp)
                warp.OnExitDoor(this);
        }
    }
}
