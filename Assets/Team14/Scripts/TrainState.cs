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

        protected bool HandleJump() => HandleMove(TrainMove.Jump, TrainController.JumpState);
        protected bool HandleDuck() => HandleMove(TrainMove.Duck, TrainController.DuckState);
        
        protected bool HandleHonk()
        {
            var keyCode = TrainMoves.GetKey(TrainMove.Honk);
            if (!Input.GetKeyDown(keyCode)) return false;
            
            TrainController.Instance.CueFutureAnimations("Honk", null);

            var obstacle = Obstacle.HandleMovePressed(TrainMove.Honk);
            return obstacle != null;
        }


        private bool HandleMove(TrainMove move, TrainState state)
        {
            var key = TrainMoves.GetKey(move);
            var playerPressed = Input.GetKeyDown(key);
            if (playerPressed)
                TransitionWithMove(move, state);

            return playerPressed;
        }

        private void TransitionWithMove(TrainMove move, TrainState state)
        {
            var obstacle = Obstacle.HandleMovePressed(move);
            TrainController.TransitionState(state, obstacle ? obstacle.MoveCue : null);
        }
    }

    public class TrainDriveState : TrainState
    {
        public override string Name => "Drive";
        public override string AnimTrigger => "Idle";

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            HandleHonk();
            if (HandleDuck()) return;
            if (HandleJump()) return;
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
            TrainController.Instance.PlaySFX(TrainMove.Jump);
            currTime = 0f;
        }

        public override void OnUpdate()
        {
            HandleHonk();
            if (HandleDuck()) return;

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
            HandleHonk();
            if (HandleJump()) return;
        }
    }

    public class TrainNullState : TrainState
    {
        public override string Name => "NONE";
        public override string AnimTrigger => null;
    }
}
