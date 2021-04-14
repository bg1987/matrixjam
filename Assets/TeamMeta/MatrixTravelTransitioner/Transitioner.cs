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

        public bool isTransitioning = false;

        [SerializeField] RenderTexture gameRenderTexture;
        [SerializeField] LayerMask transitionLayer;
        [SerializeField] Camera transitionCameraFG;
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


            float volumeBeforeMute = AudioListener.volume;

            StartCoroutine(MuteAudioRoutine(gameBackgroundGrayoutDuration));
            yield return new WaitForSeconds(gameBackgroundGrayoutDuration);

            gameBackground.SetToStatic();
            yield return null;

            MatrixNodeData destinationGame = matrixTraveler.matrixGraphData.nodes[lastTravel.endPort.nodeIndex];
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(destinationGame.scenePath);
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }
            yield return null;

            float matrixMapAppearDuration = matrixMap.CalculateTotalAppearanceTime();
            matrixMap.Appear();
            yield return new WaitForSeconds(matrixMapAppearDuration);
            //Debug.Log(matrixMapAppearDuration);
            //Debug.Break();
 
            asyncOperation.allowSceneActivation = true;
            while (asyncOperation.isDone == false)
            {
                yield return null;
            }
            yield return new WaitForFixedUpdate();
            DeselectCameraMatrixLayersInTransitionedScene();
            StartCoroutine(RestoreAudioRoutine(ForegroundDisappearDuration, volumeBeforeMute));

            foreground.Disappear(ForegroundDisappearDuration, destinationGame.colorHdr1, destinationGame.colorHdr2);
            gameBackground.StopBlocking();
            yield return new WaitForSeconds(ForegroundDisappearDuration);
            
            gameBackground.Deactivate();
            matrixMap.Deactivate();
            isTransitioning = false;
        }
        void DeselectCameraMatrixLayersInTransitionedScene()
        {
            Camera[] cameras = FindObjectsOfType<Camera>();

            foreach (var camera in cameras)
            {
                if (camera == transitionCamera || camera == transitionCameraBG || camera == transitionCameraFG)
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