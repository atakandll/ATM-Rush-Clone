using System;
using Runtime.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class ShopPanelController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshProUGUI incomeLvlText;
        [SerializeField] private TextMeshProUGUI stackLvlText;
        [SerializeField] private TextMeshProUGUI stackValue;
        [SerializeField] private TextMeshProUGUI incomeValue;
        [SerializeField] private TextMeshProUGUI money;
        [SerializeField] private Button incomeLvlButton;
        [SerializeField] private Button stackLvlButton;
     

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.onSetIncomeLvlText += OnSetIncomeLvlText;
            UISignals.Instance.onSetStackLvlText += OnSetStackLvlText;
        }
        private void UnSubscribeEvents()
        {
            UISignals.Instance.onSetIncomeLvlText -= OnSetIncomeLvlText;
            UISignals.Instance.onSetStackLvlText -= OnSetStackLvlText;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Start()
        {
            SyncShopUI();
        }

        private void SyncShopUI()
        {
            OnSetIncomeLvlText();
            OnSetStackLvlText();  
            ChangesIncomeInteractable();
            ChangesStackInteractable();
        }

        private void OnSetStackLvlText()
        {
            stackLvlText.text = "Stack lvl\n" + CoreGameSignals.Instance.onGetStackLevel();
            stackValue.text = (Mathf.Pow(2, Mathf.Clamp(CoreGameSignals.Instance.onGetStackLevel(), 0, 10)) * 100)
                .ToString();
        }

        private void OnSetIncomeLvlText()
        {
            incomeLvlText.text = "Income lvl\n" + CoreGameSignals.Instance.onGetIncomeLevel();
            incomeValue.text = (Mathf.Pow(2, Mathf.Clamp(CoreGameSignals.Instance.onGetIncomeLevel(), 0, 10)) * 100)
                .ToString();
        }
        
        private void ChangesIncomeInteractable()
        {
            if (int.Parse(UISignals.Instance.onGetMoneyValue?.Invoke().ToString()!) < int.Parse(incomeValue.text) ||
                CoreGameSignals.Instance.onGetIncomeLevel() >= 30)
            {
                incomeLvlButton.interactable = false;
            }

            else
            {
                incomeLvlButton.interactable = true;
            }
        }

        private void ChangesStackInteractable()
        {
            if (int.Parse(UISignals.Instance.onGetMoneyValue?.Invoke().ToString()!) < int.Parse(stackValue.text) ||
                CoreGameSignals.Instance.onGetStackLevel() >= 15)
            {
                stackLvlButton.interactable = false;
            }
            else
            {
                stackLvlButton.interactable = true;
            }
        }
    }
}