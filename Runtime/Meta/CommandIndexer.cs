using System;
using System.Collections.Generic;
using Mirzipan.Extensions.Reflection;

namespace Mirzipan.Heist.Meta
{
    public sealed class CommandIndexer : ICommandIndexer, IDisposable
    {
        private static readonly Type ContainerType = typeof(ICommandContainer);
        private static readonly Type DataType = typeof(ICommand);
        private static readonly Type ProcessorType = typeof(ICommandReceiver);
        
        private readonly Dictionary<Type, Type> _commandToReceiver = new();
        
        public void Index(Type type)
        {
            if (!type.IsClass)
            {
                return;
            }
            
            if (!ContainerType.IsAssignableFrom(type))
            {
                return;
            }
            
            var commandType = type.GetNestedType(DataType);
            var receiverType = type.GetNestedType(ProcessorType);
            if (commandType != null && receiverType != null)
            {
                _commandToReceiver[commandType] = receiverType;
            }
        }

        public Type GetReceiver(Type commandType)
        {
            return _commandToReceiver.TryGetValue(commandType, out var result) ? result : null;
        }

        public IEnumerable<Type> GetReceivers()
        {
            foreach (var kvp in _commandToReceiver)
            {
                yield return kvp.Value;
            }
        }

        public void Dispose()
        {
            _commandToReceiver.Clear();
        }
    }
}