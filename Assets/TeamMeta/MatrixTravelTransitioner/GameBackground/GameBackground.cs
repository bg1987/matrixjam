using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TeamMeta.MatrixTravelTransitioner
{
    public class GameBackground : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [SerializeField] RenderTexture gameRenderTexture;
        [SerializeField] LayerMask transitionLayer;
        [SerializeField] Camera transitionCamera;
        [SerializeField] Material material;
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
            container.SetActive(true);

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
                    canvas.planeDistance = cameraInGameScene.nearClipPlane + 0.1f;
                }
                var canvasScaler = canvas.GetComponent<CanvasScaler>();
                canvasScaler.enabled = false;
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
                count += Time.deltaTime;
                yield return null;

            }
            material.SetFloat("_Grayness", 1);
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
