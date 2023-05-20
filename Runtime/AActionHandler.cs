using System.Collections.Generic;
using Mirzipan.Heist.Processors;
using Reflex.Attributes;

namespace Mirzipan.Heist
{
    public abstract class AActionHandler<T> : IActionHandler where T : IAction
    {
        #region Inner Types

        private struct Processable
        {
            public int[] ClientIds;
            public IAction Action;
            public ICommand Command;
            public ExecuteOn Target;

            public Processable(IAction action)
            {
                ClientIds = null;
                Action = action;
                Command = null;
                Target = ExecuteOn.None;
            }

            public Processable(ICommand command, int[] clientIds, ExecuteOn target)
            {
                ClientIds = clientIds;
                Action = null;
                Command = command;
                Target = target;
            }
        }

        #endregion Inner Types

        private readonly List<Processable> _processingQueue = new();

        [Inject]
        internal IServerProcessor Processor;

        #region Validation

        public ValidationResult Validate(IAction action, int clientId, ValidationOptions options)
        {
            return Validate((T)action, clientId, options);
        }

        protected abstract ValidationResult Validate(T action, int clientId, ValidationOptions options);

        protected static ValidationResult Pass() => ValidationResult.Pass;

        protected static ValidationResult Fail(uint reason) => new ValidationResult(reason);

        #endregion Validation

        #region Processing

        public void Process(IAction action, int clientId)
        {
            _processingQueue.Clear();
            Process((T)action, clientId);

            for (int i = 0; i < _processingQueue.Count; i++)
            {
                var entry = _processingQueue[i];
                if (entry.Action != null)
                {
                    Processor.ProcessServerAction(entry.Action);
                }

                if (entry.Command != null)
                {
                    Processor.Execute(entry.Command, entry.ClientIds, entry.Target);
                }
            }

            _processingQueue.Clear();
        }

        protected abstract void Process(T action, int clientId);

        protected void Enqueue(IAction action) => _processingQueue.Add(new Processable(action));

        protected void Enqueue(ICommand command, int[] clientIds, ExecuteOn target)
        {
            _processingQueue.Add(new Processable(command, clientIds, target));
        }

        protected void Enqueue(ICommand command, int clientId, ExecuteOn target)
        {
            Enqueue(command, new[] { clientId }, target);
        }

        protected void Enqueue(ICommand command, ExecuteOn target)
        {
            Enqueue(command, null, target);
        }

        #endregion Processing
    }
}