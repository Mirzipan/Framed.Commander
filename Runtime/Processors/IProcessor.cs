using System;

namespace Mirzipan.Heist.Processors
{
    public interface IProcessor
    {
        event Action<ICommand> OnCommandExecution;
        event Action<ICommand> OnCommandExecuted;
        ValidationResult Validate(IAction action);
        void Process(IAction action);
    }
}