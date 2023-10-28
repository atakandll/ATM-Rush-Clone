using System;
using System.Collections;
using DG.Tweening;
using Runtime.Controllers.MiniGame;
using Runtime.Signals;
using TMPro;
using UnityEngine;

namespace Runtime.Managers
{
    public class MiniGameManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject wallObject;
        [SerializeField] private GameObject fakeMoneyObject;
        [SerializeField] private Transform fakePlayer;
        [SerializeField] private Material mat;
        
        [SerializeField] private short wallCount, fakeMoneyCount;
        
        [SerializeField] private WallCheckController wallCheckController;

        #endregion
        
        #region Private Veriables

        private int _score;
        private float _multiplier;
        private Vector3 _initializePos;

        #endregion
        
        
        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            ScoreSignals.Instance.onSendFinalScore += OnSendScore;
            ScoreSignals.Instance.onGetMultiplier += OnGetMultiplier;
            CoreGameSignals.Instance.onMiniGameStart += OnMiniGameStart;
            CoreGameSignals.Instance.onReset += OnReset;
        }
        
        private void OnMiniGameStart()
        {
            fakePlayer.gameObject.SetActive(true);
            StartCoroutine(GoUp());
        }

        private IEnumerator GoUp()
        {
            yield return new WaitForSeconds(1f);
            if (_score == 0)
            {
                CoreGameSignals.Instance.onLevelFailed?.Invoke();
            }
            else
            {
                fakePlayer.DOLocalMoveY(Mathf.Clamp(_score, 0, 900), 2.7f).SetEase(Ease.Flash).SetDelay(1f);
                yield return new WaitForSeconds(4.5f);
                CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
            }
        }
        internal void SetMultiplier(float multiplierValue)
        {
            _multiplier = multiplierValue;
        }

        private float OnGetMultiplier()
        {
            return _multiplier;
        }

        private void OnSendScore(int scoreValue)
        {
           _score = scoreValue;
        }

        private void UnSubscribeEvents()
        {
            ScoreSignals.Instance.onSendFinalScore -= OnSendScore;
            ScoreSignals.Instance.onGetMultiplier -= OnGetMultiplier;
            CoreGameSignals.Instance.onMiniGameStart -= OnMiniGameStart;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

       

        private void Start()
        {
            SpawnWallObjects();
            SpawnFakeMoneyObjects();
            Init();
        }

        private void Init()
        {
            _initializePos = fakePlayer.localPosition;
        }

        private void SpawnWallObjects()
        {
            for (int i = 0; i < wallCount; i++)
            {
                var ob = Instantiate(wallObject, transform);
                ob.transform.localPosition = new Vector3(0, i * 30, 0);
                ob.transform.GetChild(0).GetComponent<TextMeshPro>().text = "x" + ((i / 10f) + 1f);

            }
        }

        private void SpawnFakeMoneyObjects()
        {
            for (int i = 0; i < fakeMoneyCount; i++)
            {
                var ob = Instantiate(fakeMoneyObject, fakePlayer);
                ob.transform.localPosition = new Vector3(0, -1 * 1.58f, -7);

            }
        }

        private void ResetWalls()
        {
            for (int i = 1; i < wallCount; i++)
            {
                transform.GetChild(i).GetComponent<Renderer>().material = mat;
                transform.GetChild(i).transform.position = Vector3.zero;
            }
        }
        private void OnReset()
        {
            StopAllCoroutines();
            DOTween.KillAll();
            ResetWalls();
            ResetFakePlayer();
        }

        private void ResetFakePlayer()
        {
            fakePlayer.gameObject.SetActive(false);
            fakePlayer.localPosition = _initializePos;
            wallCheckController.OnReset();

        }
    }
}