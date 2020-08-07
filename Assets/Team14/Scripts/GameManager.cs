using BezierSolution;
using UnityEngine;

namespace MitspeTrainRunner
{
    public class GameManager : MonoBehaviour
    {
        // If the audio is a loop - How many times do we expect it to repeat? (For spline pos)
        [SerializeField] private float loopCount;

        [SerializeField] private float splineNormalizedStart;
        // [SerializeField] private float splineNormalizedEnd = 1.0f;

        [SerializeField] private BezierSpline mainSpline;
        [SerializeField] private Transform character;
        [SerializeField] private AudioSource mainAudio;

        private bool reachedEnd;
        private int _audioLoopCounter;
        private float _prevAudioProgress;

        private float ProgressPerLoop => (1.0f - splineNormalizedStart) / loopCount;

        private void Update()
        {
            if (reachedEnd) return;

            var audioProgress = GetAudioProgress(mainAudio);
            if (_prevAudioProgress > audioProgress) _audioLoopCounter++;

            var t = GetPlayerProgress(_audioLoopCounter + audioProgress);
            var pos = mainSpline.GetPoint(t);
            character.position = pos;

            if (t >= 1.0f) OnReachedEnd();
            _prevAudioProgress = audioProgress;
        }

        private void OnReachedEnd()
        {
            reachedEnd = true;
            Debug.Log("End!");
        }

        private float GetPlayerProgress(float loops)
        {
            var progress = loops * ProgressPerLoop;
            // Mathf.Clamp(progress, splineNormalizedStart, splineNormalizedEnd);
            return splineNormalizedStart + progress;
        }

        private static float GetAudioProgress(AudioSource audioSource)
            => audioSource.time / audioSource.clip.length;
    }
}
