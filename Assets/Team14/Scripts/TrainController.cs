using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace MitspeTrainRunner
{
    public class TrainController : MonoBehaviour
    {
        public static TrainController Instance { get; private set; }

        [Header("Debug")] [SerializeField] private bool debugStates;
        [SerializeField] private Color debugColor = Color.red;
        [SerializeField] private Vector2 debugSize = new Vector2(2, 2);
        [SerializeField] private float jumpDist;

        private TrainState _currstate;
        private TrainState _prevState;

        public static TrainState DriveState { get; private set; }
        public static TrainState JumpState { get; private set; }
        public static TrainState DuckState { get; private set; }
        public static TrainState DetourState { get; private set; }
        public static TrainState NullState { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There shouldnt be 2 trains!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Init();
            _currstate = NullState;
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            Instance = null;
        }

        private void Update()
        {
            _currstate?.OnUpdate();
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
            newState.OnEnter();
            _prevState = newState;
        }


        private void Init()
        {
            DriveState = new TrainDriveState();
            JumpState = new TrainJumpState(jumpDist);
            DuckState = new TrainDuckState();
            DetourState = new TrainDetourState();
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

        public void Detour()
        {
            TransitionState(DetourState);
        }
    }
}
