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
        
        private MusicTrack CurrTrack => trackList[_trackIdx];

        private void Awake()
        {
            source.Stop();
        }
        
        private void Update()
        {
            var donePlayingTrack = source.time >= source.clip.length;
            if (donePlayingTrack) OnTrackFinishedInternal();
        }

        public Vector3 GetCurrPosition(Transform startAndDirection)
        {
            var time = Mathf.Clamp(source.time, 0f, CurrTrack.TotalSeconds);
            var pos = trackList.GetBeatPosition(startAndDirection, _trackIdx, time);
            
            return pos;
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
            Debug.Log($"NextTrack ({_trackIdx} -> {_trackIdx+1})");
            _trackIdx++;
            source.Stop();
            source.clip = trackList.GetClip(_trackIdx);
            source.Play();
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