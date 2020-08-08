using UnityEngine;

namespace MatrixJam.Team14
{
    public class ThomasMoon : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float localEyeRadius;
        [SerializeField] private float scalePerSec;

        [SerializeField] private SpriteRenderer faceSprite;
        [SerializeField] private SpriteRenderer leftEye;
        [SerializeField] private SpriteRenderer rightEye;
        [SerializeField] private SpriteRenderer leftEyebrow;
        [SerializeField] private SpriteRenderer rightEyebrow;

        public bool lookAtCursor;
        public Transform LookAtTransform;

        public float Z => faceSprite.transform.position.z;

        private Vector3 LookAt
        {
            get
            {
                if (lookAtCursor)
                {
                    var screenPos = Input.mousePosition;
                    screenPos.z = Z;
                    return cam.ScreenToWorldPoint(screenPos);
                }

                return LookAtTransform.position;
            }
        }

        private Vector3 startScale;

        private void Start()
        {
            startScale = transform.localScale;
        }

        private void Update()
        {
            transform.localScale += Vector3.one * Time.deltaTime * scalePerSec;
            var lookAt = LookAt;
            
            EyeLookAt(leftEye, lookAt);
            EyeLookAt(rightEye, lookAt);
        }

        public void RestartSize()
        {
            transform.localScale = startScale;
        }

        private void EyeLookAt(SpriteRenderer eyeSprite, Vector3 target)
        {
            var delta = transform.InverseTransformDirection(target - eyeSprite.transform.position);
            eyeSprite.transform.localPosition = delta.normalized * localEyeRadius;
        }
    }
}
