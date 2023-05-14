namespace Mirzipan.Heist.Commands
{
    public interface IActionHandler
    {
        ValidationResult Validate(IAction action, ValidationOptions options);
        void Process(IAction action);
    }
}