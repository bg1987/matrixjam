using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class Edge : MonoBehaviour
    {
        public int index;
        [SerializeField] int curveResolution = 9;
        [SerializeField] private float curveWidth = 0.2f;

        [SerializeField] List<Vector3> anchorPoints = new List<Vector3>() { Vector3.zero, new Vector3(0.5f, 0.5f, 0f), Vector3.right };
        List<Vector3> curvePoints = new List<Vector3>();
        float edgeLength;

        [Header("Mesh")]
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] MeshFilter meshFilter;
        [SerializeField] private MeshCollider meshCollider;

        Vector3[] vertices;
        int[] triangles;
        Vector2[] uv;
        // Start is called before the first frame update
        void Start()
        {
            //Init(anchorPoints[0], anchorPoints[1], anchorPoints[2]);
        }

        // Update is called once per frame
        void Update()
        {
            //UpdateMesh();
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

            CreateMesh();
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
        public void UpdateMesh()
        {
            UpdateVertices(vertices);
            UpdateUV();

            meshFilter.mesh.vertices = vertices;
            meshFilter.mesh.uv = uv;
            meshCollider.sharedMesh = meshFilter.mesh;
        }
        void CreateMesh()
        {
            vertices = new Vector3[curvePoints.Count * 2];
            UpdateVertices(vertices);

            triangles = CreateMeshTriangles();

            uv = new Vector2[vertices.Length];
            UpdateUV();

            var mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            mesh.uv = uv;
            meshCollider.sharedMesh = meshFilter.mesh;

            //meshRenderer.material.SetFloat("_EdgeLength", edgeLength); //Property _EdgeLEngth ToDo

        }
        
        private void UpdateUV()
        {
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            for (int i = 1; i < curvePoints.Count; i++)
            {
                int iByTwo = i * 2;
                float distanceToNextCurvePoint = Vector3.Distance(curvePoints[i - 1], curvePoints[i]) / edgeLength;
                uv[iByTwo] = new Vector2(0, uv[iByTwo - 2].y + distanceToNextCurvePoint);
                uv[iByTwo + 1] = new Vector2(1, uv[iByTwo - 1].y + distanceToNextCurvePoint);

            }
        }

        private void UpdateVertices(Vector3[] vertices)
        {
            float halfWidth = curveWidth / 2f;

            for (int i = 0; i < curvePoints.Count; i++)
            {
                Vector3 origin = curvePoints[i];
                Vector3 tangent;
                Vector3 normal;

                if (i != 0 && i != curvePoints.Count - 1)
                {
                    var directionToNextVertice = curvePoints[i + 1] - curvePoints[i];
                    directionToNextVertice.Normalize();

                    var directionFromPrevVertice = curvePoints[i] - curvePoints[i - 1];
                    directionFromPrevVertice.Normalize();
                    tangent = directionToNextVertice + directionFromPrevVertice;
                }
                else
                {
                    if (i == 0)
                    {
                        tangent = curvePoints[i + 1] - curvePoints[i];

                    }
                    else //if (i == curvePoints.Count - 1)
                    {
                        tangent = curvePoints[i] - curvePoints[i - 1];
                    }
                }
                tangent.Normalize();
                normal = new Vector3(tangent.y, -tangent.x, 0);

                Vector3 normalMultipliedByHalfWidth = normal * halfWidth;
                int iByTwo = i * 2;
                vertices[iByTwo] = -normalMultipliedByHalfWidth + origin;
                vertices[iByTwo + 1] = normalMultipliedByHalfWidth + origin;
            }
        }

        int[] CreateMeshTriangles()
        {
            int trianglesAmount = (curvePoints.Count - 1) * 2;
            triangles = new int[trianglesAmount * 3];

            for (int i = 0; i < trianglesAmount; i += 2)
            {
                triangles[i * 3] = i + 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
                
                triangles[i * 3 + 3] = i + 1;
                triangles[i * 3 + 4] = i + 2;
                triangles[i * 3 + 5] = i + 3;
            }
            return triangles;
        }
    }
}
