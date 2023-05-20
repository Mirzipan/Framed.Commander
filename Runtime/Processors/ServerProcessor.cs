using System;
using Mirzipan.Extensions;
using Mirzipan.Heist.Networking;

namespace Mirzipan.Heist.Processors
{
    public sealed class ServerProcessor : IServerProcessor, IDisposable
    {
        private const string LogTag = nameof(ServerProcessor);
        
        public event Action<ICommand> OnCommandExecution;
        public event Action<ICommand> OnCommandExecuted;
        
        private IOutgoingCommands _commands;
        private IIncomingActions _actions;
        private IResolver _resolver;

        #region Lifecycle

        public ServerProcessor(IIncomingActions actions, IOutgoingCommands commands, IResolver resolver)
        {
            _actions = actions;
            _commands = commands;
            _resolver = resolver;
            
            _actions.OnActionReceived += OnActionReceived;
        }

        public void Dispose()
        {
            OnCommandExecution = null;
            OnCommandExecuted = null;

            _actions.OnActionReceived -= OnActionReceived;
            _actions = null;
            _commands = null;
            _resolver = null;
        }

        #endregion Lifecycle

        #region Public

        public ValidationResult Validate(IAction action)
        {
            return Validate(action, ClientId.Server, ValidationOptions.SenderIsServer);
        }

        public ValidationResult Validate(IAction action, int clientId, ValidationOptions options)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var handler = _resolver.ResolveHandler(action);
            return handler.Validate(action, clientId, options);
        }

        public void Process(IAction action)
        {
            ProcessServerAction(action);
        }

        public void ProcessClientAction(IAction action, int clientId)
        {
            Process(action, clientId, ValidationOptions.None);
        }

        public void ProcessServerAction(IAction action)
        {
            Process(action, ClientId.Server, ValidationOptions.SenderIsServer);
        }

        public void Execute(ICommand command, int[] clientIds, ExecuteOn target)
        {
            if (target == ExecuteOn.None)
            {
                return;
            }

            bool executeOnServer = (target & ExecuteOn.Server) != 0;
            bool executeOnAllClients = (target & ExecuteOn.AllClients) != 0;
            bool executeOnAnyClient = executeOnAllClients || (target & ExecuteOn.OneClient) != 0;
            
            if (executeOnServer)
            {
                ExecuteOnServer(command);
            }

            if (executeOnAnyClient)
            {
                _commands.Send(command, clientIds, executeOnAllClients);
            }
        }

        #endregion Public

        #region Private

        private void Process(IAction action, int clientId, ValidationOptions options)
        {
            var result = Validate(action, clientId, options);
            if (!result.Success)
            {
                HeistLogger.Info(LogTag, $"Validation of {action} failed. code={result.Code}");
                return;
            }
            
            var handler = _resolver.ResolveHandler(action);
            handler.Process(action, clientId);
        }

        private void ExecuteOnServer(ICommand command)
        {
            var receiver = _resolver.ResolveReceiver(command);
            
            OnCommandExecution.SafeInvoke(command);
            receiver.Execute(command);
            OnCommandExecuted.SafeInvoke(command);
        }

        #endregion Private

        #region Bindings

        private void OnActionReceived(IAction action, int clientId)
        {
            ProcessClientAction(action, clientId);
        }

        #endregion Bindings
    }
}