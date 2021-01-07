using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphSaver
    {
        MatrixGraphView graphView;
        string path;
        List<MatrixNode> nodes;
        List<Edge> edges;
        public MatrixGraphSaver(MatrixGraphView graphView, string path)
        {
            this.graphView = graphView;
            this.path = path;

            nodes = new List<MatrixNode>();
            edges = new List<Edge>();
        }
        public void Save()
        {
            Debug.Log("Save graph at " + path);
        }
    }
}
