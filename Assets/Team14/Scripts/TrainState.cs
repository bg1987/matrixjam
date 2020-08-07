using System;
using System.Collections;
using UnityEngine;

namespace MitspeTrainRunner
{
    [Serializable]
    public abstract class TrainState
    {
        // For debug
        public abstract string Name { get; }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public override string ToString() => $"[TrainState] {Name}";
    }

    public class TrainDriveState : TrainState
    {
        public override string Name => "Drive";

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetButtonDown("Jump"))
            {
                TrainController.TransitionState(TrainController.JumpState);
            }
        }
    }

    public class TrainJumpState : TrainState
    {
        private float jumpDist;
        private int i;

        public TrainJumpState(float jumpDist)
        {
            jumpDist = this.jumpDist;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CoroutineRunner.StartCoroutineStatic(JumpRoutine());
        }

        private IEnumerator JumpRoutine()
        {
            while (i < 20)
            {
                Debug.Log(i++);
                yield return null;
            }

            TrainController.TransitionState(TrainController.DriveState);
        }

        public override string Name => "Jump";

    }

    public class TrainDuckState : TrainState
    {
        public override string Name => "Duck";
    }

    public class TrainDetourState : TrainState
    {
        public override string Name => "Detour";
    }

    public class TrainNullState : TrainState
    {
        public override string Name => "NONE";
    }
}
