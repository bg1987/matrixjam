using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MatrixJam.TeamMeta
{
    public class MatrixTravelTransitioner : MonoBehaviour
    {
        [SerializeField] MatrixTraveler matrixTraveler;
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
            MatrixNodeData destinationGame = matrixTraveler.matrixGraphData.nodes[matrixEdgeData.endPort.nodeIndex];
            SceneManager.LoadScene(destinationGame.scenePath);

        }
    }
}
