using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class Script2Animate : MonoBehaviour
    {
        public Animator animator;
        public GameObject steps;

        private bool down, left, up, right;

        void updateKeysState(){
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
                down = true;
            }
            else if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)){
                down = false;
            }

            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
                up = true;
            }
            else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)){
                up = false;
            }

            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
                left = true;
            }
            else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)){
                left = false;
            } 

            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
                right = true;
            }
            else if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)){
                right = false;
            }
        }

        void Update()
        {
            updateKeysState();
            if (down)
            {
                animator.SetTrigger("Foward");
                steps.SetActive(true);
            }
            else if (up)
            {
                animator.SetTrigger("Backwards");
                steps.SetActive(true);
            }
            else if (left)
            {
                animator.SetTrigger("Left");
                steps.SetActive(true);
            }
            else if (right)
            {
                animator.SetTrigger("Right");
                steps.SetActive(true);
            }
            if(!(left || right || up || down))
            {
                animator.SetTrigger("Idel");
                steps.SetActive(false);
            }
        }
    }
}