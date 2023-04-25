using Mirzipan.Heist.Commands;

namespace Mirzipan.Heist.Processors
{
    public interface IResolver
    {
        IActionHandler ResolveHandler(IAction action);
        ICommandReceiver ResolveReceiver(ICommand command);
    }
}