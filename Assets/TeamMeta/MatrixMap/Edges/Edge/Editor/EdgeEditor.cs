using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MatrixJam.TeamMeta.MatrixMap
{
    [CustomEditor(typeof(Edge))]

    public class EdgeEditor : Editor
    {
        private Edge edge;
        List<Vector3> anchorPoints;
        IReadOnlyList<Vector3> anchorPointsReadOnly;
        List<Vector3> bezierCurvePoints;

        static float handlesSize = 0.2f;
        static bool showPointsAlongCurve = false;
        static float pointsAlongCurveSize = 0.05f;

        private void OnEnable()
        {
            edge = target as Edge;
            anchorPoints = edge.GetAnchorPoints();

            bezierCurvePoints = edge.GetCurvePoints();

            edge.UpdateBezierCurve(anchorPoints[0], anchorPoints[1], anchorPoints[2]);
            bezierCurvePoints = edge.GetCurvePoints();

        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //EditorGUILayout.Separator();
            DrawHeaderInspector("Editor");

            handlesSize = EditorGUILayout.FloatField("Handles Size", handlesSize);

            showPointsAlongCurve = EditorGUILayout.Toggle("Show Points Along Curve", showPointsAlongCurve);
            pointsAlongCurveSize = EditorGUILayout.FloatField("points Along Curve Size", pointsAlongCurveSize);

        }

        private static void DrawHeaderInspector(string label)
        {
            var headerSkin = GUI.skin.label;
            headerSkin.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField(label, headerSkin);
        }

        protected virtual void OnSceneGUI()
        {
            Handles.color = Color.blue;

            for (int i = 0; i < anchorPoints.Count; i++)
            {
                Vector3 point = Handles.FreeMoveHandle(anchorPoints[i] + edge.transform.position, Quaternion.identity, handlesSize, Vector3.zero, Handles.ConeHandleCap);
                if (point != anchorPoints[i])
                {
                    Undo.RecordObject(edge, "Delete segment");
                    anchorPoints[i] = point - edge.transform.position;
                    edge.UpdateBezierCurve(anchorPoints[0], anchorPoints[1], anchorPoints[2]);

                    bezierCurvePoints = edge.GetCurvePoints();
                }
            }
            Handles.color = Color.gray;
            Handles.DrawLine(anchorPoints[0] + edge.transform.position, anchorPoints[1] + edge.transform.position);
            Handles.DrawLine(anchorPoints[2] + edge.transform.position, anchorPoints[1] + edge.transform.position);

            DrawBezierCurve();

            if (showPointsAlongCurve)
                DrawPointsAlongCurve();
        }
        void DrawBezierCurve()
        {
            Handles.color = Color.white;
            for (int i = 1; i < bezierCurvePoints.Count; i++)
            {
                Vector3 p1 = bezierCurvePoints[i - 1] + edge.transform.position;
                Vector3 p2 = bezierCurvePoints[i] + edge.transform.position;
                Handles.DrawLine(p1, p2);
            }

        }
        void DrawPointsAlongCurve()
        {
            Handles.color = Color.gray;
            for (int i = 0; i < bezierCurvePoints.Count; i++)
            {
                Handles.SphereHandleCap(i, bezierCurvePoints[i] + edge.transform.position, Quaternion.identity, pointsAlongCurveSize, EventType.Repaint);
            }
        }
    }
}
