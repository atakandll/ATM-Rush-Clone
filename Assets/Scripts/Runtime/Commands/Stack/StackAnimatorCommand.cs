using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackAnimatorCommand
    {
        private StackManager _stackManager;
        private StackData _stackData;
        private List<GameObject> _collectableStack;

        public StackAnimatorCommand(StackManager stackManager, StackData stackData,
            ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _stackData = stackData;
            _collectableStack = collectableStack;
        }

        public IEnumerator Execute()
        {
            for (int i = 0; i <= _collectableStack.Count - 1; i++)
            {
                int index = (_collectableStack.Count - 1) - i;
                _collectableStack[index].transform
                    .DOScale(
                        new Vector3(_stackData.StackScaleValue, _stackData.StackScaleValue, _stackData.StackScaleValue),
                        _stackData.StackAnimDuraction).SetEase(Ease.Flash);
                _collectableStack[index].transform.DOScale(Vector3.one, _stackData.StackAnimDuraction)
                    .SetDelay(_stackData.StackAnimDuraction).SetEase(Ease.Flash);
                yield return new WaitForSeconds(_stackData.StackAnimDuraction / 3);
            }
        }
    }
}