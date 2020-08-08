using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(ReflectiveController))]
    [RequireComponent(typeof(GoThroughController))]
    public class Floopable : MonoBehaviour
    {
        [SerializeField] FloopableMaterialTypes currentMaterial;

        private BoxCollider2D boxCollider;
        private ReflectiveController reflectiveController;
        private GoThroughController goThroughController;

        void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            reflectiveController = GetComponent<ReflectiveController>();
            goThroughController = GetComponent<GoThroughController>();
            UpdateMaterial();
        }

        public void ChangeMaterial(FloopableMaterialTypes newMaterial)
        {
            currentMaterial = newMaterial;
            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            switch (currentMaterial)
            {
                // Using boxCollider.isTrigger so we can still detect it via raycast
                case FloopableMaterialTypes.Reflective:
                    boxCollider.isTrigger = false;
                    reflectiveController.enabled = true;
                    goThroughController.enabled = false;
                    // TODO: Update sprite to reflective
                    break;
                case FloopableMaterialTypes.Opaque:
                    boxCollider.isTrigger = false;
                    reflectiveController.enabled = false;
                    goThroughController.enabled = false;
                    // TODO: Update sprite to opaque
                    break;
                case FloopableMaterialTypes.GoThrough:
                    boxCollider.isTrigger = true;
                    reflectiveController.enabled = false;
                    goThroughController.enabled = true;
                    // TODO: Update sprite to goThrough
                    break;
            }
        }
    }
}
