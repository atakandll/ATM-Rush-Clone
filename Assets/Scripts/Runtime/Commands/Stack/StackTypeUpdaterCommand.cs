using Runtime.Managers;

namespace Runtime.Commands.Stack
{
    public class StackTypeUpdaterCommand
    {
        private StackManager _stackManager;
        public StackTypeUpdaterCommand(StackManager stackManager)
        {
            _stackManager = stackManager;
        }
    }
}