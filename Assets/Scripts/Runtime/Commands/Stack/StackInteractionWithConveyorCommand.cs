using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackInteractionWithConveyorCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        
        public StackInteractionWithConveyorCommand(StackManager stackManager, ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            
        }
    }
}