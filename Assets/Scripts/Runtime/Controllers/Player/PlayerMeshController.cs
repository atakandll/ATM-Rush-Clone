using System;
using Runtime.Handler;
using Runtime.Signals;
using TMPro;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshPro scoreText;

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
           PlayerSignals.Instance.onSetTotalScore += OnSetTotalScore;
        }
        private void UnsubscribeEvents()
        {
            PlayerSignals.Instance.onSetTotalScore -= OnSetTotalScore;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void OnSetTotalScore(int score)
        {
           scoreText.text = score.ToString();
        }
    }
}