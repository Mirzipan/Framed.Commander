using System;
using System.Collections.Generic;
using Mirzipan.Heist.Commands;
using Mirzipan.Heist.Networking;
using UnityEngine;

namespace Mirzipan.Heist.Processors
{
    public sealed class ClientProcessor : IClientProcessor, IDisposable
    {
        private Queue<ICommand> _commandQueue = new();
        
        private INetwork _network;
        private IResolver _resolver;

        #region Lifecycle

        public ClientProcessor(INetwork network, IResolver resolver)
        {
            _network = network;
            _resolver = resolver;

            _network.OnReceived += OnReceived;
        }

        public void Tick()
        {
            while (_commandQueue.Count > 0)
            {
                ICommand command = _commandQueue.Dequeue();
                ICommandReceiver handler = _resolver.ResolveReceiver(command);
                
                handler.Execute(command);
            }
        }

        public void Dispose()
        {
            _network.OnReceived -= OnReceived;
            _network = null;
            _resolver = null;
            
            _commandQueue.Clear();
            _commandQueue = null;
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
                Debug.LogError($"Validation of {action} failed. code={result.Code}");
                return;
            }

            _network?.Send(action);
        }

        #endregion Public

        #region Bindings

        private void OnReceived(IProcessable processable)
        {
            if (processable is not ICommand command)
            {
                return;
            }
            
            _commandQueue.Enqueue(command);
        }

        #endregion Bindings
    }
}