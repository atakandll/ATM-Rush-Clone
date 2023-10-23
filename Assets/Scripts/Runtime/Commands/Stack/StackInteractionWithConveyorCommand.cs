using Runtime.Data.ValueObject;
using Runtime.Managers;

namespace Runtime.Commands.Stack
{
    public class StackInteractionWithConveyorCommand
    {
        private StackManager _stackManager;
        
        public StackInteractionWithConveyorCommand(StackManager stackManager)
        {
            _stackManager = stackManager;
            
        }
    }
}