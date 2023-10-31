using System.Collections.Generic;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackTypeUpdaterCommand
    {
        private List<GameObject> _collectableStack;
        private int _totalListScore;

        public StackTypeUpdaterCommand(ref List<GameObject> collectableStack)
        {
            _collectableStack = collectableStack;
        }

        public void Execute()
        {
            _totalListScore = 0;
            foreach (var items in _collectableStack)
            {
                _totalListScore += items.GetComponent<CollectableManager>().GetCurrentValue() + 1;
            }

            ScoreSignals.Instance.onSetScore?.Invoke(_totalListScore);
        }
    }
}