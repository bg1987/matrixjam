using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team13
{
    public class Doorway : MonoBehaviour {
		[SerializeField] private Exit _exit;

		private void OnTriggerEnter(Collider collider){
			Debug.Log(collider.gameObject.name + " has left the building!");
		}
    }
}
