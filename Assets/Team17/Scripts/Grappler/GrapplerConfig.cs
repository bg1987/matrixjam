using UnityEngine;

namespace TheFlyingDragons
{
    [CreateAssetMenu(menuName = "TheFlyingDragons/GrapplingGunConfig")]
    public class GrapplerConfig : ScriptableObject
    {
        [Space]
        
        public Grappler grapplerPrefab;
        public float changeLengthSpeed = 10f;
    }
}
