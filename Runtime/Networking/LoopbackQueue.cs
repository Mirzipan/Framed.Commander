namespace Mirzipan.Heist.Networking
{
    public sealed class LoopbackQueue : IIncomingActions, IOutgoingActions, IIncomingCommands, IOutgoingCommands
    {
        public event ActionReceived OnActionReceived;
        public event CommandReceived OnCommandReceived;
        
        #region Lifecycle

        public void Dispose()
        {
            OnActionReceived = null;
            OnCommandReceived = null;
        }

        #endregion Lifecycle

        #region Public

        public void Send(IAction action)
        {
            OnActionReceived?.Invoke(action, ClientId.Default);
        }

        public void Send(ICommand command, int[] clientIds, bool sendToAll)
        {
            OnCommandReceived?.Invoke(command);
        }

        #endregion Public
    }
}