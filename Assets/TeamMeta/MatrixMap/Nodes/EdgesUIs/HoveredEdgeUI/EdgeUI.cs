using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgeUI : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;
        [SerializeField] TextMeshSizer textSizer;
        [SerializeField] TextMeshContainer textContainer;
        [SerializeField] GameObject container;
        [SerializeField] private float distanceFromEdge = 1;
        [SerializeField] Transform line;
        [SerializeField] LineShader lineShader;
        [SerializeField] private float lineDistanceFromEdgeRadius = 1;
        [SerializeField] private bool reverseDirection;
        [Header("Line Appearance")]
        [SerializeField] float lineFadeInDuration = 0.34f;
        [SerializeField] float lineFadeOutDuration = 0.23f;

        [SerializeField] Brackets brackets;
        [Header("brackets Appearance")]
        [SerializeField] float bracketsFadeInDuration = 0.34f;
        [SerializeField] float bracketsFadeOutDuration = 0.23f;

        [Header("Text Appearance")]
        [SerializeField] TmpFader tmpFader;

        

        [SerializeField] float overallAlphaFadeInDuration = 0.4f;

        [Header("Text Disappearance")]
        [SerializeField] float overallAlphaFadeOutDuration = 0.3f;

        [Header("For Debug")]
        [SerializeField] Edge edge;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            //if (edge)
            //    PositionAtEdgeCenter(edge);
                //PositionAroundNode(Vector3.zero, node);
        }
        public void Activate()
        {
            container.SetActive(true);
        }
        public void Deactivate()
        {
            DisappearInstantly();
            container.SetActive(false);
        }
        public void Appear()
        {
            tmpFader.FadeInOverallAlpha(overallAlphaFadeInDuration);
            brackets.Appear(bracketsFadeInDuration);
            lineShader.Appear(lineFadeInDuration);

        }
        public void Disappear()
        {
            tmpFader.FadeOutOverallAlpha(overallAlphaFadeOutDuration);
            brackets.Disappear(bracketsFadeOutDuration);
            lineShader.Disappear(lineFadeOutDuration);

        }
        public void DisappearInstantly()
        {
            tmpFader.FadeOutOverallAlpha(0.0f);
            brackets.Disappear(0);
            lineShader.Disappear(0);

        }
        public void SetEdgeData(int visitsCount)
        {
            string textString = "";

            textString += "Used: " + visitsCount;

            text.text = textString;

            textSizer.UpdateTextSize();
            textContainer.UpdateSize();
        }
        void RotateAndPositionLine(Vector3 startPosition, Vector3 targetDirection, float targetDistance)
        {
            line.rotation = Quaternion.identity;
            Vector3 linePosition = startPosition;
            line.position = linePosition;
            Vector3 lineScale = line.localScale;
            lineScale.y = targetDistance;
            line.localScale = lineScale;
            Vector3 lineLookDirection = line.transform.up;
            Vector3 lineTargetDirection = targetDirection;
            var degrees = Mathf.Atan2(lineTargetDirection.y, lineTargetDirection.x) - Mathf.Atan2(lineLookDirection.y, lineLookDirection.x);
            degrees *= Mathf.Rad2Deg;
            line.Rotate(0, 0, degrees);
        }
        public void PositionAtEdgeCenter(Edge edge)
        {
            List<Vector3> edgePoints = edge.GetCurvePoints();
            var centerPoint =  edgePoints[edgePoints.Count / 2];
            var preCenterPoint =  edgePoints[edgePoints.Count / 2 - 1];

            var centerPointWorldSpace = edge.transform.TransformPoint(centerPoint);
            Vector3 mapCenter = Vector3.zero;

            Vector3 preCenterPointToCenterPointDirection  = centerPoint - preCenterPoint;
            preCenterPointToCenterPointDirection .Normalize();

            Vector3 normal = Vector3.Cross(preCenterPointToCenterPointDirection , Vector3.back);

            bool isNormalTowardsCenter = Vector3.Dot(normal, mapCenter - centerPoint) > -0.01 ? true : false;
            Vector3 targetDirection = normal;
            if(!isNormalTowardsCenter)
            {
                targetDirection *= -1;
            }
            if(reverseDirection)
                targetDirection *= -1;

            targetDirection.Normalize();

            transform.position = centerPointWorldSpace + (targetDirection*distanceFromEdge);
            
            //Handle line

            //Offset text box by its scale
            Vector2 scale = textContainer.transform.localScale;
            scale.x /= 2f;
            if (targetDirection.x < 0)
                scale.x *= -1;
            scale.y /= 2f;
            if (targetDirection.y < 0)
                scale.y *= -1;
            var offsetDistance = scale.magnitude;

            

            var textContainerLocalPosition = textContainer.transform.localPosition;
            textContainerLocalPosition.x = scale.x;
            textContainerLocalPosition.y = scale.y;

            textContainer.transform.localPosition = textContainerLocalPosition;

            var textLocalPosition = text.transform.localPosition;
            textLocalPosition.x = scale.x;
            textLocalPosition.y = scale.y;

            text.transform.localPosition = textLocalPosition;

            var lineDirection = textContainer.transform.position - centerPointWorldSpace;
            lineDirection.Normalize();
            var lineStart = centerPointWorldSpace + (lineDirection * distanceFromEdge);
            RotateAndPositionLine(lineStart, -lineDirection, distanceFromEdge - lineDistanceFromEdgeRadius);

        }

        internal void SetLineStartColor(ColorHdr colorHdr1)
        {
            throw new System.NotImplementedException();
        }
        internal void SetLineStartColor(Color color)
        {
            //Todo implement when adding line appearance
            color = color / Mathf.Pow(2, 0.8f);
            lineShader.SetStartColor(color);
        }
    }
}