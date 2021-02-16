using MatrixJam;
using MatrixJam.TeamMeta;
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
        [SerializeField] GameObject gameBackground;
        // Start is called before the first frame update
        void Awake()
        {
            gameBackground.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                RenderGameAsBackground();
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

            yield return new WaitForSeconds(transitionDuration);
            MatrixNodeData destinationGame = matrixTraveler.matrixGraphData.nodes[lastTravel.endPort.nodeIndex];
            SceneManager.LoadScene(destinationGame.scenePath);

            isTransitioning = false;
        }
        void RenderGameAsBackground()
        {
            gameBackground.SetActive(true);

            Camera[] allCameras = Camera.allCameras;
            Camera cameraInGameScene = Camera.main;

            foreach (var camera in allCameras)
            {
                if (camera == transitionCamera)
                    continue;
                camera.cullingMask = camera.cullingMask & ~transitionLayer;
                camera.targetTexture = gameRenderTexture;
                cameraInGameScene = camera;
            }

            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach (var canvas in canvases)
            {
                if (canvas.gameObject.layer == FirstSetLayer(transitionLayer))
                {
                    continue;
                }
                if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
                {
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = cameraInGameScene;
                }
                var canvasScaler = canvas.GetComponent<CanvasScaler>();
                canvasScaler.enabled = false;
            }
        }
        int FirstSetLayer(LayerMask mask)
        {
            int value = mask.value;
            if (value == 0) return 0;  // Early out
            for (int l = 1; l < 32; l++)
                if ((value & 1 << l) != 0) return l;  // Bitwise
            return -1;  // This line won't ever be reached but the compiler needs it
        }
    }
}
