using Mirzipan.Heist.Commands;

namespace Mirzipan.Heist.Processors
{
    public interface IServerProcessor : IProcessor
    {
        ValidationResult Validate(IAction action, ValidationOptions options);
        void ProcessFromClient(IAction action);
        void ProcessFromServer(IAction action);
        void Execute(ICommand command);
    }
}