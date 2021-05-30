using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class NodeSelectable : MonoBehaviour,ISelectable
    {
        [SerializeField] Node node;

        [SerializeField] float idleColorAlpha = 0.19f;
        [SerializeField] float idleColorIntensity = 0;

        [SerializeField] float hoverColorAlpha = 0.19f;
        [SerializeField] float hoverColorIntensity = 3;

        [SerializeField] float selectedColorAlpha = 0.63f;
        [SerializeField] float selectedColorIntensity = 4;
        [Header("---")]
        [SerializeField] float maxColorValue = 0.4f;
        bool isSelected = false;
        bool isHovered = false;

        public void Select()
        {
            if (isSelected)
                return;

            SelectEffect();

            //Debug.Log("Select " + node.name);
            isSelected = true;

            NodeEdgesInteractable(node, isInteractable: true);

        }
        public void Unselect()
        {
            if (isSelected == false)
                return;

            //Debug.Log("Unselect " + node.name);
            isSelected = false;

            if (isHovered)
                HoverEffect();
            else
                IdleEffect();
            NodeEdgesInteractable(node, isInteractable: false);

        }
        public void HoverEnter()
        {
            if (isHovered)
                return;
            //Debug.Log("Hover " + node.name);
            if (!isSelected)
                HoverEffect();

            isHovered = true;
        }

        public void HoverExit()
        {
            if (!isHovered)
                return;
            //Debug.Log("Unhover " + node.name);
            if (!isSelected)
                IdleEffect();

            isHovered = false;
        }
        public Node GetNode()
        {
            return node;
        }
        void IdleEffect()
        {
            ChangeAlpha(idleColorAlpha);
            ChangeIntensity(idleColorIntensity);
        }
        void HoverEffect()
        {
            ChangeAlpha(hoverColorAlpha);
            ChangeIntensity(hoverColorIntensity);
        }
        
        void SelectEffect()
        {
            ChangeAlpha(selectedColorAlpha);
            ChangeIntensity(selectedColorIntensity);
        }
        void ChangeAlpha(float targetAlpha)
        {
            Color outlineColor = node.ModelMaterial.GetColor("_OutlineColor");
            Color.RGBToHSV(outlineColor, out float h, out float s, out float v);
            if(v> maxColorValue)
            {
                v = maxColorValue;
            }
            outlineColor = Color.HSVToRGB(h, s, v);
            outlineColor.a = targetAlpha;
            node.ModelMaterial.SetColor("_OutlineColor", outlineColor);
        }
        void ChangeIntensity(float targetIntensity)
        {
            node.ModelMaterial.SetFloat("_OutlineColorIntensity", targetIntensity);
        }
        void NodeEdgesInteractable(Node node, bool isInteractable)
        {
            List<Edge> edges = node.startPortActiveEdges;
            foreach (var edge in edges)
            {
                edge.EdgeSelectable.interactable = isInteractable;
            }
        }
        public void FlashInteractable(float duration, float selectedColorMark, float hoverColorMark, float idleColorMark)
        {
            StartCoroutine(FlashInteractableRoutine(duration, selectedColorMark, hoverColorMark, idleColorMark));
        }
        IEnumerator FlashInteractableRoutine(float duration, float selectedColorMark, float hoverColorMark, float idleColorMark)
        {
            float t = 0;
            float selectedDuration = duration * selectedColorMark;
            float hoverDuration = duration * hoverColorMark;
            float idleDuration = duration * idleColorMark;

            yield return StartCoroutine(ChangeColorRoutine(selectedDuration, idleColorAlpha, selectedColorAlpha, idleColorIntensity, selectedColorIntensity,smoothStep:true));
            yield return StartCoroutine(ChangeColorRoutine(hoverDuration, selectedColorAlpha, hoverColorAlpha, selectedColorIntensity, hoverColorIntensity,false));
            yield return StartCoroutine(ChangeColorRoutine(idleDuration, hoverColorAlpha, idleColorAlpha, hoverColorIntensity, idleColorIntensity,false));
        }
        IEnumerator ChangeColorRoutine(float duration, float startAlpha, float targetAlpha, float startIntensity, float targetIntensity, bool smoothStep, bool ignoreNodeState = false)
        {
            float t = 0;
            while (t < 1)
            {
                if(ignoreNodeState==false)
                    if (isSelected || isHovered)
                        yield break;

                float alpha;
                if (smoothStep)
                    alpha = Mathf.SmoothStep(startAlpha, targetAlpha, t);
                else
                    alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                ChangeAlpha(alpha);

                float intensity;
                if (smoothStep)
                    intensity = Mathf.SmoothStep(startIntensity, targetIntensity, t);
                else
                    intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
                ChangeIntensity(intensity);

                yield return null;
                t += Time.deltaTime / duration;
            }
            ChangeAlpha(targetAlpha);
            ChangeIntensity(targetIntensity);
        }
        public void Flash(float duration)
        {
            StartCoroutine(FlashRoutine(duration));
        }
        IEnumerator FlashRoutine(float duration)
        {
            Color outlineColor = node.ModelMaterial.GetColor("_OutlineColor");
            float startAlpha = outlineColor.a;
            float targetAlpha = selectedColorAlpha;

            float startIntensity = node.ModelMaterial.GetFloat("_OutlineColorIntensity");
            float targetIntensity = selectedColorIntensity;

            yield return StartCoroutine(ChangeColorRoutine(duration/2f, startAlpha, targetAlpha, startIntensity, targetIntensity, true, true));
            yield return StartCoroutine(ChangeColorRoutine(duration/2f, targetAlpha, startAlpha, targetIntensity, startIntensity, true, true));
        }
    }
}
