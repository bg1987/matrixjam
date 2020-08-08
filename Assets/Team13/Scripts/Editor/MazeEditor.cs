using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MatrixJam.Team13
{
	[CustomEditor(typeof(Maze))]
    public class MazeEditor : Editor {
		Maze maze;

		private float _distance = 2.5f;
		private float _rectHalfSize = 1.25f;

        void OnSceneGUI(){
			maze = (Maze)target;

			if(maze.horizontalEdges == null){
				maze.horizontalEdges = new MultiGameObjectArray(maze.width, maze.height + 1);
			}
			if(maze.verticalEdges == null){
				maze.verticalEdges = new MultiGameObjectArray(maze.width + 1, maze.height);
			}

			Handles.BeginGUI();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical(GUILayout.Width(100));
			EditorGUILayout.LabelField("Width", GUILayout.Width(100));
			int newWidth = EditorGUILayout.IntField(maze.width, GUILayout.Width(100));
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(GUILayout.Width(100));
			EditorGUILayout.LabelField("Height", GUILayout.Width(100));
			int newHeight = EditorGUILayout.IntField(maze.height, GUILayout.Width(100));
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			
			
			if(newHeight > maze.height){
				MultiGameObjectArray newArr = new MultiGameObjectArray(maze.width + 1, newHeight);
				for(int i = 0; i <= maze.width; i++){
					for(int j = 0; j < maze.height; j++){
						newArr[i, j] = maze.verticalEdges[i, j];
					}
				}
				maze.verticalEdges = newArr;
				maze.height = newHeight;
			}else if(newHeight < maze.height){
				if(EditorUtility.DisplayDialog("Delete extra?", "Changing the dimensions to smaller values will delete some of the design. Continue?", "Yes", "No")){
					MultiGameObjectArray newArr = new MultiGameObjectArray(maze.width + 1, newHeight);
					for(int i = 0; i <= maze.width; i++){
						for(int j = 0; j < maze.height; j++){
							if(j < newHeight){
								newArr[i, j] = maze.verticalEdges[i, j];
							}else{
								DestroyImmediate(maze.verticalEdges[i, j]);
							}
						}
					}
					maze.verticalEdges = newArr;
					maze.height = newHeight;
				}
			}

			if(newWidth > maze.width){
				MultiGameObjectArray newArr = new MultiGameObjectArray(newWidth, maze.height + 1);
				for(int i = 0; i < maze.width; i++){
					for(int j = 0; j <= maze.height; j++){
						newArr[i, j] = maze.horizontalEdges[i, j];
					}
				}
				maze.horizontalEdges = newArr;
				maze.width = newWidth;
			}else if(newWidth < maze.width){
				if(EditorUtility.DisplayDialog("Delete extra?", "Changing the dimensions to smaller values will delete some of the design. Continue?", "Yes", "No")){
					MultiGameObjectArray newArr = new MultiGameObjectArray(newWidth, maze.height + 1);
					for(int i = 0; i < maze.width; i++){
						for(int j = 0; j <= maze.height; j++){
							if(i < newWidth){
								newArr[i, j] = maze.horizontalEdges[i, j];
							}else{
								DestroyImmediate(maze.horizontalEdges[i, j]);
							}
						}
					}
					maze.horizontalEdges = newArr;
					maze.width = newWidth;
				}
			}

			GUILayout.Space(25);
			if(GUILayout.Button("Clear All", GUILayout.Width(100))){
				Debug.Log("Clear");
				Clear();
			}
			Handles.EndGUI();

			DrawHorizontalEdges();
			DrawVerticalEdges();
		}

		private void DrawVerticalEdges(){
			Vector3 currentPos = maze.transform.position;
			currentPos.z += _rectHalfSize;
			for(int i = 0; i <= maze.width; i++){
				for(int j = 0; j < maze.height; j++){
					Handles.color = Color.red;
					if(maze.verticalEdges[i, j] == null){
						Handles.color = Color.green;
					}
					if(Handles.Button(currentPos, Quaternion.Euler(0, 90, 0), _rectHalfSize, _rectHalfSize, Handles.RectangleHandleCap)){
						if(maze.verticalEdges[i, j] == null){
							GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(maze.verticalWall, maze.transform);
							go.transform.position = currentPos;
							maze.verticalEdges[i, j] = go;
						}else{
							DestroyImmediate(maze.verticalEdges[i, j]);
							maze.verticalEdges[i, j] = null;
						}
					}
					currentPos.z += _distance;
				}
				currentPos.z = maze.transform.position.z + _rectHalfSize;
				currentPos.x += _distance;
			}
		}


		private void DrawHorizontalEdges(){
			Vector3 currentPos = maze.transform.position;
			currentPos.x += _rectHalfSize;
			for(int i = 0; i <= maze.height; i++){
				for(int j = 0; j < maze.width; j++){
					Handles.color = Color.red;
					if(maze.horizontalEdges[j, i] == null){
						Handles.color = Color.green;
					}
					if(Handles.Button(currentPos, Quaternion.Euler(0, 0, 0), _rectHalfSize, _rectHalfSize, Handles.RectangleHandleCap)){
						if(maze.horizontalEdges[j, i] == null){
							GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(maze.horizontalWall, maze.transform);
							go.transform.position = currentPos;
							maze.horizontalEdges[j, i] = go;
						}else{
							DestroyImmediate(maze.horizontalEdges[j, i]);
							maze.horizontalEdges[j, i] = null;
						}
					}
					currentPos.x += _distance;
				}
				currentPos.x = maze.transform.position.x + _rectHalfSize;
				currentPos.z += _distance;
			}
		}

		private void Clear(){
			for(int i = 0; i <= maze.height; i++){
				for(int j = 0; j < maze.width; j++){
					if(maze.horizontalEdges[j, i] != null){
						DestroyImmediate(maze.horizontalEdges[j, i]);
						maze.horizontalEdges[j, i] = null;
					}
				}
			}

			for(int i = 0; i <= maze.width; i++){
				for(int j = 0; j < maze.height; j++){
					if(maze.verticalEdges[i, j] != null){
						DestroyImmediate(maze.verticalEdges[i, j]);
						maze.verticalEdges[i, j] = null;
					}
				}
			}
		}
    }
}
