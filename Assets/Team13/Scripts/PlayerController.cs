using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team13
{
	[RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour {
		private Animator _animator;
        void Awake(){
			_animator = GetComponent<Animator>();
		}

		void Update(){
			//Debug.Log(Input.GetAxis("Vertical"));
			if(Input.GetAxis("Vertical") > 0){
				_animator.SetBool("moving", true);
			}else if(Input.GetAxis("Vertical") == 0){
				_animator.SetBool("moving", false);
			}

			if(Input.GetButtonDown("Fire1")){
				_animator.SetBool("crouching", true);
			}
			if(Input.GetButtonUp("Fire1")){
				_animator.SetBool("crouching", false);
			}

			if(Input.GetButtonDown("Fire3")){
				_animator.SetBool("running", true);
			}

			if(Input.GetButtonUp("Fire3")){
				_animator.SetBool("running", false);
			}

			if(Input.GetButtonDown("Fire2")){
				_animator.SetTrigger("faint");
			}
		}
    }
}
