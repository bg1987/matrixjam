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
        [SerializeField] string TransitionSceneName;
        AsyncOperation nextNodeAsyncOperation;
        [Header("Effects Durations")]
        [SerializeField] float gameBackgroundGrayoutDuration=2f;
        [SerializeField] float ForegroundDisappearDuration=2f;

        [Header("Continue Transition Input")]
        [SerializeField] PressContinueKey pressContinueKey;
        [SerializeField, Min(0)] int minimumVisitedGamesToEnablePressContinue = 1;
        // Start is called before the first frame update
        void Awake()
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
            if (!success)
            {
                Debug.Log("Travel history is blank");
                yield break;
            }
            float volumeBeforeMute = AudioListener.volume;

            //Start transition
            yield return StartCoroutine(StartTransitionRoutine());

            MatrixNodeData destinationGame = matrixTraveler.matrixGraphData.nodes[lastTravel.endPort.nodeIndex];

            //Preload next scene
            yield return StartCoroutine(PreloadNextNodeScene(destinationGame.scenePath));

            //Show Matrix Map
            float matrixMapAppearDuration = matrixMap.CalculateTotalAppearanceTime();
            matrixMap.Appear();
            yield return new WaitForSeconds(matrixMapAppearDuration);

            //Wait for key press before continuing the transition
            int visitedGamesCount = matrixTraveler.travelData.GetVisitedGamesCount();
            if (visitedGamesCount > minimumVisitedGamesToEnablePressContinue)
            {
                pressContinueKey.gameObject.SetActive(true);
                pressContinueKey.Activate();
                WaitUntil waitUntilContinueKeyIsHeld = new WaitUntil(() => pressContinueKey.WasContinueKeyPressed() == true);
                yield return waitUntilContinueKeyIsHeld;
                pressContinueKey.Deactivate();
            }

            //End transition
            yield return StartCoroutine(EndTransitionRoutine(volumeBeforeMute, destinationGame));

        }
        IEnumerator StartTransitionRoutine()
        {
            isTransitioning = true;

            gameBackground.RenderGameAsBackground();
            gameBackground.Grayout(gameBackgroundGrayoutDuration);
            foreground.Appear();

            StartCoroutine(MuteAudioRoutine(gameBackgroundGrayoutDuration));
            yield return new WaitForSeconds(gameBackgroundGrayoutDuration);

            gameBackground.SetToStatic();
            yield return null;
            SceneManager.LoadScene(TransitionSceneName);
            yield return null;
        }
        IEnumerator EndTransitionRoutine(float volumeBeforeMute, MatrixNodeData destinationGame)
        {
            yield return StartCoroutine(LoadNextNodeScene());

            yield return null;
            DeselectCameraMatrixLayersInTransitionedScene();
            StartCoroutine(RestoreAudioRoutine(ForegroundDisappearDuration, volumeBeforeMute));

            foreground.Disappear(ForegroundDisappearDuration, destinationGame.colorHdr1, destinationGame.colorHdr2);
            gameBackground.StopBlocking();
            yield return new WaitForSeconds(ForegroundDisappearDuration);

            gameBackground.Deactivate();
            matrixMap.Deactivate();
            pressContinueKey.gameObject.SetActive(false);

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
        IEnumerator PreloadNextNodeScene(string sceneName)
        {
            
            nextNodeAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
            
            nextNodeAsyncOperation.allowSceneActivation = false;
            while (nextNodeAsyncOperation.progress < 0.9f)
            {
                yield return null;
            }
        }
        IEnumerator LoadNextNodeScene()
        {
            nextNodeAsyncOperation.allowSceneActivation = true;
            while (nextNodeAsyncOperation.isDone == false)
            {
                yield return null;
            }
        }
    }
}
