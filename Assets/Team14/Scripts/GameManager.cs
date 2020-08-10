using System;
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
        public static event Action ResetEvent;
        
        [SerializeField] private int startLives;
        [SerializeField] private AudioManager audioManager;

        [SerializeField] private Transform startAndDirection;

        [SerializeField] private Transform character;
        [SerializeField] private ThomasMoon thomasMoon;

        [Header("Infra")]
        [SerializeField] private Exit winExit;
        [SerializeField] private Exit loseExit;

        private bool reachedEnd;
        
        public static GameManager Instance { get; private set; }

        public Vector3[] BeatPositions { get; private set; }
        public Vector3[] TrackStartPositions { get; set; }
        public Vector3[] TrackEndPositions { get; private set; }


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There shouldnt be 2 trains!");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            TrainController.Instance.Lives = startLives;
            audioManager.OnFinishTrack += OnTrackFinished;
            audioManager.OnFinishTracklist += OnTrackListFinished;
            
            // Can maybe get rid of this if it causes problems
            UpdateBeatPositions();    
        }

        private void Start()
        {
            audioManager.Restart();     
        }

        public void OnValidate()
        {
            UpdateBeatPositions();
        }

        private void Update()
        {
            if (reachedEnd) return;
            
            var pos = audioManager.GetCurrPosition(startAndDirection);
            character.position = pos;
        }

        private void OnDestroy()
        {
            audioManager.OnFinishTrack -= OnTrackFinished;
            audioManager.OnFinishTracklist -= OnTrackListFinished;
            
            if (Instance != this) return;
            Instance = null;
        }

        public static float GetTimeinTracklist() => Instance.audioManager.GetCurrGlobalSecs();

        private void OnTrackFinished(int i)
        {
            Debug.Log($"Track {i} finished!");
        }

        private void OnTrackListFinished()
        {
            reachedEnd = true;
            Debug.Log("Success! Last Track Finished!");
        }

        private void UpdateBeatPositions()
        {
            BeatPositions = audioManager.GetAllBeatPositions(startAndDirection);
            TrackEndPositions = audioManager.GetTrackEndPositions(startAndDirection);
            TrackStartPositions = audioManager.GetTrackStartPositions(startAndDirection);
        }

        public void OnDeath()
        {
            GameOverExplosive.Explode();
            Invoke(nameof(DoDeath), 5f);
        }

        private void DoDeath()
        {
            GameOverExplosive.StopExplosion();
            var livesRemaining = --TrainController.Instance.Lives;
            if (livesRemaining == 0) OnGameOver();
            else Restart();
        }

        private void OnGameOver()
        {
            Debug.Log("GAME OVERRR");
        }

        private void Restart()
        {
            Debug.Log("RESTART!");
            audioManager.Restart();

            ResetEvent?.Invoke();
        }
    }
}
