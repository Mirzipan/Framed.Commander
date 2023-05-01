using Mirzipan.Heist.Commands;

namespace Mirzipan.Heist.Processors
{
    public interface IServerProcessor : IProcessor
    {
        void Execute(ICommand command);
    }
}