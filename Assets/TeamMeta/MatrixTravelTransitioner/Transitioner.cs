using Assets.TeamMeta.MatrixTravelTransition;
using MatrixJam;
using MatrixJam.TeamMeta;
using MatrixJam.TeamMeta.MatrixTravelTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.TeamMeta.MatrixTravelTransition
{
    public class Transitioner : MonoBehaviour
    {
        [SerializeField] MatrixTraveler matrixTraveler;
        [SerializeField] float transitionDuration = 1;

        public bool isTransitioning = false;

        [SerializeField] RenderTexture gameRenderTexture;
        [SerializeField] LayerMask transitionLayer;
        [SerializeField] Camera transitionCamera;
        [SerializeField] GameBackground gameBackground;
        [SerializeField] Foreground foreground;

        [Header("Effects Durations")]
        [SerializeField] float gameBackgroundGrayoutDuration=2f;
        [SerializeField] float ForegroundDisappearDuration=2f;

        // Start is called before the first frame update
        void Awake()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                gameBackground.RenderGameAsBackground();
                gameBackground.Grayout(gameBackgroundGrayoutDuration);
                foreground.Appear();
            }
        }
        public void Transition(MatrixEdgeData matrixEdgeData)
        {
            StartCoroutine(TransitionRoutine());
        }
        IEnumerator TransitionRoutine()
        {
            bool success = matrixTraveler.travelData.TryGetLastTravel(out MatrixEdgeData lastTravel);
            if (!success)
            {
                Debug.Log("Travel history is blank");
                yield break;
            }

            isTransitioning = true;

            gameBackground.RenderGameAsBackground();
            gameBackground.Grayout(gameBackgroundGrayoutDuration);
            foreground.Appear();

            yield return new WaitForSeconds(Mathf.Max(gameBackgroundGrayoutDuration, transitionDuration));

            MatrixNodeData destinationGame = matrixTraveler.matrixGraphData.nodes[lastTravel.endPort.nodeIndex];
            SceneManager.LoadScene(destinationGame.scenePath);

            gameBackground.SetToStatic();
            yield return null;
            yield return new WaitForFixedUpdate();

            foreground.Disappear(ForegroundDisappearDuration);
            gameBackground.StopBlocking();
            yield return new WaitForSeconds(ForegroundDisappearDuration);
            
            gameBackground.Deactivate();
            isTransitioning = false;
        }
    }
}
