using System.Collections.Generic;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackInitializerCommand
    {
        private StackManager _stackManager;
        private GameObject _money;
        public StackInitializerCommand(StackManager stackManager, ref GameObject money)
        {
            _stackManager = stackManager;
            _money = money;
        }

        public void Execute()
        {
            var stackLevel = CoreGameSignals.Instance.onGetStackLevel();
            for (int i = 1; i < stackLevel; i++)
            {
                GameObject obj = Object.Instantiate(_money);
                _stackManager.ItemAdderOnStackCommand.Execute(obj);
            }

            _stackManager.StackTypeUpdaterCommand.Execute();
        }
    }
}