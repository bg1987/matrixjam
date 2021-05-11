using MatrixJam.TeamMeta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TeamMeta.MatrixTravelTransition
{
    public class GameBackground : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [SerializeField] RenderTexture gameRenderTexture;
        [SerializeField] Texture2D gameRenderTextureStatic;
        [SerializeField] LayerMask transitionLayer;
        [SerializeField] Camera transitionCamera;
        [SerializeField] Material material;
        [SerializeField] RawImage gameBgRawImage;
        // Start is called before the first frame update
        void Awake()
        {
            container.SetActive(false);
        }
        private void Start()
        {
        }
        public void RenderGameAsBackground()
        {
            Camera[] allCameras = Camera.allCameras;
            Camera cameraInGameScene = Camera.main;
            UpdateCamerasRenderTexture(allCameras);

            UpdateGameCanvasesToCameraThatUsesRenderTexture(cameraInGameScene);

            container.SetActive(true);
            gameBgRawImage.texture = gameRenderTexture;

            gameBgRawImage.raycastTarget = true;
        }
        void UpdateCamerasRenderTexture(Camera[] allCameras)
        {
            foreach (var camera in allCameras)
            {
                if (transitionLayer == (transitionLayer | 1 << camera.gameObject.layer))
                {
                    continue;
                }
                camera.cullingMask = camera.cullingMask & ~transitionLayer;
                camera.targetTexture = gameRenderTexture;
            }
        }
        void UpdateGameCanvasesToCameraThatUsesRenderTexture(Camera cameraInGameScene)
        {
            //Unity's CanvasScaler uses screen size to determine its scaling.
            //With a renderTexture the desired size for scaling is the texture's size
            //For that there's a need to destroy current CanvasScaler and replace it with the custom ResolutionCanvasScaler

            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach (var canvas in canvases)
            {
                if (transitionLayer == (transitionLayer | 1 << canvas.gameObject.layer))
                {
                    continue;
                }
                var canvasScaler = canvas.GetComponent<CanvasScaler>();

                //Destroy CanvasScaler and replace with ResolutionCanvasScaler
                if (canvasScaler!=null && canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    var uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    var matchWidthOrHeight = canvasScaler.matchWidthOrHeight;
                    var referenceResolution = canvasScaler.referenceResolution;
                    var screenMatchMode = canvasScaler.screenMatchMode;

                    DestroyImmediate(canvasScaler, false);

                    var resolutionCanvasScaler = canvas.gameObject.AddComponent<ResolutionCanvasScaler>();
                    resolutionCanvasScaler.uiScaleMode = uiScaleMode;
                    resolutionCanvasScaler.matchWidthOrHeight = matchWidthOrHeight;
                    resolutionCanvasScaler.referenceResolution = referenceResolution;
                    resolutionCanvasScaler.screenMatchMode = screenMatchMode;
                    resolutionCanvasScaler.SetScreenResolution(new Vector2(gameRenderTexture.width, gameRenderTexture.height));

                    resolutionCanvasScaler.UpdateThisFrame();

                }
                //Change canvas to camera space
                if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
                {
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = cameraInGameScene;
                    canvas.planeDistance = cameraInGameScene.nearClipPlane + 0.1f;
                    canvas.sortingOrder = 100;
                }
            }
        }
        public void Grayout(float duration)
        {
            StartCoroutine(GrayoutRoutine(duration));
        }
        IEnumerator GrayoutRoutine(float duration)
        {
            float count = 0;
            while (count < duration)
            {
                float t = count / duration;
                material.SetFloat("_Grayness", t);
                count += Time.unscaledDeltaTime;
                yield return null;

            }
            material.SetFloat("_Grayness", 1);
        }
        public void SetToStatic()
        {
            gameRenderTextureStatic = new Texture2D(gameRenderTexture.width, gameRenderTexture.height, textureFormat: TextureFormat.ARGB32, mipChain:false);
            gameRenderTextureStatic.filterMode = FilterMode.Point;

            Graphics.CopyTexture(gameRenderTexture, gameRenderTextureStatic);

            gameRenderTexture.Release();
            gameBgRawImage.texture = gameRenderTextureStatic;
        }
        public void Deactivate()
        {
            container.SetActive(false);
        }
        public void StopBlocking()
        {
            gameBgRawImage.raycastTarget = false;
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
