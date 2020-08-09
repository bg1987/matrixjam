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

        [SerializeField]
        private LayerMask _collectibleMask;

        private bool _wasKeyPickedThisLevel = false;

        private void OnCollisionEnter(Collision collision)
        {
            bool isCollidedLayerInLevelLostMask = _levelLostLayerMask.value == (_levelLostLayerMask.value | (1 << collision.gameObject.layer));

            if (isCollidedLayerInLevelLostMask)
            {
                LevelManager.Instance.NotifyLevelLost();
            }

            if (_wasKeyPickedThisLevel)
            {
                bool isCollidedLayerInLevelPassedMask = _levelPassedLayerMask.value == (_levelPassedLayerMask.value | (1 << collision.gameObject.layer));

                if (isCollidedLayerInLevelPassedMask)
                {
                    LevelManager.Instance.NotifyLevelPassed();

                    _wasKeyPickedThisLevel = false;
                }
            }

            bool isCollidedLayerInCollectibleMask = _collectibleMask.value == (_collectibleMask.value | (1 << collision.gameObject.layer));

            if (isCollidedLayerInCollectibleMask)
            {
                Destroy(collision.gameObject);

                _wasKeyPickedThisLevel = true;
            }
        }
    }
}
