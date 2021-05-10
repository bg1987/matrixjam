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

            RemoveUnusedExits(matrixGraphSO);

            return matrixGraphSO;
        }
        void RemoveUnusedExits(MatrixGraphSO matrixGraphSO)
        {
            var nodes = matrixGraphSO.nodes;
            var edges = matrixGraphSO.edges;

            var outputPortsToRemoveIndexes = new List<int>();

            for (int i = 0; i < nodes.Count; i++)
            {
                outputPortsToRemoveIndexes.Clear();

                var outputPorts = nodes[i].outputPorts;

                for (int j = 0; j < outputPorts.Count; j++)
                {
                    int foundIndex = edges.FindIndex(edgeData => edgeData.startPort == outputPorts[j]);
                    if (foundIndex < 0)
                        outputPortsToRemoveIndexes.Add(j);  
                }
                for (int j = outputPortsToRemoveIndexes.Count-1; j >= 0; j--)
                {
                    int index = outputPortsToRemoveIndexes[j];
                    outputPorts.RemoveAt(index);
                }
            } 
        }
    }
}
