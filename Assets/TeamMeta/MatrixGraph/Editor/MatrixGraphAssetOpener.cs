using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphAssetOpener
    {
        [OnOpenAssetAttribute(1)]
        public static bool OpenAsset(int instanceID, int line, int col)
        {
            Object openedObject = EditorUtility.InstanceIDToObject(instanceID);
            string path = AssetDatabase.GetAssetPath(openedObject);
            string extension = Path.GetExtension(path);
            if (extension == ".matrixgraph")
            {
                var matrixGraph = ScriptableObject.CreateInstance(typeof(MatrixGraphWindow)) as MatrixGraphWindow;
                matrixGraph.OpenMatrixGraphWindow();
                return true;
            }
            return false; // we did not handle the open
        }
    }
}
