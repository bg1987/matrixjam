using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphConverter
    {
        public string ToJson(MatrixGraphSO matrixGraphSO)
        {
            string matrixGraphJson = "";

            matrixGraphJson = JsonUtility.ToJson(matrixGraphSO);

            return matrixGraphJson;
        }
        public MatrixGraphSO ToScriptableObject(string jsonObject)
        {
            var matrixGraphSO = ScriptableObject.CreateInstance<MatrixGraphSO>();

            string[] lines = jsonObject.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length == 1)
            {
                JsonUtility.FromJsonOverwrite(jsonObject, matrixGraphSO);
                return matrixGraphSO;
            }
            string matrixGraphDataJson = lines[0];

            JsonUtility.FromJsonOverwrite(matrixGraphDataJson, matrixGraphSO);

            return matrixGraphSO;
        }
    }
}
