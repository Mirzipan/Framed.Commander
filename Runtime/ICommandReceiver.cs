namespace Mirzipan.Heist
{
    public interface ICommandReceiver
    {
        void Execute(ICommand command);
    }
}