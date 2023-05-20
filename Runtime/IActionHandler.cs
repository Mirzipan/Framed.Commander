namespace Mirzipan.Heist
{
    public interface IActionHandler
    {
        ValidationResult Validate(IAction action, int clientId, ValidationOptions options);
        void Process(IAction action, int clientId);
    }
}