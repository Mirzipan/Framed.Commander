using System;
using Mirzipan.Heist.Commands;
using Mirzipan.Heist.Networking;
using UnityEngine;

namespace Mirzipan.Heist.Processors
{
    public sealed class ServerProcessor : IServerProcessor, IDisposable
    {
        private INetwork _network;
        private IResolver _resolver;

        #region Lifecycle

        public ServerProcessor(INetwork network, IResolver resolver)
        {
            _network = network;
            _resolver = resolver;
            
            _network.OnReceived += OnReceived;
        }

        public void Dispose()
        {
            _network.OnReceived -= OnReceived;
            _network = null;
            _resolver = null;
        }

        #endregion Lifecycle

        #region Public

        public ValidationResult Validate(IAction action)
        {
            return Validate(action, ValidationOptions.SenderIsServer);
        }

        public ValidationResult Validate(IAction action, ValidationOptions options)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var handler = _resolver.ResolveHandler(action);
            return handler.Validate(action, options);
        }

        public void Process(IAction action)
        {
            ProcessFromServer(action);
        }

        public void ProcessFromClient(IAction action)
        {
            Process(action, ValidationOptions.None);
        }

        public void ProcessFromServer(IAction action)
        {
            Process(action, ValidationOptions.SenderIsServer);
        }

        public void Execute(ICommand command)
        {
            var receiver = _resolver.ResolveReceiver(command);
            receiver.Execute(command);
        }

        #endregion Public

        #region Private

        private void Process(IAction action, ValidationOptions options)
        {
            var result = Validate(action, options);
            if (!result.Success)
            {
                // TODO: do we need to log this?
                Debug.LogError($"Validation of {action} failed. code={result.Code}");
                return;
            }
            
            var handler = _resolver.ResolveHandler(action);
            handler.Process(action);
        }

        #endregion Private

        #region Bindings

        private void OnReceived(IProcessable processable)
        {
            if (processable is not IAction action)
            {
                return;
            }

            ProcessFromClient(action);
        }

        #endregion Bindings
    }
}