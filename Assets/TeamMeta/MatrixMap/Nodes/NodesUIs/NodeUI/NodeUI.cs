using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    [ExecuteInEditMode]
    public class NodeUI : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;
        [SerializeField] TextMeshSizer textSizer;
        [SerializeField] TextMeshContainer textContainer;
        [SerializeField] GameObject container;
        [SerializeField] private float distanceFromNode;
        [SerializeField] float diagonalDistanceFactorInfluence = 1;
        [SerializeField] Transform line;

        [Header("Appearance")]
        [SerializeField] TmpFader tmpFader;
        [SerializeField] float linesFadeInDuration = 0.34f;
        [SerializeField] float characterFadeInDuration = 0;
        [SerializeField] float overallAlphaFadeInDuration = 0.4f;

        [Header("Disappearance")]
        [SerializeField] float linesFadeOutDuration = 0.23f;
        [SerializeField] float characterFadeOutDuration = 0.2f;
        [SerializeField] float overallAlphaFadeOutDuration = 0.3f;

        [Header("For Debug")]
        [SerializeField] Node node;
        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return null;
            //yield return new WaitForSeconds(1);
            //Disappear(true);
            //yield return new WaitForSeconds(1);
            //Appear(true);
        }
        private void Update()
        {
            //For Debugging
            if (node)
                PositionAroundNode(Vector3.zero, node);
        }
        public void Activate()
        {
            container.SetActive(true);
        }
        public void Appear(bool shouldFadeOverallAlpha)
        {
            tmpFader.FadeInLines(linesFadeInDuration, characterFadeInDuration);
            if (shouldFadeOverallAlpha)
                tmpFader.FadeInOverallAlpha(overallAlphaFadeInDuration);
        }
        public void AppearInstantly()
        {
            tmpFader.FadeInLines(0f, 0f);
            tmpFader.FadeInOverallAlpha(0.0f);
        }
        public void Disappear(bool shouldFadeOverallAlpha)
        {
            tmpFader.FadeOutLines(linesFadeOutDuration, characterFadeOutDuration);
            if (shouldFadeOverallAlpha)
                tmpFader.FadeOutOverallAlpha(overallAlphaFadeOutDuration);
        }
        public void DisappearInstantly()
        {
            tmpFader.FadeOutLines(0f, 0f);
            tmpFader.FadeOutOverallAlpha(0.0f);
        }
        public void deactivate()
        {
            DisappearInstantly();
            container.SetActive(false);
        }
        public void SetNodeData(string name, int visitsCount, int DiscoveredEdgesCount, int totalEdgesCount)
        {
            string textString = "";
            
            textString += name;
            textString += "\n";

            textString += "Visits: "+ visitsCount;
            textString += "\n";

            textString += "Paths: "+DiscoveredEdgesCount+"/"+totalEdgesCount;
            
            text.text = textString;

            textSizer.UpdateTextSize();
            textContainer.UpdateSize();
        }
        public void PositionAroundNode(Vector3 mapCenter,Node node)
        {
            Vector3 position = node.transform.position;

            Vector3 directionFromMap = node.transform.position - mapCenter;
            directionFromMap.Normalize();

            Vector3 targetDirection = Vector3.zero;
            if (directionFromMap.x <= 0 && directionFromMap.y<0)
            {
                var startDirection = (Vector3.left+Vector3.up* diagonalDistanceFactorInfluence).normalized;
                var endDirection = Vector3.left;
              
                targetDirection = Vector3.Lerp(startDirection, endDirection, 1 + directionFromMap.y).normalized;
            }
            else if (directionFromMap.x < 0 && directionFromMap.y >= 0)
            {
                var startDirection = Vector3.left;
                var endDirection = (Vector3.left + Vector3.down* diagonalDistanceFactorInfluence).normalized;

                targetDirection = Vector3.Lerp(startDirection, endDirection, directionFromMap.y).normalized;
            }
            else if (directionFromMap.x >= 0 && directionFromMap.y >= 0)
            {
                var startDirection = Vector3.right;
                var endDirection = (Vector3.right + Vector3.down* diagonalDistanceFactorInfluence).normalized;

                targetDirection = Vector3.Lerp(startDirection, endDirection, directionFromMap.y).normalized;
            }
            else if (directionFromMap.x > 0 && directionFromMap.y <= 0)
            {
                var startDirection = (Vector3.right + Vector3.up* diagonalDistanceFactorInfluence).normalized;
                var endDirection = Vector3.right;

                targetDirection = Vector3.Lerp(startDirection, endDirection, 1 + directionFromMap.y).normalized;
            }
            position += targetDirection * distanceFromNode;
            position.z = transform.position.z;

            float scaleX = textContainer.transform.localScale.x;
            float scaleOffsetX = scaleX/2f;
            if(targetDirection.x < 0){
                scaleOffsetX = -scaleOffsetX;
            }
            position.x += scaleOffsetX;
            transform.position = position;

            Vector3 linePosition = position;
            linePosition.x -= scaleOffsetX;
            RotateAndPositionLine(linePosition, -targetDirection, distanceFromNode);
        }
        void RotateAndPositionLine(Vector3 startPosition, Vector3 targetDirection, float targetDistance)
        {
            line.rotation = Quaternion.identity;
            Vector3 linePosition = startPosition;
            line.position = linePosition;
            Vector3 lineScale = line.localScale;
            lineScale.y = targetDistance;

            Vector3 lineLookDirection = line.transform.up;
            Vector3 lineTargetDirection = targetDirection;
            var degrees = Mathf.Atan2(lineTargetDirection.y, lineTargetDirection.x) - Mathf.Atan2(lineLookDirection.y, lineLookDirection.x);
            degrees *= Mathf.Rad2Deg;
            line.Rotate(0, 0, degrees);
        }
    }
}
