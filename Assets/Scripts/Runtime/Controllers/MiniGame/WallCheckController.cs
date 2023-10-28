using DG.Tweening;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Controllers.MiniGame
{
    public class WallCheckController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private MiniGameManager manager;

        #endregion

        #region Private Variables

        private float _changesColor;
        private float _multiplier;

        private readonly string _wall = "Wall";

        #endregion

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_wall)) return;
            _multiplier += 0.1f;
            manager.SetMultiplier(_multiplier);
            ChangeColor(other);
        }

        private void ChangeColor(Collider other)
        {
            _changesColor = (0.036f + _changesColor) % 1;
            var otherGameObject = other.gameObject;
            otherGameObject.GetComponent<Renderer>().material.DOColor(Color.HSVToRGB(_changesColor, 1, 1), 0.1f);
            otherGameObject.transform.DOLocalMoveZ(-3, 0.1f);
        }
        public void OnReset()
        {
            _changesColor = 0;
            _multiplier = 0.98f;
        }
    }
}