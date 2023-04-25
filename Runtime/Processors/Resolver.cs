using System;
using Mirzipan.Heist.Commands;
using Mirzipan.Heist.Meta;
using Reflex.Core;

namespace Mirzipan.Heist.Processors
{
    internal class Resolver : IResolver, IDisposable
    {
        private Container _container;
        private IActionIndexer _actionIndexer;
        private ICommandIndexer _commandIndexer;

        #region Lifecycle

        public Resolver(Container parent)
        {
            var container = parent.Resolve<IMetadataContainer>();
            container.Process();
            
            _actionIndexer = parent.Resolve<IActionIndexer>();
            _commandIndexer = parent.Resolve<ICommandIndexer>();
            _container = parent.Scope("commands", InstallBindings);
        }

        public void Dispose()
        {
            _container = null;
            _actionIndexer = null;
            _commandIndexer = null;
        }

        #endregion Lifecycle

        private void InstallBindings(ContainerDescriptor descriptor)
        {
            foreach (var entry in _actionIndexer.GetHandlers())
            {
                descriptor.AddSingleton(entry);
            }
            
            foreach (var entry in _commandIndexer.GetReceivers())
            {
                descriptor.AddSingleton(entry);
            }
        }

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
    }
}