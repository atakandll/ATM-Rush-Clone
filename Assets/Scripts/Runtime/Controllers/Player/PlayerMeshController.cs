﻿using System;
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
        
        public void SetTotalScore(int score)
        {
           scoreText.text = score.ToString();
        }
    }
}