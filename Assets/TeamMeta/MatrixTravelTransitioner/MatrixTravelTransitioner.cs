using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MatrixJam.TeamMeta
{
    public class MatrixTravelTransitioner : MonoBehaviour
    {
        [SerializeField] MatrixTraveler matrixTraveler;
        [SerializeField] float transitionDuration = 1;
        public bool isTransitioning = false;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void Transition(MatrixEdgeData matrixEdgeData)
        {
            
            StartCoroutine(TransitionRoutine());
        }
        IEnumerator TransitionRoutine()
        {
            bool success = matrixTraveler.travelData.TryGetLastTravel(out MatrixEdgeData lastTravel);
            if(!success)
            {
                Debug.Log("Travel history is blank");
                yield break;
            }

            isTransitioning = true;

            yield return new WaitForSeconds(transitionDuration);
            MatrixNodeData destinationGame = matrixTraveler.matrixGraphData.nodes[lastTravel.endPort.nodeIndex];
            SceneManager.LoadScene(destinationGame.scenePath);

            isTransitioning = false;
        }
    }
}
