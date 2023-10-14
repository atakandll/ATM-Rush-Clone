using Runtime.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class ScoreSignals : MonoSingleton<ScoreSignals>
    {
        public UnityAction<int> onSetTotalScore = delegate { };
    }
}