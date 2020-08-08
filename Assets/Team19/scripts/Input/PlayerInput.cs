using UnityEngine;

namespace MatrixJam.Team19.Input
{

    /// <summary>
    /// PlayerInput
    /// 
    /// Allows consumers to get delayed information.
    /// Handles cases where keys are pressed mid-movement, but still need to register.
    /// </summary>
    public class PlayerInput : Base.IInputHandler
    {
        private Team19.DataStructures.FixedSizedQueue<Vector3> _inputQueue;
        
        public bool IsInputAvailable
        {
            get
            {
                return _inputQueue.Count > 0;
            }
        }

        public PlayerInput()
        {
            _inputQueue = new Team19.DataStructures.FixedSizedQueue<Vector3>(5);
        }

        public Vector3 GetNextDirection()
        {
            return _inputQueue.Dequeue();
        }

        public void UpdateInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.W) || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow)) {
                _inputQueue.Enqueue(Vector3.forward);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.S) || UnityEngine.Input.GetKeyDown(KeyCode.DownArrow)) {
                _inputQueue.Enqueue(Vector3.back);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.A) || UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow)) {
                _inputQueue.Enqueue(Vector3.left);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.D) || UnityEngine.Input.GetKeyDown(KeyCode.RightArrow)) {
                _inputQueue.Enqueue(Vector3.right);
            }
        }

    }
}
