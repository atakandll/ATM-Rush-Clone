using System;
using DG.Tweening;
using Runtime.Signals;
using TMPro;
using UnityEngine;

namespace Runtime.Managers
{
    public class AtmManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private DOTweenAnimation dOTweenAnimation;
        [SerializeField] private TextMeshPro atmText;

        #endregion
        #endregion

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
            dOTweenAnimation = GetComponentInChildren<DOTweenAnimation>();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onAtmTouched += OnAtmTouched;
            AtmSignals.Instance.onSetAtmScoreText += OnSetAtmScoreText;
        }
        
        private void OnAtmTouched(GameObject touchedAtmObject)
        {
            if (touchedAtmObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                dOTweenAnimation.DOPlay();
            }
        }
        private void OnSetAtmScoreText(int value)
        {
            atmText.text = value.ToString();
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onAtmTouched -= OnAtmTouched;
            AtmSignals.Instance.onSetAtmScoreText -= OnSetAtmScoreText;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

       
    }
}