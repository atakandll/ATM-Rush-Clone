using Runtime.Data.ValueObject;
using Runtime.Keys;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        [ShowInInspector] private PlayerMovementData _data;
        
        [Header("Additional Value")] [ShowInInspector]
        private bool _isReadyToPlay, _isReadyToMove;
 
        #endregion

        #endregion
        internal void SetMovementData(PlayerMovementData movementData)
        {
            _data = movementData;
        }

        internal void IsReadyToMove(bool condition) => _isReadyToMove = condition;
        internal void IsReadyToPlay(bool condition) => _isReadyToPlay = condition;


        public void UpdateInputValue(HorizontalInputParams inputParams)
        {
            throw new System.NotImplementedException();
        }

        public void OnReset()
        {
            throw new System.NotImplementedException();
        }
    }
}