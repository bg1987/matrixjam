using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = target as GameManager;
            
            GUILayout.Space(20);
            if (GUILayout.Button("Update"))
            {
                script.OnValidate();
            }
        }
    }
#endif
    public class GameManager : MonoBehaviour
    {
        // If the audio is a loop - How many times do we expect it to repeat? (For spline pos)
        [SerializeField] private float bpm;
        [SerializeField] private float zPerBeat;

        [SerializeField] private Transform startAndDirection;
        // [SerializeField] private float splineEnd = 1.0f;

        // [SerializeField] private BezierSpline mainSpline;
        [SerializeField] private Transform character;
        [SerializeField] private AudioSource mainAudio;
        [SerializeField] private ThomasMoon thomasMoon;

        [Header("Infra")]
        [SerializeField] private Exit winExit;
        [SerializeField] private Exit loseExit;

        private bool reachedEnd;
        // private int _audioLoopCounter;
        private float _prevAudioProgress;
        private float _prevY;

        public float BeatPerSec => bpm / 60;
        public float XPerSec => zPerBeat * BeatPerSec;

        public Vector3[] BeatPositions { get; private set; }

        private void Awake()
        {
            // Can maybe get rid of this if it causes problems
            UpdateBeatPositions();    
        }

        public void OnValidate()
        {
            UpdateBeatPositions();
        }

        private void Update()
        {
            if (reachedEnd) return;

            var audioProgress = GetAudioProgress(mainAudio);
            // if (_prevAudioProgress > audioProgress) _audioLoopCounter++;

            // var pos = GetPosition(_audioLoopCounter, mainAudio);
            var pos = GetPosition(mainAudio);
            character.position = pos;
            
            // if ((pos - FinalPoint).sqrMagnitude < 0.1f) OnReachedEnd();
            
            
            _prevY = pos.y;
            _prevAudioProgress = audioProgress;
        }

        private void UpdateBeatPositions()
        {
            var totalBeats = mainAudio.clip.length * BeatPerSec;

            BeatPositions = Enumerable
                .Range(0, Mathf.CeilToInt(totalBeats))
                .Select(i => GetPosition(i))
                .ToArray();
        }

        private void OnReachedEnd()
        {
            reachedEnd = true;
            Debug.Log("End!");
        }

        private Vector3 GetPosition(AudioSource source)
        {
            // var loopedSecs = currLoops * source.clip.length;
            // var totalSeces = loopedSecs + source.time;
            var totalSecs = source.time;
            var beat = GetBeatNum(totalSecs);

            return GetPosition(beat);
        }
        
        private Vector3 GetPosition(float beat)
        {
            var z = beat * zPerBeat;
            return startAndDirection.position + startAndDirection.forward * z;
        }

        private float GetBeatNum(float secs) => secs * BeatPerSec;

        private static float GetAudioProgress(AudioSource audioSource)
            => audioSource.time / audioSource.clip.length;
    }
}
