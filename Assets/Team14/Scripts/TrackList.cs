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

        private void OnValidate()
        {
            GameManager.Instance.OnValidate();
        }

        public IEnumerable<Vector3> GetAllBeatPositions(Transform startAndDirection)
            => Enumerable
                .Range(0, Mathf.CeilToInt(TotalBeats))
                .Select(beatNum => GetPosition(startAndDirection, beatNum));


        public Vector3 GetLastPosition(Transform startAndDirection) 
            => GetPosition(startAndDirection, TotalBeats - 1);
        
        public Vector3 GetPosition(Transform startAndDirection, float beatNum, Vector3 offset) 
            => offset + GetPosition(startAndDirection, beatNum);

        public Vector3 GetPosition(Transform startAndDirection, float beatNum)
        {
            var z = beatNum * zPerBeat;
            return startAndDirection.position + startAndDirection.forward * z;
        }
        
        public float GetBeatNum(float secs) => secs * BeatPerSec;
    }

    [CreateAssetMenu(menuName = "Team14/TrackList", fileName = nameof(TrackList))]
    public class TrackList : ScriptableObject
    {
        [SerializeField] private MusicTrack[] tracks;

        private void OnValidate()
        {
            if (GameManager.Instance) GameManager.Instance.OnValidate();
        }
        
        public int TrackCount => tracks.Length;

        public IEnumerable<Vector3> GetAllBeatPositions(Transform startAndDirection)
        {
            var offset = Vector3.zero;
            for (var i = 0; i < TrackCount; i++)
            {
                var track = tracks[i];
                for (var beatNum = 0; beatNum < track.TotalBeats; beatNum++)
                    yield return track.GetPosition(startAndDirection, beatNum, offset);

                offset = track.GetLastPosition(startAndDirection);
            }
        }

        public IEnumerable<Vector3> GetTrackEndPositions(Transform startAndDirection) 
            => tracks.Select(track => track.GetLastPosition(startAndDirection));

        public IEnumerable<Vector3> GetTrackStarts(Transform startAndDirection)
        {
            var offset = Vector3.zero;
            foreach (var track in tracks)
            {
                yield return track.GetPosition(startAndDirection, 0f, offset);
                offset = track.GetLastPosition(startAndDirection);
            }
        }

        public AudioClip GetTrack(int trackIdx) => tracks[trackIdx].Clip;

        // public float BeatsInTrack(int trackIdx) => tracks[trackIdx].TotalBeats;

        // public float GetBeatNum(int trackIdx, float secsInTrack) => tracks[trackIdx].GetBeatNum(secsInTrack);


        public Vector3 GetBeatPosition(Transform startAndDirection, int trackIdx, float trackSecs)
        {
            // The position where this track starts
            var offset = tracks.Take(trackIdx).Aggregate(
                Vector3.zero,
                (sum, track) => sum + track.GetLastPosition(startAndDirection)
            );
            
            var currTrack = tracks[trackIdx];
            var beatNum = currTrack.GetBeatNum(trackSecs);
            var currTrackPos = currTrack.GetPosition(startAndDirection, beatNum);

            return offset + currTrackPos;
        }
    }
}