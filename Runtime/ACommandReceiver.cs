namespace Mirzipan.Heist
{
    public abstract class ACommandReceiver<T> : ICommandReceiver where T : ICommand
    {
        public void Execute(ICommand command)
        {
            Execute((T)command);
        }

        protected abstract void Execute(T command);
    }
}