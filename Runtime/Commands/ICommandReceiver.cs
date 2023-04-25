namespace Mirzipan.Heist.Commands
{
    public interface ICommandReceiver
    {
        void Execute(ICommand command);
    }
}