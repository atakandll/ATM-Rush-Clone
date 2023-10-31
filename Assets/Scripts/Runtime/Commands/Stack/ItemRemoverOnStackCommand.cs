using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class ItemRemoverOnStackCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;

        public ItemRemoverOnStackCommand(StackManager stackManager, ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
        }

        public void Execute(GameObject collectableGameObject)
        {
            int index = _collectableStack.IndexOf(collectableGameObject);
            int last = _collectableStack.Count - 1;
            collectableGameObject.transform.SetParent(_levelHolder.transform.GetChild(0));
            collectableGameObject.SetActive(false);
            _stackManager.StackJumperCommand.Execute(last, index);
            _collectableStack.RemoveAt(index);
            _collectableStack.TrimExcess();
            _stackManager.StackTypeUpdaterCommand.Execute();
        }
    }
}