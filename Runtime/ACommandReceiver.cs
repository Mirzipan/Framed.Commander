namespace Mirzipan.Heist
{
    public abstract class ACommandReceiver<T> : ICommandReceiver where T : ICommand
    {
        public void Execute(ICommand command, ExecutionOptions options)
        {
            Execute((T)command, options);
        }

        protected abstract void Execute(T command, ExecutionOptions options);
    }
}