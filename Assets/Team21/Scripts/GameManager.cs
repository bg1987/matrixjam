using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team21 {
    public class GameManager : MonoBehaviour {
		public Animator introText;
		public float mouseIndicatorWait = 30f;
		public float mouseIndicatorTimeToShow = 5f;
		public GameObject mouseIndicator;

		private IEnumerator mouseIndicatorWaitCoroutine;

		void Start() {
			mouseIndicatorWaitCoroutine = MouseIndicatorWaitRoutine();
			StartCoroutine(mouseIndicatorWaitCoroutine);
		}

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
				GameState.state = State.PLAY;
				introText.SetBool("IsPlay", true);
				mouseIndicator.SetActive(false);
				StopCoroutine(mouseIndicatorWaitCoroutine);
			}
        }

		IEnumerator MouseIndicatorWaitRoutine() {
			yield return new WaitForSeconds(mouseIndicatorWait);
			mouseIndicator.SetActive(true);
			// StartCoroutine(HideMouseIndicatorRoutine());
			yield return null;
		}

		IEnumerator HideMouseIndicatorRoutine() {
			yield return new WaitForSeconds(mouseIndicatorTimeToShow);
			mouseIndicator.SetActive(false);
			yield return null;
		}
    }
}
