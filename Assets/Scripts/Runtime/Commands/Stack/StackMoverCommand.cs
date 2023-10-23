using Runtime.Data.ValueObject;
using Runtime.Managers;

namespace Runtime.Commands.Stack
{
    public class StackMoverCommand
    {
        private StackManager _stackManager;
        private StackData _data;
        public StackMoverCommand(StackManager stackManager, ref StackData stackData)
        {
            _stackManager = stackManager;
            _data = stackData;
        }
    }
}