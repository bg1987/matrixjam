using UnityEngine;

namespace TheFlyingDragons
{
    [CreateAssetMenu(menuName = "TheFlyingDragons/ExplosionConfig")]
    public class ExplosionConfig : ScriptableObject
    {
        public Explosion explosionPrefab;
        public float lifetime = 1f;
        public float radius = 1f;
    }
}
