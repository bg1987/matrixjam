using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [CustomEditor(typeof(MatrixOver))]

    public class MatrixOverEditor : Editor
    {
        MatrixOver matrixOver;
        private void OnEnable()
        {
            matrixOver = target as MatrixOver;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //EditorGUILayout.Separator();
            DrawHeaderInspector("Editor");

            if(GUILayout.Button("Execute Matrix Over (Playmode)"))
            {
                if (Application.isPlaying)
                    matrixOver.Execute();
            }
        }

        private static void DrawHeaderInspector(string label)
        {
            var headerSkin = GUI.skin.label;
            headerSkin.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField(label, headerSkin);
        }
    }
}
