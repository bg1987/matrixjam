using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace MatrixJam.Team13{
    [RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(NavMeshAgent))]
    public class AiController : MonoBehaviour {
		private Animator _animator;
		private NavMeshAgent _agent;

		[SerializeField] private Transform[] _poi; //Points of interest
		private int _destPoi;

		/*private enum BehaviourType {Scared, Brave, Dumb, Calculated};
		private BehaviourType _behaviour;
		private int _numBehaviours = 4;*/

		[SerializeField] private float _walkSpeed;
		[SerializeField] private float _runSpeed;
		private float _speed;

		[SerializeField] private UnityEvent _onWalk;
		[SerializeField] private UnityEvent _onRun;

		[SerializeField] private UnityEvent _onMoveStart;
		[SerializeField] private UnityEvent _onMove;
		[SerializeField] private UnityEvent _onMoveEnd;


		void Awake(){
			_animator = GetComponent<Animator>();
			_agent = GetComponent<NavMeshAgent>();
			_destPoi = Random.Range(0, _poi.Length);
			//_behaviour = (BehaviourType)Random.Range(0, 4);
		}

		void Start(){
			GoToNextPoi();
		}

		private void GoToNextPoi(){
			if(_poi.Length == 0){
				return;
			}

			_agent.destination = _poi[_destPoi].position;
			_destPoi = Random.Range(0, _poi.Length);
			_animator.SetBool("moving", true);
			Walk();
		}

		public void SetNewDest(Transform newDest){
			_agent.destination = newDest.position;
		}

		public void Walk(){
			_agent.speed = _walkSpeed;
			_animator.SetBool("running", false);
			_onWalk.Invoke();
		}

		public void Run(){
			_agent.speed = _runSpeed;
			_animator.SetBool("running", true);
			_onRun.Invoke();
		}

		void OnDrawGizmosSelected(){
			Gizmos.color = Color.yellow;
			if(Application.isPlaying){
				Gizmos.DrawLine(transform.position, _agent.destination);
			}
		}

		void Update(){
			if(!_agent.pathPending && _agent.remainingDistance < 0.1f){
				GoToNextPoi();
			}

			/*_moveDir.x = Input.GetAxis("Horizontal");
			_moveDir.z = Input.GetAxis("Vertical");
			_moveDir.y = 0;

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

			_newPos = transform.position + _moveDir * _speed * Time.deltaTime;
			NavMeshHit hit;
			if(NavMesh.SamplePosition(_newPos, out hit, 1f, NavMesh.AllAreas)){
				//Debug.Log((transform.position  - hit.position).magnitude);
				transform.position = hit.position;
				/*if((transform.position  - hit.position).magnitude >= 0.1f){
					
				}*
			}*/
		}
	}
}
