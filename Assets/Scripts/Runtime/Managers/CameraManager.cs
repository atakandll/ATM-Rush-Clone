using System;
using Cinemachine;
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

        [SerializeField] private Animator animator;
        [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;

        #endregion

        #region Private Variables

        [ShowInInspector] private float3 _initialPosition;

        #endregion

        #endregion

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

        private void OnChangeCameraState(CameraStates cameraState)
        {
            animator.SetTrigger(cameraState.ToString());
        }

        private void OnSetCinemachineTarget(CameraTargetState arg0)
        {
            //var playerManager = FindObjectOfType<PlayerManager>().transform;
            //stateDrivenCamera.Follow = playerManager;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            CameraSignals.Instance.onSetCinemachineTarget -= OnSetCinemachineTarget;
            CameraSignals.Instance.onChangeCameraState -= OnChangeCameraState;
        }
        private void OnReset()
        {
           CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.Initial);
           stateDrivenCamera.Follow = null;
           stateDrivenCamera.LookAt = null;
           transform.position = _initialPosition;
        }
    }
}