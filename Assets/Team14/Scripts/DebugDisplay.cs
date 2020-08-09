using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MatrixJam.Team14
{
    public class DebugDisplay : MonoBehaviour
    {
        public enum DebugTrackBorder
        {
            None,
            Start,
            End,
        }
        
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Camera cam;

        [Header("Track borders")]
        [SerializeField] private DebugTrackBorder debugTrackBorder;
        [SerializeField] private Color trackBorderColor = Color.white;
        [SerializeField] private Color borderZColor = Color.red;
        [SerializeField] private float trackBorderSize = 10f;

        [Header("ShowTimes")]
        [SerializeField] private bool[] showBeatNums = new bool[4];

        [Header("Colors")]
        [SerializeField] private Color[] beatColors = new Color[4];

        [Header("Sizes")]
        [SerializeField] private float[] beatSizes = new float[4];

        private int beatsPerBar = 4;
        
        private void OnDrawGizmos()
        {
            if (!cam || !gameManager) return;
            
            var beatPositions = gameManager.BeatPositions;
            for (var i = 0; i < beatPositions.Length; i++)
            {
                var beatPos = beatPositions[i];
                DrawBeatLine(beatPos, i);
            }

            DebugTrackBorders(debugTrackBorder);
        }

        private void DebugTrackBorders(DebugTrackBorder trackBorderMode)
        {
            switch (trackBorderMode)
            {
                case DebugTrackBorder.None:
                    return;
                case DebugTrackBorder.Start:
                    foreach (var startPos in gameManager.TrackStartPositions)
                        DrawTrackBorderLine(startPos);
                    break;
                case DebugTrackBorder.End:
                    foreach (var endPos in gameManager.TrackEndPositions)
                        DrawTrackBorderLine(endPos);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(trackBorderMode), trackBorderMode, null);
            }
        }

        private void DrawTrackBorderLine(Vector3 center)
        {
            DrawLine(center, trackBorderSize, trackBorderColor);
            DrawPosLabel(center, trackBorderSize, borderZColor);
        }

        private void DrawBeatLine(Vector3 center, int beatIdx)
        {
            var i = beatIdx % beatsPerBar; // The  beats index in the bar

            Gizmos.color = beatColors[i];
            var size = beatSizes[i];
            var color = beatColors[i];

            DrawLine(center, size, color);

            if (showBeatNums[i])
            {
                DrawBeatNumLabel(beatIdx, center, size, color);
            }
        }

        private void DrawLine(Vector3 center, float size, Color color)
        {
            Gizmos.color = color;
            var halfSize = size * 0.5f;

            Gizmos.DrawLine(
                center + Vector3.up * halfSize,
                center - Vector3.up * halfSize
            );
        }

        private void DrawPosLabel(Vector3 center, float size, Color color)
        {
            var z = center.z;
            var pos = new Vector3(0f, 0.5f * size, z);

            DrawLabel(z.ToString(), pos, color);
        }

        private void DrawBeatNumLabel(int beatNum, Vector3 center, float size, Color color)
        {
            var z = center.z;
            var pos = new Vector3(0f, 0.5f * size, z);

            DrawLabel(beatNum.ToString(), pos, color);
        }

        private void DrawLabel(string text, Vector3 pos, Color color)
        {
#if UNITY_EDITOR
            Handles.color = color;
            // Handles.RectangleHandleCap(-1, pos, Quaternion.identity, 3, EventType.Repaint);
            Handles.Label(pos, text);
#endif
        }
    }
}