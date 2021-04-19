using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class Overlay : MonoBehaviour
    {
        [SerializeField] FillScreen fillScreen;
        [SerializeField] GameObject model;
        [SerializeField] float zPositionWhenActivated = -1;
        [SerializeField] Collider _collider;

        Material material;
        Color originColor;
        // Start is called before the first frame update
        void Awake()
        {
            material = model.GetComponent<Renderer>().material;
            originColor = material.GetColor("_Color");
            Deactivate();
        }
        public void Activate()
        {
            _collider.enabled = true;
            fillScreen.enabled = true;
            Appear();
            BringToFront();
        }
        public void Deactivate()
        {
            _collider.enabled = false;
            Disappear();
            fillScreen.enabled = false;
        }
        void Appear()
        {
            material.SetColor("_Color",originColor);
            
        }
        void Disappear()
        {
            Color color = originColor;
            color.a = 0;
            material.SetColor("_Color", color);
        }
        void BringToFront()
        {
            var targetPosition = transform.position;
            targetPosition.z = zPositionWhenActivated;
            transform.position = targetPosition;
        }
    }
}
