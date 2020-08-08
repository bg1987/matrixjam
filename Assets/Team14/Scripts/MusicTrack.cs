using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
    [Serializable]
    public class MusicTrack
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private float zPerBeat = 7.5f;
        [SerializeField] private float bpm;

        public AudioClip Clip => clip;
        public float BPM => bpm;
        private float BeatPerSec => bpm / 60;

        public float TotalBeats => BeatPerSec * clip.length;
        public float TotalSeconds => TotalBeats * BeatPerSec;

        private void OnValidate()
        {
            GameManager.Instance.OnValidate();
        }

        public IEnumerable<Vector3> GetAllBeatPositions(Transform startAndDirection)
            => Enumerable
                .Range(0, Mathf.FloorToInt(TotalBeats))
                .Select(beatNum => GetPosition(startAndDirection, beatNum));


        public Vector3 GetLastPosition(Transform startAndDirection) 
            => GetPosition(startAndDirection, TotalBeats);
        
        public Vector3 GetPosition(Transform startAndDirection, float beatNum, Vector3 offset) 
            => offset + GetPosition(startAndDirection, beatNum);

        public Vector3 GetPosition(Transform startAndDirection, float beatNum)
        {
            var z = beatNum * zPerBeat;
            return startAndDirection.position + startAndDirection.forward * z;
        }
        
        public float GetBeatNum(float secs) => secs * BeatPerSec;
    }
}