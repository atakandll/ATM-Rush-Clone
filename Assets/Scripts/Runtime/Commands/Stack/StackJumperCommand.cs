using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackJumperCommand
    {
        private StackData _data;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;

        public StackJumperCommand(StackData stackData, ref List<GameObject> collectableStack)
        {
            _data = stackData;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
        }

        public void Execute(int last, int index)
        {
            for (int i = last; i > index; i--)
            {
                _collectableStack[i].transform.GetChild(1).tag = "Collectable";
                _collectableStack[i].transform.SetParent(_levelHolder.transform.GetChild(0));
                _collectableStack[i].transform.DOJump(
                    new Vector3(
                        Random.Range(-_data.JumpItemsClampX, _data.JumpItemsClampX + 1),
                        1.12f,
                        _collectableStack[i].transform.position.z + Random.Range(10, 15)),
                    _data.JumpForce,
                    Random.Range(1, 3), 0.5f
                );
                _collectableStack[i].transform.DOScale(Vector3.one, 0);
                _collectableStack.RemoveAt(i);
                _collectableStack.TrimExcess();
            }
        }
    }
}