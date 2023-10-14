
using System.Collections;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private PlayerPhysicsController physicsController;
        [SerializeField] private PlayerMeshController meshController;

        #endregion

        #region Private Variables

        [ShowInInspector] private PlayerData _data;
        private const string PlayerDataPath = "Data/CD_Player";

        #endregion

        #endregion

        private void Awake()
        {
            GetPlayerData();
            SendPlayerDataToControllers();
        }
        private PlayerData GetPlayerData() => Resources.Load<CD_Player>(PlayerDataPath).Data;

        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(_data.MovementData);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        { 
            InputSignals.Instance.onInputTaken += () => movementController.IsReadyToMove(true);
            InputSignals.Instance.onInputReleased += () => movementController.IsReadyToMove(false);
            InputSignals.Instance.onInputDragged += OnInputDragged;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful += () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onLevelFailed += () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onReset += OnReset;
            
            ScoreSignals.Instance.onSetTotalScore += meshController.OnSetTotalScore;
            CoreGameSignals.Instance.onMiniGameEntered += OnMiniGameEntered;
        }
        

        private void OnPlay()
        {
            movementController.IsReadyToPlay(true);
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Run);
        }
        
        private void OnInputDragged(HorizontalInputParams inputParams)
        {
            movementController.UpdateInputValue(inputParams);
        }
        
        private void OnMiniGameEntered()
        {
            movementController.IsReadyToPlay(false);
            StartCoroutine(WaitForFinal());
        }
        
        private void OnReset()
        {
            movementController.OnReset();
            animationController.OnReset();
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= () => movementController.IsReadyToMove(true);
            InputSignals.Instance.onInputReleased -= () => movementController.IsReadyToMove(false);
            InputSignals.Instance.onInputDragged -= OnInputDragged;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful -= () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onLevelFailed -= () => movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        IEnumerator WaitForFinal()
        {
            PlayerSignals.Instance.onChangePlayerAnimationState(PlayerAnimationStates.Idle);
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
            CoreGameSignals.Instance.onMiniGameStart?.Invoke();
        }
    }
}