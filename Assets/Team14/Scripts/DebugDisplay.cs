using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
    public class DebugDisplay : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Camera cam;

        [Header("Track starts")]
        [SerializeField] private Color trackStartColor = Color.white;
        [SerializeField] private float trackStartSize = 10f;
        
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

            foreach (var trackStartPosition in gameManager.TrackStartPositions)
                DrawTrackStartLine(trackStartPosition);
        }

        private void DrawTrackStartLine(Vector3 center)
        {
            DrawLine(center, trackStartSize, trackStartColor);
        }
        
        private void DrawBeatLine(Vector3 center, int beatIdx)
        {
            var i = beatIdx % beatsPerBar; // The  beats index in the bar
            
            Gizmos.color = beatColors[i];
            DrawLine(center, beatSizes[i], beatColors[i]);
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
    }
}