using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MatrixJam.Team21 {
    public class Ball : MonoBehaviour {
		public bool inWindZone = false;
		public GameObject windZone;
		public float delayStart = 5f;
		public float strengthBoost = 5f;
		public float restartDelay = 2f;
		public float delaySeagullEffect = 2f;

		private Rigidbody rb;
		private WindArea windArea;
		private bool isHitBySeagull = false;

        // Start is called before the first frame update
        void Start() {
			rb = GetComponent<Rigidbody>();
			// print("SceneManager.GetActiveScene(); " + SceneManager.GetActiveScene());
			// StartCoroutine(DelayStartRoutine());
        }

		void FixedUpdate() {
			if (GameState.state == State.PLAY && !rb.useGravity) {
				rb.useGravity = true;
			}
			if (inWindZone && !isHitBySeagull) {
				float strength = windArea.strength;
				if (Input.GetMouseButtonDown(0)) {
					strength += strengthBoost;
				}
				rb.AddForce(windArea.direction * strength, ForceMode.Impulse);
			}
		}

        void OnTriggerEnter(Collider col) {
			if (col.gameObject.tag == "Tag0") {// windarea
				// windZone = col.gameObject;
				windArea = col.gameObject.GetComponent<WindArea>();
				inWindZone = true;
			}

			if (col.gameObject.tag == "Tag2") {// killzone
				print("killed!");
				StartCoroutine(RestartLevel());
			}
		}

		void OnTriggerExit(Collider col) {
			if (col.gameObject.tag == "Tag0") {// windarea
				inWindZone = false;
			}
		}

		void OnCollisionEnter(Collision collision) {
			if (collision.collider.tag == "Tag3") {// obstacle
				inWindZone = false;
			}
			if (collision.collider.tag == "Tag6") {// seagull
				isHitBySeagull = true;
				StartCoroutine(DisableSeagullEffectRoutine());
			}
		}

		IEnumerator DisableSeagullEffectRoutine() {
			yield return new WaitForSeconds(delaySeagullEffect);
			isHitBySeagull = true;
			yield return null;
		}

		IEnumerator DelayStartRoutine() {
			yield return new WaitForSeconds(delayStart);
			rb.useGravity = true;
			yield return null;
		}

		IEnumerator RestartLevel() {
			yield return new WaitForSeconds(restartDelay);
			GameState.state = State.PAUSE;
			LevelHolder.Level.Restart();
			yield return null;
		}
    }
}
