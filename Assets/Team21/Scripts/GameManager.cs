using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team21 {
    public class GameManager : MonoBehaviour {

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonDown(0)) {
				GameState.state = State.PLAY;
			}
        }
    }
}
