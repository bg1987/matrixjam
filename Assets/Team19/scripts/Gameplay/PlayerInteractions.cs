using UnityEngine;
using MatrixJam.Team19.Gameplay.Managers;

namespace MatrixJam.Team19
{
    public class PlayerInteractions : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _levelLostLayerMask;

        [SerializeField]
        private LayerMask _levelPassedLayerMask;

        private void OnCollisionEnter(Collision collision)
        {
            bool isCollidedLayerInLevelLostMask = _levelLostLayerMask.value == (_levelLostLayerMask.value | (1 << collision.gameObject.layer));

            if (isCollidedLayerInLevelLostMask)
            {
                LevelManager.Instance.NotifyLevelLost();
            }

            bool isCollidedLayerInLevelPassedMask = _levelPassedLayerMask.value == (_levelPassedLayerMask.value | (1 << collision.gameObject.layer));

            if (isCollidedLayerInLevelPassedMask)
            {
                LevelManager.Instance.NotifyLevelPassed();
            }
        }
    }
}
