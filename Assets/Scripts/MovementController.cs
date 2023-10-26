using System.Collections;
using System.Collections.Generic;
using Gybe.Game;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Gybe.Game
{

    public class MovementController : MonoBehaviour
    {
        public Image joystickBase;
        public Image joystickHandle;

        private bool _isDragging = false;
        private Vector2 _startTouch;
        public Vector2 swipeDelta;

        public float speed = 5f;
    public float rotationSpeed = 200f;
        public float maxJoystickDelta = 50f; // Max distance joystick handle can move

        private UnityEngine.AI.NavMeshAgent _agent;

        private IPlayerData _playerData;

        [Inject]
        public void Construct(IPlayerData playerData)
        {
            _playerData = playerData;
        }

        void Start()
        {
            // Initially hide the joystick UI
            joystickBase.gameObject.SetActive(false);
            joystickHandle.gameObject.SetActive(false);

            _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (!_agent)
            {
                Debug.LogError("NavMesh Agent component missing!");
            enabled = false; // Disable the script if there's no NavMesh Agent
                return;
            }

            _agent.speed = speed;
            _agent.angularSpeed = rotationSpeed;
        }

        void Update()
        {
            swipeDelta = Vector2.zero;

            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                _startTouch = Input.mousePosition;

                // Show joystick UI at touch position
                joystickBase.transform.position = _startTouch;
                joystickHandle.transform.position = _startTouch;
                joystickBase.gameObject.SetActive(true);
                joystickHandle.gameObject.SetActive(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                joystickBase.gameObject.SetActive(false);
                joystickHandle.gameObject.SetActive(false);
            }

            if (_isDragging)
            {
                if (Input.touches.Length > 0)
                {
                    swipeDelta = Input.touches[0].position - _startTouch;
                }
                else // For editor
                {
                    swipeDelta = (Vector2)Input.mousePosition - _startTouch;
                }

                // Update the joystick handle position
                joystickHandle.transform.position = _startTouch + Vector2.ClampMagnitude(swipeDelta, maxJoystickDelta);
            }

            if (swipeDelta.magnitude > 0)
            {
                Vector3 moveDirection = new Vector3(swipeDelta.x, 0, swipeDelta.y).normalized;
                _agent.SetDestination(transform.position + moveDirection);

                // For a smoother rotation, you can still use the following
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}