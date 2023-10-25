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

        public void Execute(GameObject collectableGameobject)
        {
            int index = _collectableStack.IndexOf(collectableGameobject);
            int last = _collectableStack.Count - 1;
            _collectableStack.RemoveAt(index);
            _collectableStack.TrimExcess();
            collectableGameobject.transform.SetParent(_levelHolder.transform.GetChild(0));
            collectableGameobject.SetActive(false);
            _stackManager.StackJumperCommand.Execute(last, index); // we jump collectables that collision with obstacles index to last
            _stackManager.StackTypeUpdaterCommand.Execute();
        }
    }
}