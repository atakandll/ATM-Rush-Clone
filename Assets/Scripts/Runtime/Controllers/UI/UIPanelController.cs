using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Controllers.UI
{
    public class UIPanelController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<GameObject> layers = new List<GameObject>();

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
            CoreUISignals.Instance.onClosePanel += OnClosePanel;
            CoreUISignals.Instance.onCloseAllPanel += OnCloseAllPanel;
        }
        private void UnsubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
            CoreUISignals.Instance.onClosePanel -= OnClosePanel;
            CoreUISignals.Instance.onCloseAllPanel -= OnCloseAllPanel;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        [Button("Open Panel")]
        private void OnOpenPanel(UIPanelTypes panel, int layerValue)
        {
           CoreUISignals.Instance.onClosePanel?.Invoke(layerValue);
           Instantiate(Resources.Load<GameObject>($"Screens/{panel}Panel"), layers[layerValue].transform);
        }
        
        [Button("Close Panel")]
        private void OnClosePanel(int layerValue)
        {
            if (layers[layerValue].transform.childCount > 0)
            {
                for (int i = 0; i < layers[layerValue].transform.childCount; i++)
                {
                    Destroy(layers[layerValue].transform.GetChild(i).gameObject);
                }
            }
            
        }
        [Button ("Close All Panel")]
        private void OnCloseAllPanel()
        {
            foreach (var layer in layers)
            {
                for (int i = 0; i < layer.transform.childCount; i++)
                {
                    Destroy(layer.transform.GetChild(i).gameObject);
                    
                }
            }
        }

        
    }
}