using System;
using DG.Tweening.Plugins.Options;
using Runtime.Data.ValueObject;
using Runtime.Keys;
using Runtime.Managers;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private Rigidbody rigidbody;

        #endregion

        #region Private Variables

        [ShowInInspector] private PlayerMovementData _data;
        [ShowInInspector] private bool _isReadyToPlay, _isReadyToMove;
        [ShowInInspector] private float _inputValue;
        [ShowInInspector] private Vector2 _clampValue;

        #endregion

        #endregion
        internal void SetMovementData(PlayerMovementData movementData)
        {
            _data = movementData;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerSignals.Instance.onPlayConditionChanged += onPlayConditionChanged;
            PlayerSignals.Instance.onMoveConditionChanged += onMoveConditionChanged;
        }
        private void UnSubscribeEvents()
        {
            PlayerSignals.Instance.onPlayConditionChanged -= onPlayConditionChanged;
            PlayerSignals.Instance.onMoveConditionChanged -= onMoveConditionChanged;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        private void onPlayConditionChanged(bool condition) => _isReadyToPlay = condition;
        private void onMoveConditionChanged(bool condition) => _isReadyToMove = condition;


        public void UpdateInputValue(HorizontalInputParams inputParams)
        {
            _inputValue = inputParams.HorizontalInputValue;
            _clampValue = inputParams.HorizontalInputClampSides;
        }

        private void Update()
        {
            if (_isReadyToPlay)
            {
                manager.SetStackPosition();
            }
        }

        private void FixedUpdate()
        {
            if (_isReadyToPlay)
            {
                if (_isReadyToMove)
                {
                    Move();
                }
                else
                {
                    StopSideways();
                }
            }
            else
            {
                Stop();
            }
        }

        private void Move()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValue * _data.SidewaysSpeed, velocity.y, _data.ForwardSpeed);
            rigidbody.velocity = velocity;

            Vector3 position;
            position = new Vector3(Mathf.Clamp(rigidbody.position.x, _clampValue.x, _clampValue.y),
                (position = rigidbody.position).y, position.z);

            rigidbody.position = position;
        }

        private void StopSideways()
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _data.ForwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
        }

        private void Stop()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            
        }

        public void OnReset()
        {
            Stop();
           _isReadyToMove = false;
           _isReadyToPlay = false;
        }
    }
}