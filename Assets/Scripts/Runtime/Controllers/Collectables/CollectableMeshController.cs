using System;
using Runtime.Data.ValueObject;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Controllers.Collectables
{
    public class CollectableMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private new MeshFilter meshFilter;

        #endregion

        #region Private Variables

        [ShowInInspector] private CollectableMeshData _data;

        #endregion

        #endregion

        private void OnEnable()
        {
            ActivateMeshVisuals();
        }
        
        internal void SetMeshData(CollectableMeshData collectableDataMeshData)
        {
            _data = collectableDataMeshData;
        }
        private void ActivateMeshVisuals()
        {
           meshFilter.mesh = _data.MeshList[0];
        }


        public void OnUpgradeCollectableVisuals(int value)
        {
            meshFilter.mesh = _data.MeshList[value];
        }
    }
}