using System;
using Cinemachine;
using Runtime.Controllers.MiniGame;
using Runtime.Enums;
using Runtime.Signals;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
        [SerializeField] private Animator animator;

        #endregion

        #region Private Variables

        [ShowInInspector] private float3 _initialPosition;

        #endregion

        #endregion

        #region Event Subscriptions

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _initialPosition = transform.position;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            CameraSignals.Instance.onSetCinemachineTarget += OnSetCinemachineTarget;
            CameraSignals.Instance.onChangeCameraState += OnChangeCameraState;
        }

        private void OnSetCinemachineTarget(CameraTargetState state)
        {
            switch (state)
            {
                case CameraTargetState.Player:
                {
                    var playerManager = FindObjectOfType<PlayerManager>().transform;
                    stateDrivenCamera.Follow = playerManager;
                }
                    break;
                case CameraTargetState.FakePlayer:
                {
                    stateDrivenCamera.Follow = null;
                    var fakePlayer = FindObjectOfType<WallCheckController>().transform.parent.transform;
                    stateDrivenCamera.Follow = fakePlayer;
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void OnChangeCameraState(CameraStates state)
        {
            animator.SetTrigger(state.ToString());
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            CameraSignals.Instance.onSetCinemachineTarget -= OnSetCinemachineTarget;
            CameraSignals.Instance.onChangeCameraState -= OnChangeCameraState;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion


        private void OnReset()
        {
            CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.Initial);
            stateDrivenCamera.Follow = null;
            stateDrivenCamera.LookAt = null;
            transform.position = _initialPosition;
        }
    }
}