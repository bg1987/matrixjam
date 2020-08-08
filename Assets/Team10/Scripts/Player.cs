using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class Player : MonoBehaviour
    {
        public float speed;

        private string playerName;
        private RandomDialogTree t;
        private Rigidbody2D myRigidBody;
        private Vector3 change;

        // Start is called before the first frame update
        void Start()
        {
            t = new RandomDialogTree();
            //myRigidBody = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            // change = Vector3.zero;
            // change.x = Input.GetAxisRaw("Horizontal");
            // change.y = Input.GetAxisRaw("Vertical");
            // if(change != Vector3.zero){
            //     moveCharacter();
            // }   
        }

        void moveCharacter(){
            // myRigidBody.MovePosition(
            //     transform.position + change.normalized * speed * Time.deltaTime
            // );
        }

        void checkForRandomDialogue(string room){
            DialogueTree a;
            bool dialog = t.getRandomDialog(room, out a);
            if(dialog){
                FindObjectOfType<DialogueManager>().StartDialogue(a);
            }
        }
    }
}
