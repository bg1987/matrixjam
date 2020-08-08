using UnityEngine;
using MatrixJam.Team19.Gameplay.Managers;

namespace MatrixJam.Team19
{
    public class PlayerInteractions : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _gameoverLayerMask;

        private void OnCollisionEnter(Collision collision)
        {
            bool isCollidedLayerInGameOverMask = _gameoverLayerMask == (_gameoverLayerMask | (1 << collision.gameObject.layer));

            if (isCollidedLayerInGameOverMask)
            {
                LevelManager.Instance.NotifyLevelLost();
            }
        }
    }
}
