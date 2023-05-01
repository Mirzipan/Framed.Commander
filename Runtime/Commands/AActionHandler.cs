using System.Collections.Generic;
using Mirzipan.Heist.Processors;
using Reflex.Attributes;

namespace Mirzipan.Heist.Commands
{
    public abstract class AActionHandler<T> : IActionHandler where T : IAction
    {
        private readonly List<IProcessable> _data = new();

        [Inject]
        internal IServerProcessor Processor;

        #region Validation

        public ValidationResult Validate(IAction action)
        {
            return Validate((T)action);
        }

        protected abstract ValidationResult Validate(T action);

        protected static ValidationResult Pass() => ValidationResult.Pass;

        protected static ValidationResult Fail(uint reason) => new ValidationResult(reason);

        #endregion Validation

        #region Processing
        
        public void Process(IAction action)
        {
            _data.Clear();
            Process((T)action);

            for (int i = 0; i < _data.Count; i++)
            {
                var entry = _data[i];
                if (entry is IAction entryAction)
                {
                    Processor.Process(entryAction);
                }

                if (entry is ICommand entryCommand)
                {
                    Processor.Execute(entryCommand);
                }
            }
            
            _data.Clear();
        }

        protected abstract void Process(T action);

        protected void Enqueue(IProcessable processable) => _data.Add(processable);

        #endregion Processing
    }
}