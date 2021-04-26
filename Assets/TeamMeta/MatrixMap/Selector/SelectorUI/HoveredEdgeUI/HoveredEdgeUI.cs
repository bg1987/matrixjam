using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class HoveredEdgeUI : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;
        [SerializeField] TextMeshSizer textSizer;
        [SerializeField] TextMeshContainer textContainer;
        [SerializeField] GameObject container;
        [SerializeField] Transform line;

        [SerializeField] private float distanceFromEdge = 1;
        [SerializeField] private bool reverseDirection;
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
        public void deactivate()
        {
            container.SetActive(false);
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
            RotateAndPositionLine(centerPointWorldSpace, lineDirection, distanceFromEdge);

        }
    }
}
