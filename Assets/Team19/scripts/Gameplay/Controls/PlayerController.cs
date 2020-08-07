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

        private bool _isReadyToStep = true;

        private Team19.Input.PlayerInput _playerInput;

        private Vector3 _activeStepDirectionVector = Vector3.zero;
        private Vector3 _activeStepOrigin = Vector3.zero;
        private Vector3 _activeStepDestination = Vector3.zero;

        private float _activeStepProgress = 0;

        private Dictionary<Team19.Input.EInputDirection, Vector3> _inputDirectionToVectorMap;

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
        }

        private void Update()
        {
            _playerInput.UpdateInput();
            UpdateMovement(Time.deltaTime);
        }

        private void UpdateMovement(float deltaTime)
        {
            if (_playerInput.IsInputAvailable && _isReadyToStep)
            {
                _activeStepDirectionVector = _inputDirectionToVectorMap[_playerInput.GetNextInput()];

                _activeStepOrigin = transform.position;
                _activeStepDestination = _activeStepOrigin + ( _activeStepDirectionVector * _stepDistance );

                _activeStepProgress = 0;
                _isReadyToStep = false;
            }

            if (_isReadyToStep == false && _activeStepDirectionVector != Vector3.zero)
            {
                float unitPerSecond = (_stepDistance / _stepTimeSeconds);
                _activeStepProgress += unitPerSecond * deltaTime;
                transform.position = Vector3.Lerp(_activeStepOrigin, _activeStepDestination, _activeStepProgress);
            }

            if (_activeStepProgress > 0.98f) {
                transform.position = _activeStepDestination;
                _activeStepDirectionVector = Vector3.zero;

                _isReadyToStep = true;
            }
        }
    }
}
