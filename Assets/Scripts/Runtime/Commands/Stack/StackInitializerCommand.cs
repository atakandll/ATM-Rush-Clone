using System.Collections.Generic;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackInitializerCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        public StackInitializerCommand(StackManager stackManager, ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}