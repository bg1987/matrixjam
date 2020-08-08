using System;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private TrackList trackList;
        [SerializeField] private AudioSource source;

        private int _trackIdx;

        public event Action<int> OnFinishTrack;
        public event Action OnFinishTracklist;

        private void Awake()
        {
            source.Stop();
        }
        
        public void Restart()
        {
            _trackIdx = -1;
            NextTrack();
        }

        public Vector3[] GetAllBeatPositions(Transform startAndDirection) 
            => trackList.GetAllBeatPositions(startAndDirection).ToArray();

        public Vector3[] GetTrackEndPositions(Transform startAndDirection)
            => trackList.GetTrackEndPositions(startAndDirection).ToArray();
        public Vector3[] GetTrackStartPositions(Transform startAndDirection)
            => trackList.GetTrackStarts(startAndDirection).ToArray();

        private void NextTrack()
        {
            _trackIdx++;
            source.Stop();
            source.clip = trackList.GetTrack(_trackIdx);
            source.Play();
        }

        private void Update()
        {
            var donePlayingTrack = source.time > source.clip.length;
            if (donePlayingTrack) OnTrackFinishedInternal();
        }

        public Vector3 GetCurrPosition(Transform startAndDirection)
        {
            var currTrackProgress = source.time;
            var pos = trackList.GetBeatPosition(startAndDirection, _trackIdx, currTrackProgress);
            
            return pos;
        }

        // TODO: GameManager: handle Events
        private void OnTrackFinishedInternal()
        {
            OnFinishTrack?.Invoke(_trackIdx);
            if (_trackIdx == trackList.TrackCount - 1) OnLastTrackFinished();
            else NextTrack();
        }

        private void OnLastTrackFinished()
        {
            OnFinishTracklist?.Invoke();
        }
    }
}