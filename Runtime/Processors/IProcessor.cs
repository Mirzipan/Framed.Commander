using Mirzipan.Heist.Commands;

namespace Mirzipan.Heist.Processors
{
    public interface IProcessor
    {
        ValidationResult Validate(IAction action);
        void Process(IAction action);
        void Tick();
        void Execute(ICommand command);
    }
}