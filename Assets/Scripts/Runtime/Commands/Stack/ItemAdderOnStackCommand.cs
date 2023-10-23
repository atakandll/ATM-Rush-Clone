using Runtime.Managers;

namespace Runtime.Commands.Stack
{
    public class ItemAdderOnStackCommand
    {
        private StackManager _stackManager;
        public ItemAdderOnStackCommand(StackManager stackManager)
        {
           _stackManager = stackManager;
        }
    }
}