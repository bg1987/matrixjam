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

		private Rigidbody rb;
		private WindArea windArea;

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
			if (inWindZone) {
				float strength = windArea.strength;
				if (Input.GetMouseButtonDown(0)) {
					strength += strengthBoost;
				}
				rb.AddForce(windArea.direction * strength, ForceMode.Impulse);
			}
		}

        void OnTriggerEnter(Collider col) {
			if (col.gameObject.tag == "Tag0") {
				// windZone = col.gameObject;
				windArea = col.gameObject.GetComponent<WindArea>();
				inWindZone = true;
			}

			if (col.gameObject.tag == "Tag2") {
				print("killed!");
				StartCoroutine(RestartLevel());
			}
		}

		void OnTriggerExit(Collider col) {
			if (col.gameObject.tag == "Tag0") {
				inWindZone = false;
			}
		}

		IEnumerator DelayStartRoutine() {
			yield return new WaitForSeconds(delayStart);
			rb.useGravity = true;
			yield return null;
		}

		IEnumerator RestartLevel() {
			yield return new WaitForSeconds(restartDelay);
			LevelHolder.Level.Restart();
			yield return null;
		}
    }
}
