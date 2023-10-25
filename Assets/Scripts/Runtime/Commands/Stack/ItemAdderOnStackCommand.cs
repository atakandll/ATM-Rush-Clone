using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class ItemAdderOnStackCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private StackData _data;
        public ItemAdderOnStackCommand(StackManager stackManager, ref List<GameObject> collectableStack, ref StackData stackData)
        {
           _stackManager = stackManager;
           _data = stackData;
           _collectableStack = collectableStack;
        }

        public void Execute(GameObject collectableGameobject)
        {
            if (_collectableStack.Count <= 0)
            {
                _collectableStack.Add(collectableGameobject);
                collectableGameobject.transform.SetParent(_stackManager.transform);
                collectableGameobject.transform.localPosition = Vector3.zero; // new collectable gameobject position = centre position
            }
            else
            {
                collectableGameobject.transform.SetParent(_stackManager.transform);
                Vector3 newPos = _collectableStack[_collectableStack.Count - 1].transform.localPosition; // new collectable position is last gameobject position
                newPos.z += _data.CollectableOffsetInStack;
                collectableGameobject.transform.localPosition = newPos;
                _collectableStack.Add(collectableGameobject);
            }
        }
    }
}