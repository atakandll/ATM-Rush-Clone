using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackInteractionWithConveyorCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;
        
        public StackInteractionWithConveyorCommand(StackManager stackManager, ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
            
        }

        public void Execute()
        {
            _stackManager.LastCheck = true;
            int i = _collectableStack.Count - 1;
            _collectableStack[i].transform.SetParent(_levelHolder.transform.GetChild(0));
            _collectableStack[i].transform.DOScale(Vector3.zero, 2.5f); // Bu, nesnenin kaybolmasına neden olur
            _collectableStack[i].transform.DOMove(new Vector3(-10, 2, _collectableStack[i].transform.position.z), 1.5f);
            _collectableStack.RemoveAt(i);
            _collectableStack.TrimExcess();
        }
    }
}