using System;
using Runtime.Controllers.Collectables;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class CollectableManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CollectableMeshController meshController;
        [SerializeField] private CollectablePhysicsController physicsController;

        #endregion

        #region Private Variables

        [ShowInInspector] private CollectableData _collectableData;
        [ShowInInspector] private byte _currentValue;
        
        
        
        private readonly string collectableDataPath = "Data/CD_Collectable";

        #endregion

        #endregion

        private void Awake()
        {
            _collectableData = GetCollectableData();
            SendDataToControllers();
        }
        private CollectableData GetCollectableData()
        {
            return Resources.Load<CD_Collectable>(collectableDataPath).Data;
        }
        private void SendDataToControllers()
        {
            meshController.SetMeshData(_collectableData.MeshData);
        }
        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            CollectableSignals.Instance.onCollectableUpgrade += OnCollectableUpgrade;
        }
        internal void OnCollectableUpgrade(int value)
        {
            if (_currentValue < 2) // Bu, bir tür sınırlama getirerek toplanabilir nesnenin yükseltme sayısını en fazla 2 yapar.
                _currentValue++;
            
            meshController.OnUpgradeCollectableVisuals(_currentValue);
            
            StackSignals.Instance.onUpdateType?.Invoke();
        }
        private void UnSubscribeEvents()
        {
            CollectableSignals.Instance.onCollectableUpgrade -= OnCollectableUpgrade;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        public byte GetCurrentValue()
        {
            return _currentValue;
        }
        public void InteractionWithCollectables(GameObject collectableGameObject)
        {
            StackSignals.Instance.onInteractionCollectable?.Invoke(collectableGameObject);
        }
        public void InteractionWithATM(GameObject atmGameObject)
        {
            StackSignals.Instance.onInteractionATM?.Invoke(atmGameObject); // stack listesinden çıkarılıp para olarak aktarma
        }
        public void InteractionWithObstacle(GameObject obstacleGameObject)
        {
            StackSignals.Instance.onInteractionObstacle?.Invoke(obstacleGameObject);
        }
        public void InteractionWithMiniGame()
        {
            StackSignals.Instance.onInteractionConveyor?.Invoke();
        }

        
    }
}