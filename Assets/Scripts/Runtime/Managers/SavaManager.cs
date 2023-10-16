using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class SavaManager : MonoBehaviour
    {
        #region EventSubscribtion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onSaveGameData += SaveData;
        }

        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onSaveGameData -= SaveData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void SaveData()
        {
            Debug.LogWarning(ScoreSignals.Instance.onGetMoney());
            OnSaveGame(
                new SaveGameDataParams()
                {
                    Money = ScoreSignals.Instance.onGetMoney(),
                    Level = CoreGameSignals.Instance.onGetLevelID(),
                    IncomeLevel = CoreGameSignals.Instance.onGetIncomeLevel(),
                    StackLevel = CoreGameSignals.Instance.onGetStackLevel()
                }
            );
        }

        private void OnSaveGame(SaveGameDataParams saveDataParams)
        {
            ES3.Save("Level", saveDataParams.Level);
            ES3.Save("Money", saveDataParams.Money);
            ES3.Save("IncomeLevel", saveDataParams.IncomeLevel);
            ES3.Save("StackLevel", saveDataParams.StackLevel);
        }
    }
}