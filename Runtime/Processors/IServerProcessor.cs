using System;

namespace Mirzipan.Heist.Processors
{
    public interface IServerProcessor : IProcessor
    {
        event Action<IAction> OnActionProcessing;
        event Action<IAction> OnActionProcessed;
        ValidationResult Validate(IAction action, int clientId, ValidationOptions options);
        void ProcessClientAction(IAction action, int clientId);
        void ProcessServerAction(IAction action);
        void Execute(ICommand command, int[] clientIds, ExecuteOn target);
    }
}