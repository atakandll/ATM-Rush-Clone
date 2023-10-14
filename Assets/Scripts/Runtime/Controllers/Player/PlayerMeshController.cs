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

        public void OnSetTotalScore(int score)
        {
           scoreText.text = score.ToString();
        }
    }
}