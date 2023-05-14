using System;
using Mirzipan.Heist.Commands;

namespace Mirzipan.Heist.Processors
{
    public interface IClientProcessor : IProcessor
    {
        void Tick();
        event Action<ICommand> OnCommandExecution;
        event Action<ICommand> OnCommandExecuted;
    }
}