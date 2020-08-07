using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MatrixJam.Team19.Gameplay.Controls
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _stepDistance = 1f;

        [SerializeField]
        private float _stepTimeSeconds = 1f;

        [SerializeField]
        private LayerMask _stepDenyingLayerMask;

        private bool _isReadyToStep = true;

        private Team19.Input.PlayerInput _playerInput;

        private Vector3 _activeStepDirectionVector = Vector3.zero;
        private Vector3 _activeStepOrigin = Vector3.zero;
        private Vector3 _activeStepDestination = Vector3.zero;

        private float _activeStepProgress = 0;

        private Dictionary<Team19.Input.EInputDirection, Vector3> _inputDirectionToVectorMap;

        private Collider _collider;

        private void Awake()
        {
            _playerInput = new Team19.Input.PlayerInput();

            _inputDirectionToVectorMap = new Dictionary<Team19.Input.EInputDirection, Vector3>
            {
                { Team19.Input.EInputDirection.FORWARD, Vector3.forward },
                { Team19.Input.EInputDirection.BACKWARD, Vector3.back },
                { Team19.Input.EInputDirection.LEFT, Vector3.left },
                { Team19.Input.EInputDirection.RIGHT, Vector3.right }
            };

            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
            _playerInput.UpdateInput();
        }

        private void FixedUpdate()
        {
            UpdateMovement(Time.deltaTime);
        }

        private void UpdateMovement(float deltaTime)
        {
            if (_playerInput.IsInputAvailable && _isReadyToStep)
            {
                Vector3 nextLocalDirectionVector = _inputDirectionToVectorMap[_playerInput.GetNextInput()];
                Vector3 nextWorldDirectionVector = transform.TransformDirection(nextLocalDirectionVector);
                Vector3 nextStepDestination = transform.position + (nextWorldDirectionVector * _stepDistance);

                if (CanPerformNextStep(nextStepDestination))
                {
                    UpdateActiveStepData(nextWorldDirectionVector);
                }
            }

            if (_isReadyToStep == false && _activeStepDirectionVector != Vector3.zero)
            {
                float unitPerSecond = (_stepDistance / _stepTimeSeconds);
                _activeStepProgress += unitPerSecond * deltaTime;
                transform.position = Vector3.Lerp(_activeStepOrigin, _activeStepDestination, _activeStepProgress);
            }

            if (_activeStepProgress > 0.98f) {
                EndStep();
            }
        }

        private void UpdateActiveStepData(Vector3 directionVector)
        {
            _activeStepDirectionVector = directionVector;

            _activeStepOrigin = transform.position;
            _activeStepDestination = _activeStepOrigin + (directionVector * _stepDistance);

            _activeStepProgress = 0;
            _isReadyToStep = false;
        }

        private void EndStep()
        {
            transform.position = _activeStepDestination;
            _activeStepDirectionVector = Vector3.zero;

            _isReadyToStep = true;
        }

        private bool CanPerformNextStep(Vector3 _destination)
        {
            return ! (Physics.OverlapBox(_destination, _collider.bounds.extents, transform.rotation, _stepDenyingLayerMask.value).Length > 0);
        }
    }
}