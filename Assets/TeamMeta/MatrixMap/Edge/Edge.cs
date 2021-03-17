using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class Edge : MonoBehaviour
    {
        [SerializeField] int curveResolution = 9;

        [SerializeField] List<Vector3> anchorPoints = new List<Vector3>() { Vector3.zero, new Vector3(0.5f, 0.5f, 0f), Vector3.right };
        List<Vector3> curvePoints;
        float edgeLength;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        Vector3 QuadraticBezierCurve(Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * p1 +
                2f * oneMinusT * t * p2 +
                t * t * p3;
        }
        public List<Vector3> CalculateBezierDistributedPoints(Vector3 p1, Vector3 p2, Vector3 p3, int resolution)
        {
            curvePoints.Clear();

            curvePoints.Add(p1);

            //Start QuadraticBezierCurveLength -> For optimization - Put here to avoid doing QuadraticBezierCurve twice.
            float length = 0;
            Vector3 previousPoint = p1;
            //QuadraticBezierCurveLength

            for (int i = 1; i < resolution; i++)
            {
                Vector3 point = QuadraticBezierCurve(p1, p2, p3, i / ((float)resolution - 1));
                curvePoints.Add(point);

                //QuadraticBezierCurveLength
                length += Vector3.Distance(previousPoint, point);
                previousPoint = point;
                //QuadraticBezierCurveLength
            }
            //QuadraticBezierCurveLength
            edgeLength = length;
            //End QuadraticBezierCurveLength

            curvePoints[curvePoints.Count - 1] = p3;
            return curvePoints;
        }
        public void Init(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            curvePoints = CalculateBezierDistributedPoints(p1, p2, p3, curveResolution);

            anchorPoints[0] = p1;
            anchorPoints[1] = p2;
            anchorPoints[2] = p3;
        }
        public void UpdateBezierCurve(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            anchorPoints[0] = p1;
            anchorPoints[1] = p2;
            anchorPoints[2] = p3;

            curvePoints = CalculateBezierDistributedPoints(p1, p2, p3, curveResolution);
        }
        public List<Vector3> GetAnchorPoints()
        {
            List<Vector3> anchorPointsCopy = new List<Vector3>(anchorPoints);
            return anchorPointsCopy;
        }
        public List<Vector3> GetCurvePoints()
        {
            List<Vector3> pointsCopy = new List<Vector3>(curvePoints);
            return pointsCopy;
        }
    }
}
