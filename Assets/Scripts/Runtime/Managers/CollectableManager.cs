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

        [ShowInInspector] private CollectableData _data;
        [ShowInInspector] private byte _currentValue = 0;

        private readonly string _collectableDataPath = "Data/CD_Collectable";

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetCollectableData();
            SendDataToController();
        }

        private CollectableData GetCollectableData() => Resources.Load<CD_Collectable>(_collectableDataPath).Data;

        private void SendDataToController()
        {
            meshController.SetMeshData(_data.MeshData);
        }

      
        internal void CollectableUpgrade(int value)
        {
            if (_currentValue < 2) _currentValue++;
            meshController.OnUpgradeCollectableVisuals(_currentValue);
            StackSignals.Instance.onUpdateType?.Invoke();
        }

        public byte GetCurrentValue()
        {
            return _currentValue;
        }

        public void InteractionWithCollectable(GameObject collectableGameObject)
        {
            StackSignals.Instance.onInteractionCollectable?.Invoke(collectableGameObject);
        }

        public void InteractionWithAtm(GameObject collectableGameObject)
        {
            StackSignals.Instance.onInteractionATM?.Invoke(collectableGameObject);
        }

        public void InteractionWithObstacle(GameObject collectableGameObject)
        {
            StackSignals.Instance.onInteractionObstacle?.Invoke(collectableGameObject);
        }

        public void InteractionWithConveyor()
        {
            StackSignals.Instance.onInteractionConveyor?.Invoke();
        }

        
    }
}