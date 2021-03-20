using Assets.TeamMeta.MatrixTravelTransition;
using MatrixJam;
using MatrixJam.TeamMeta;
using MatrixJam.TeamMeta.MatrixMap;
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
        [SerializeField] Camera transitionCameraBG;
        [SerializeField] Camera transitionCamera;
        [SerializeField] GameBackground gameBackground;
        [SerializeField] Foreground foreground;
        [SerializeField] MatrixMap matrixMap;

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

            matrixMap.Appear();

            float volumeBeforeMute = AudioListener.volume;

            StartCoroutine(MuteAudioRoutine(gameBackgroundGrayoutDuration));
            yield return new WaitForSeconds(gameBackgroundGrayoutDuration);

            MatrixNodeData destinationGame = matrixTraveler.matrixGraphData.nodes[lastTravel.endPort.nodeIndex];
            SceneManager.LoadScene(destinationGame.scenePath);

            gameBackground.SetToStatic();
            yield return null;
            yield return new WaitForFixedUpdate();
            DeselectCameraMatrixLayersInTransitionedScene();
            StartCoroutine(RestoreAudioRoutine(ForegroundDisappearDuration, volumeBeforeMute));
            foreground.Disappear(ForegroundDisappearDuration);
            gameBackground.StopBlocking();
            yield return new WaitForSeconds(ForegroundDisappearDuration);
            
            gameBackground.Deactivate();
            matrixMap.Disappear();
            isTransitioning = false;
        }
        void DeselectCameraMatrixLayersInTransitionedScene()
        {
            Camera[] cameras = FindObjectsOfType<Camera>();

            foreach (var camera in cameras)
            {
                if (camera == transitionCamera || camera == transitionCameraBG)
                    continue;
                camera.cullingMask = camera.cullingMask & ~transitionLayer;
            }
        }
        IEnumerator MuteAudioRoutine(float duration)
        {
            float count = 0;
            float originVolume = AudioListener.volume;
            while (count<duration)
            {
                var t = count / duration;
                AudioListener.volume = Mathf.Lerp(originVolume, 0, t);
                yield return null;
                count += Time.unscaledDeltaTime;
            }
            AudioListener.volume = 0;
        }
        IEnumerator RestoreAudioRoutine(float duration,float targetVolume)
        {
            float count = 0;
            while (count < duration)
            {
                var t = count / duration;
                AudioListener.volume = Mathf.Lerp(0, targetVolume, t);
                yield return null;
                count += Time.unscaledDeltaTime;
            }
            AudioListener.volume = targetVolume;
        }
    }
}
