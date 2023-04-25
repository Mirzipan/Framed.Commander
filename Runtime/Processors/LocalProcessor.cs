using System;
using System.Collections.Generic;
using Mirzipan.Heist.Commands;
using UnityEngine;

namespace Mirzipan.Heist.Processors
{
    public class LocalProcessor : IProcessor, IDisposable
    {
        private Queue<IAction> _queue = new();

        private IResolver _resolver;

        #region Lifecycle

        public LocalProcessor(IResolver container)
        {
            _resolver = container;
        }

        public void Tick()
        {
            while (_queue.Count > 0)
            {
                var input = _queue.Dequeue();

                var handler = _resolver.ResolveHandler(input);
                handler.Process(input);
            }
        }

        public void Dispose()
        {
            _resolver = null;
            _queue.Clear();
            _queue = null;
        }

        #endregion Lifecycle

        #region Public

        public ValidationResult Validate(IAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
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
    }
}