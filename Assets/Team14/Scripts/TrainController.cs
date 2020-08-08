using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace MatrixJam.Team14
{
    public struct FutureAnimation
    {
        public readonly Animator Anim;
        public readonly float Value;
        public readonly string Trigger;

        public FutureAnimation(Animator anim, float value, string trigger)
        {
            Anim = anim;
            Value = value;
            Trigger = trigger;
        }
    }
    
    public class TrainController : MonoBehaviour
    {
        public static TrainController Instance { get; private set; }

        [Header("States config")]
        [SerializeField] private float jumpTime = 0.4f;
        
        [Header("Cars config")]
        [SerializeField] private Animator masterCarAnim;
        [SerializeField] private Animator[] slaveCarAnims;

        [Header("Debug")] 
        [SerializeField] private bool debugStates;
        [SerializeField] private Color debugColor = Color.red;
        [SerializeField] private Vector2 debugSize = new Vector2(2, 2);

        private TrainState _currstate;
        private TrainState _prevState;

        public static TrainState DriveState { get; private set; }
        public static TrainState JumpState { get; private set; }
        public static TrainState DuckState { get; private set; }
        public static TrainState DetourState { get; private set; }
        public static TrainState NullState { get; private set; }
        
        private HashSet<FutureAnimation> _futureAnimations = new HashSet<FutureAnimation>();

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There shouldnt be 2 trains!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Obstacle.OnObstacleEvent += OnObstacleEvent;
            CreateStates();
            _currstate = NullState;
        }

        private void OnValidate()
        {
            CreateStates();
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            Obstacle.OnObstacleEvent -= OnObstacleEvent;
            Instance = null;
        }

        private void Update()
        {
            _currstate?.OnUpdate();
            HandlePendingAnimations();
        }

        private void Start()
        {
            TransitionState(DriveState);
        }

        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(debugSize.x, debugSize.y, 1f));
            GUI.color = debugColor;
            if (debugStates)
            {
                GUILayout.Label($"TrainState: {_currstate?.Name}");
                GUILayout.Label($"PrevState: {_prevState?.Name}");
            }
        }

        public static void TransitionState(TrainState newState) => Instance.TransitionStateInternal(newState);

        private void TransitionStateInternal(TrainState newState)
        {
            Assert.IsNotNull(newState);

            Debug.Log($"State Transition: {_prevState?.Name} -> {newState.Name}");
            _prevState?.OnExit();
            
            _currstate = newState;
            newState.OnEnter();

            if (newState.AnimTrigger != null)
            {
                CueFutureAnimations(newState.AnimTrigger);
                HandlePendingAnimations();
            }
            
            _prevState = newState;
        }


        private void CreateStates()
        {
            DriveState = new TrainDriveState();
            JumpState = new TrainJumpState(jumpTime);
            DuckState = new TrainDuckState();    
            NullState = new TrainNullState();
        }

        public void Jump()
        {
            TransitionState(JumpState);
        }

        public void Duck()
        {
            TransitionState(DuckState);
        }

        private void OnObstacleEvent(ObstaclePayload payload)
        {
            if (!payload.Successful)
            {
                OnObstacleFailed();
                return;
            }
            
        }

        private void OnObstacleFailed()
        {
            throw new System.NotImplementedException();
        }

        private void HandlePendingAnimations()
        {
            foreach (var futureAnim in _futureAnimations.Reverse())
            {
                var currValue = futureAnim.Anim.transform.position.z;
                if (currValue >= futureAnim.Value)
                {
                    _futureAnimations.Remove(futureAnim);
                    futureAnim.Anim.SetTrigger(futureAnim.Trigger);
                }
            }
        }

        
        /// <summary>
        /// Add future animation cues for master + slave chars, using the transform given or master car
        /// </summary>
        /// <param name="trigger">The trigger to cue</param>
        /// <param name="moveCue">The transform to use for position, if null will use masterCar postion</param>
        private void CueFutureAnimations(string trigger, Transform moveCue)
        {
            var value = moveCue
                ? moveCue.position.z
                : masterCarAnim.transform.position.z;
            
            // Master car
            var masterFutureAnim = new FutureAnimation(masterCarAnim, value, trigger);
            _futureAnimations.Add(masterFutureAnim);
            
            foreach (var slaveCarAnim in slaveCarAnims)
            {
                var futureAnim = new FutureAnimation(slaveCarAnim, value, trigger);
                _futureAnimations.Add(futureAnim);
            }
        }
    }
}
