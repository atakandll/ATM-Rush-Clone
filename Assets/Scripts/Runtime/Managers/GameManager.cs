using System;
using Runtime.Enums;
using Runtime.Extensions;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Self Variables

        #region Public Variables

        public GameStates States;

        #endregion

        #endregion

        protected override void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameStates += OnChangeGameStates;
        }

        private void OnChangeGameStates(GameStates newStates)
        {
            States = newStates;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameStates -= OnChangeGameStates;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

    }
}