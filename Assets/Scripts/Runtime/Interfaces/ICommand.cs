namespace Runtime.Interfaces
{
    public interface ICommand
    {
        public void Execute(int parameter){} // default method, override zorunluluğu yok
        public void Execute(){}
    }
}