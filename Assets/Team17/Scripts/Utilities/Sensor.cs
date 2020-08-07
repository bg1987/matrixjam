using UnityEngine;

namespace TheFlyingDragons
{
    [RequireComponent(typeof(Collider))]
    public class Sensor : MonoBehaviour
    {
        //[ReadOnly]
        [SerializeField]
        private bool isDetected;
        public bool IsDetected => isDetected;

        new Collider collider;
        public Collider Collider => collider;

        public bool IsTouchingLayers(LayerMask touchLayers)
        {
            return false;//collider.IsTouchingLayers(touchLayers);
        }

        void Awake()
        {
            collider = GetComponent<Collider>();
        }

        void OnEnable()
        {
            isDetected = false;
        }

        void FixedUpdate()
        {
            isDetected = false;//collider.IsTouchingLayers(layers);
        }
    }
}
