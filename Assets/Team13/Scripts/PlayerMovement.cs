using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team13
{
    public class PlayerMovement : MonoBehaviour{
        private float _x;
		private float _z;
		private Vector3 _move;

		private enum moveType {Walking, Running, Sneaking};
		private moveType _moveState = moveType.Walking;

		[SerializeField] private CharacterController _controller;
		[SerializeField] private float _walkSpeed = 10f;
		[SerializeField] private float _runSpeed = 15f;
		[SerializeField] private float _sneakSpeed = 5f;
		private float _speed;

		void Start(){
			_speed = _walkSpeed;
		}
        
        void Update(){
            _x = Input.GetAxis("Horizontal");
			_z = Input.GetAxis("Vertical");

			if(Input.GetKeyDown(KeyCode.LeftControl)){
				if(_moveState == moveType.Sneaking){
					_moveState = moveType.Walking;
					_speed = _walkSpeed;
				}else{
					_moveState = moveType.Sneaking;
					_speed = _sneakSpeed;	
				}
			}

			if(Input.GetKeyDown(KeyCode.LeftShift)){
				if(_moveState == moveType.Running){
					_moveState = moveType.Walking;
					_speed = _walkSpeed;
				}else{
					_moveState = moveType.Running;
					_speed = _runSpeed;
				}
			}

			_move.x = _x;
			_move.z = _z;

			_move = transform.right * _x + transform.forward * _z;

			_controller.Move(_move * _speed * Time.deltaTime);
			//Debug.Log(_moveState);
        }
    }
}
