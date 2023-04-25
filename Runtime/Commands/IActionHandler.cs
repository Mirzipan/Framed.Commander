namespace Mirzipan.Heist.Commands
{
    public interface IActionHandler
    {
        ValidationResult Validate(IAction action);
        void Process(IAction action);
    }
}