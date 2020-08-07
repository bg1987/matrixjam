using UnityEngine;

namespace TheFlyingDragons
{
    public class Explosion : MonoBehaviour
    {
        public ExplosionConfig config;
    
        private void Awake()
        {
            Destroy(gameObject, config.lifetime);
        }
    }
}
