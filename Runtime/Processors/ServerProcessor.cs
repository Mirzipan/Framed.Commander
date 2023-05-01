using System;
using System.Collections.Generic;
using Mirzipan.Heist.Commands;
using Mirzipan.Heist.Networking;
using UnityEngine;

namespace Mirzipan.Heist.Processors
{
    public sealed class ServerProcessor : IServerProcessor, IDisposable
    {
        private Queue<IAction> _queue = new();
        
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
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var handler = _resolver.ResolveHandler(action);
            return handler.Validate(action);
        }

        public void Process(IAction action)
        {
            var result = Validate(action);
            if (!result.Success)
            {
                // TODO: do we need to log this?
                Debug.LogError($"Validation of {action} failed. code={result.Code}");
                return;
            }

            _queue.Enqueue(action);
        }

        public void Execute(ICommand command)
        {
            var receiver = _resolver.ResolveReceiver(command);
            receiver.Execute(command);
        }

        #endregion Public

        #region Bindings

        private void OnReceived(IProcessable processable)
        {
            if (processable is not IAction action)
            {
                return;
            }

            Process(action);
        }

        #endregion Bindings
    }
}