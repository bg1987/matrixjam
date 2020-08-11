using System.Collections;
using System.Linq;
using MatrixJam.Team14;
using UnityEngine;

namespace MatrixJam.Team14.Team
{
    public class CatAnimation : MonoBehaviour
    {
        [SerializeField] private float animTime;

        [SerializeField] private bool testOnStart;
        
        private float _startTime;
        private GameObject[] _cats;
        private Obstacle _parentObstacle;
        private SFXmanager SFXmanager;
        private int FrameCount => _cats.Length;
        private float FramesPerSec => FrameCount / animTime;

        private void Start()
        {
            SFXmanager = FindObjectOfType<SFXmanager>();
            _cats = transform
                .Cast<Transform>()
                .Select(trans => trans.gameObject)
                .ToArray();

            _parentObstacle = GetComponentInParent<ObstacleHolder>().Obstacle;
            if (_parentObstacle == null) Debug.LogError($"No obstacle found above {name}", this);

            Obstacle.OnObstacleEvent += OnObstacleEvent;
            GameManager.ResetEvent += OnGameReset;
            
            SetFrameActive(0);
            if (testOnStart) Animate();
        }

        private void OnGameReset() => SetFrameActive(0);

        private void OnDestroy()
        {
            Obstacle.OnObstacleEvent -= OnObstacleEvent;
            GameManager.ResetEvent -= OnGameReset;
        }

        private void OnObstacleEvent(ObstaclePayload payload)
        {
            // Debug.Log($"ObsEvent 1");
            if (payload.Obstacle != _parentObstacle) return;
            Debug.Log($"ObsEvent 2");
            if (payload.Successful)
            {
                Animate();
                SFXmanager.CatSqueals.PlayRandom();
            }
        }

        private void Animate()
        {
            Debug.Log("ANIMATE!");
            _startTime = Time.time;
            StopAllCoroutines();
            StartCoroutine(AnimateRoutine());
        }

        private IEnumerator AnimateRoutine()
        {
            var frameIdx = 0;
            while (frameIdx < _cats.Length)
            {
                var secsSinceStart = Time.time - _startTime;
                var frame = secsSinceStart * FramesPerSec;
                frameIdx = Mathf.RoundToInt(frame);
                
                SetFrameActive(frameIdx);
                yield return null;
            }

            foreach (var cat in _cats)
                cat.SetActive(false);
        }

        private void SetFrameActive(int frameIdx)
        {
            Debug.Log($"SetFrameActive({frameIdx})");
            if (frameIdx >= FrameCount) frameIdx = FrameCount - 1;
            for (var i = 0; i < FrameCount; i++)
            {
                var catGO = _cats[i];
                var active = i == frameIdx;
                catGO.SetActive(active);
            }
        }
    }
}
