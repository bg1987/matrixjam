using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(ReflectiveController))]
    [RequireComponent(typeof(GoThroughController))]
    public class Floopable : MonoBehaviour
    {
        [SerializeField] FloopableMaterialTypes currentMaterial;

        private ReflectiveController reflectiveController;
        private GoThroughController goThroughController;

        void Start()
        {
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
                case FloopableMaterialTypes.Reflective:
                    goThroughController.enabled = false;
                    reflectiveController.enabled = true;
                    // TODO: Update sprite to reflective
                    break;
                case FloopableMaterialTypes.GoThrough:
                    reflectiveController.enabled = false;
                    goThroughController.enabled = true;
                    // TODO: Update sprite to goThrough
                    break;
                case FloopableMaterialTypes.Refractive:
                    reflectiveController.enabled = true;
                    goThroughController.enabled = true;
                    // TODO: Update sprite to refractive
                    break;
            }

            Debug.Log($"Reflective enabled = {reflectiveController.enabled}");
            Debug.Log($"GoThrough enabled = {goThroughController.enabled}");
        }
    }
}
