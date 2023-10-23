using Runtime.Data.ValueObject;
using Runtime.Managers;

namespace Runtime.Commands.Stack
{
    public class ItemRemoverOnStackCommand
    {
        private StackManager _stackManager;
        
        public ItemRemoverOnStackCommand(StackManager stackManager)
        {
            _stackManager = stackManager;
            
        }
    }
}