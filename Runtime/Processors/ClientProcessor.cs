using System;
using System.Collections.Generic;
using Mirzipan.Extensions;
using Mirzipan.Heist.Networking;

namespace Mirzipan.Heist.Processors
{
    public sealed class ClientProcessor : IClientProcessor, IDisposable
    {
        private const string LogTag = nameof(ClientProcessor);
        
        public event Action<ICommand> OnCommandExecution;
        public event Action<ICommand> OnCommandExecuted;
        
        private Queue<ICommand> _processingQueue = new();

        private IOutgoingActions _actions;
        private IIncomingCommands _commands;
        private IResolver _resolver;

        #region Lifecycle

        public ClientProcessor(IOutgoingActions actions, IIncomingCommands commands, IResolver resolver)
        {
            _actions = actions;
            _commands= commands;
            _resolver = resolver;

            _commands.OnCommandReceived += OnCommandReceived;
        }

        public void Tick()
        {
            while (_processingQueue.Count > 0)
            {
                ICommand command = _processingQueue.Dequeue();
                ICommandReceiver handler = _resolver.ResolveReceiver(command);
                
                OnCommandExecution.SafeInvoke(command);
                handler.Execute(command, ExecutionOptions.None);
                OnCommandExecuted.SafeInvoke(command);
            }
        }

        public void Dispose()
        {
            OnCommandExecution = null;
            OnCommandExecuted = null;

            _actions = null;
            _commands.OnCommandReceived -= OnCommandReceived;
            _commands = null;
            _resolver = null;
            
            _processingQueue.Clear();
            _processingQueue = null;
        }

        #endregion Lifecycle

        #region Public

        public ValidationResult Validate(IAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var handler = _resolver.ResolveHandler(action);
            return handler.Validate(action, 0, ValidationOptions.None);
        }

        public void Process(IAction action)
        {
            var result = Validate(action);
            if (!result.Success)
            {
                HeistLogger.Error(LogTag, $"Validation of {action} failed. code={result.Code}");
                return;
            }

            _actions?.Send(action);
        }

        #endregion Public

        #region Bindings

        private void OnCommandReceived(ICommand command)
        {
            _processingQueue.Enqueue(command);
        }

        #endregion Bindings
    }
}