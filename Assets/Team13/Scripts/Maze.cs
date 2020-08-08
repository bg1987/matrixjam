using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MatrixJam.Team13
{
	[ExecuteInEditMode]
    public class Maze : MonoBehaviour {
    	[HideInInspector] public int width;
		[HideInInspector] public int height;

		public GameObject verticalWall;
		public GameObject horizontalWall;

		public MultiGameObjectArray horizontalEdges;
		public MultiGameObjectArray verticalEdges;

		/*private GameObject[,] _horizontalEdges;
		private GameObject[,] _verticalEdges;
		public GameObject[,] horizontalEdges{
			get{
				return _horizontalEdges;
			}
			set{
				_horizontalEdges = value;
			}
		}

		public GameObject[,] verticalEdges{
			get{
				return _verticalEdges;
			}
			set{
				_verticalEdges = value;
			}
		}*/
		[HideInInspector] public GameObject[] edgeIntersections;

		/*public void AddHorizontalEdge(int x, int y, GameObject go){
			horizontalEdges[x, y] = go;
		}*/
    }
}
