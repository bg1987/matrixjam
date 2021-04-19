using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class FillScreen : MonoBehaviour
    {
        public Camera cam;
        void Update()
        {
            float worldScreenHeight = cam.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
            Vector3 scale = transform.localScale;

            scale.x = worldScreenWidth;
            scale.y = worldScreenHeight;
            transform.localScale = scale;

            var targetPosition = cam.transform.position;
            targetPosition.z = transform.position.z;
            transform.position = targetPosition;
        }
    }
}