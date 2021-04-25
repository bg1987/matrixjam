using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgeSelectable : MonoBehaviour, ISelectable
    {
        [SerializeField] Edge edge;

        [SerializeField] float nonInteractableColorAlpha = 0f;
        [SerializeField] float nonInteractableColorIntensity = 0f;


        [SerializeField] float interactableColorAlpha = 0.40f;
        [SerializeField] float interactableColorIntensity = 1f;

        [SerializeField] float hoverColorAlpha = 0.40f;
        [SerializeField] float hoverColorIntensity = 1.36f;
        [SerializeField] float hoverColorIntensityRelativeLuminanceWeight = 5;

        [Header("---")]
        [SerializeField] float maxColorValue = 0.4f;
        [SerializeField] float maxColorInHoverValue = 0.25f;

        bool isHovered = false;

        //public bool interactable { set { if (value) selector.Activate(); else selector.Deactivate(); } }
        private bool _interactable = false;
        public bool interactable 
        { 
            set  
            {
                _interactable = value;
                if (value==true)
                    Interactable();
                else
                    NonInteractable();
            }
            get => _interactable;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void HoverEnter()
        {
            if (isHovered)
                return;
            Debug.Log("Hover " + edge.name);
            HoverEffect();
            isHovered = true;
        }

        public void HoverExit()
        {
            if (!isHovered)
                return;
            Debug.Log("Unhover " + edge.name);

            InteractableEffect();

            isHovered = false;
        }

        public void Select()
        {
            return;
        }

        public void Unselect()
        {
            return;
        }
        void Interactable()
        {
            edge.MeshCollider.enabled = true;
            InteractableEffect();
        }
        void NonInteractable()
        {
            edge.MeshCollider.enabled = false;
            NonInteractableEffect();

        }
        void NonInteractableEffect()
        {
            ChangeAlpha(nonInteractableColorAlpha,maxColorValue);
            ChangeIntensity(nonInteractableColorIntensity);
        }
        void InteractableEffect()
        {
            ChangeAlpha(interactableColorAlpha,maxColorValue);
            ChangeIntensity(interactableColorIntensity);
        }
        void HoverEffect()
        {
            ChangeAlpha(hoverColorAlpha, maxColorInHoverValue);

            float lum = CalculateLum(edge.Material.GetColor("_OutlineColor"));
            float colorIntensity = hoverColorIntensity - (lum * hoverColorIntensityRelativeLuminanceWeight* hoverColorIntensity);

            ChangeIntensity(colorIntensity);
        }
        void ChangeAlpha(float targetAlpha, float maxColorValue)
        {
            Color outlineColor = edge.Material.GetColor("_OutlineColor");
            

            Color.RGBToHSV(outlineColor, out float h, out float s, out float v);
            
            if (v > maxColorValue)
            {
                v = maxColorValue;
            }
            outlineColor = Color.HSVToRGB(h, s, v);
            outlineColor.a = targetAlpha;

            edge.Material.SetColor("_OutlineColor", outlineColor);
        }
        void ChangeIntensity(float targetIntensity)
        {
            edge.Material.SetFloat("_OutlineColorIntensity", targetIntensity);
        }
        float CalculateLum(Color color)
        {
            Color linearColor = color;
            linearColor.r = sRGBtoLin(color.r);
            linearColor.g = sRGBtoLin(color.g);
            linearColor.b = sRGBtoLin(color.b);

            float lum = 0.2126f * linearColor.r + 0.7152f * linearColor.g + 0.0722f * linearColor.b;
            return lum;
        }
        float sRGBtoLin(float colorChannel)
        {
            // Send this function a decimal sRGB gamma encoded color value
            // between 0.0 and 1.0, and it returns a linearized value.

            if (colorChannel <= 0.04045f)
            {
                return colorChannel / 12.92f;
            }
            else
            {
                return Mathf.Pow(((colorChannel + 0.055f) / 1.055f), 2.4f);
            }
        }
    }
}
