using UnityEngine;

namespace MatrixJam.Team19.Input
{
    public enum EInputDirection
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT,
    }

    /// <summary>
    /// PlayerInput
    /// 
    /// Allows consumers to get delayed information.
    /// Handles cases where keys were pressed mid-movement, but still need to register.
    /// </summary>
    public class PlayerInput
    {
        private Team19.DataStructures.FixedSizedQueue<EInputDirection> _inputQueue;
        
        public bool IsInputAvailable
        {
            get
            {
                return _inputQueue.Count > 0;
            }
        }

        public PlayerInput()
        {
            _inputQueue = new Team19.DataStructures.FixedSizedQueue<EInputDirection>(5);
        }

        public EInputDirection GetNextInput()
        {
            return _inputQueue.Dequeue();
        }

        public void UpdateInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.W) || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow)) {
                _inputQueue.Enqueue(EInputDirection.FORWARD);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.S) || UnityEngine.Input.GetKeyDown(KeyCode.DownArrow)) {
                _inputQueue.Enqueue(EInputDirection.BACKWARD);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.A) || UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow)) {
                _inputQueue.Enqueue(EInputDirection.LEFT);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.D) || UnityEngine.Input.GetKeyDown(KeyCode.RightArrow)) {
                _inputQueue.Enqueue(EInputDirection.RIGHT);
            }
        }

    }
}
