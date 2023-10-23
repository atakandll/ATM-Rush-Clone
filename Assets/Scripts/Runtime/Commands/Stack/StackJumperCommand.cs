using Runtime.Data.ValueObject;
using Runtime.Managers;

namespace Runtime.Commands.Stack
{
    public class StackJumperCommand
    {
        private StackManager _stackManager;
        private StackData _data;
        public StackJumperCommand(StackManager stackManager, StackData stackData)
        {
            _stackManager = stackManager;
            _data = stackData;
        }
    }
}