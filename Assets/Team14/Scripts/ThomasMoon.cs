using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team14
{
    public class ThomasMoon : MonoBehaviour
    {
        [Header("DEBUG")]
        [SerializeField] private Vector2 targetLeftEyeLocal;
        [SerializeField] private Vector2 targetRightEyeLocal;
        [Space]
            
        [SerializeField] private Camera cam;
        
        [SerializeField] private RectTransform leftEyeParent;
        [SerializeField] private RectTransform rightEyeParent;
        
        [SerializeField] private RectTransform leftEye;
        [SerializeField] private RectTransform rightEye;
        
        [SerializeField] private RectTransform leftEyebrow;
        [SerializeField] private RectTransform rightEyebrow;
        
         

        public Transform LookAt;

        private void Update()
        {
            EyeLookAt(leftEyeParent, leftEye, LookAt.position, out targetLeftEyeLocal);
            EyeLookAt(leftEyeParent, rightEye, LookAt.position, out targetRightEyeLocal);
        }

        private void EyeLookAt(RectTransform parent, RectTransform eye, Vector3 worldPos, out Vector2 debugLocalPos)
        {
            // Get screen pos
            var screenPos = cam.WorldToScreenPoint(worldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(eye, screenPos, cam, out var eyeLocalPos);
            debugLocalPos = eyeLocalPos;

        }
    }
}
