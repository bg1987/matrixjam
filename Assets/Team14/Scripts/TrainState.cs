using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace MatrixJam.Team14
{
    [Serializable]
    public abstract class TrainState
    {
        // For debug
        public abstract string Name { get; }
        public abstract string AnimTrigger { get; }

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

        protected void TransitionWithMove(TrainMove move, TrainState state)
        {
            var obstacles = Obstacle.CurrObstacles[move];
            Assert.IsTrue(obstacles.Count <= 1, "More than one obstacle should not overlap!");
            
            var obs = obstacles.FirstOrDefault();
            TrainController.TransitionState(state, obs ? obs.MoveCue : null);
        }
    }

    public class TrainDriveState : TrainState
    {
        public override string Name => "Drive";
        public override string AnimTrigger => "Idle";

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetKeyDown(TrainMoves.GetKey(TrainMove.Jump)))
            {
                TransitionWithMove(TrainMove.Jump, TrainController.JumpState);
            }
        }
    }

    public class TrainJumpState : TrainState
    {
        // private float _jumpDist;
        private float _jumpTime;
        private float currTime;

        public override string AnimTrigger => "Jump";

        public TrainJumpState(float jumpTime)
        {
            _jumpTime = jumpTime;
            // _jumpDist = jumpDist;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            currTime = 0f;
        }

        public override void OnUpdate()
        {
            currTime += Time.deltaTime;
            // y in anim
            if (currTime >= _jumpTime)
                TrainController.TransitionState(TrainController.DriveState, null);
        }

        public override string Name => "Jump";

    }

    public class TrainDuckState : TrainState
    {
        public override string Name => "Duck";
        public override string AnimTrigger => "Duck";

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetKeyDown(TrainMoves.GetKey(TrainMove.Jump)))
            {
                TransitionWithMove(TrainMove.Jump, TrainController.JumpState);
            }
        }
    }

    public class TrainNullState : TrainState
    {
        public override string Name => "NONE";
        public override string AnimTrigger => null;
    }
}
