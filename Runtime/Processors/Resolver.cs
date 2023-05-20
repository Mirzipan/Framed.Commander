using System;
using Mirzipan.Heist.Meta;
using Reflex.Attributes;
using Reflex.Core;

namespace Mirzipan.Heist.Processors
{
    public sealed class Resolver : IResolver, IDisposable
    {
        [Inject]
        private Container _container;
        [Inject]
        private IActionIndexer _actionIndexer;
        [Inject]
        private ICommandIndexer _commandIndexer;

        #region Lifecycle

        public void Dispose()
        {
            _actionIndexer = null;
            _commandIndexer = null;
        }

        #endregion Lifecycle

        #region Queries

        public IActionHandler ResolveHandler(IAction action)
        {
            var type = _actionIndexer.GetHandler(action.GetType());
            return _container.Resolve(type) as IActionHandler;
        }

        public ICommandReceiver ResolveReceiver(ICommand command)
        {
            var type = _commandIndexer.GetReceiver(command.GetType());
            return _container.Resolve(type) as ICommandReceiver;
        }

        #endregion Queries
    }
}