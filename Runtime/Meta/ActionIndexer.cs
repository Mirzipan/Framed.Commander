using System;
using System.Collections.Generic;
using Mirzipan.Extensions.Reflection;
using Mirzipan.Heist.Commands;

namespace Mirzipan.Heist.Meta
{
    public sealed class ActionIndexer : IActionIndexer, IDisposable
    {
        private static readonly Type ContainerType = typeof(IActionContainer);
        private static readonly Type DataType = typeof(IAction);
        private static readonly Type ProcessorType = typeof(IActionHandler);
        
        private readonly Dictionary<Type, Type> _actionToHandler = new();
        
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
            
            var actionType = type.GetNestedType(DataType);
            var handlerType = type.GetNestedType(ProcessorType);
            if (actionType != null && handlerType != null)
            {
                _actionToHandler[actionType] = handlerType;
            }
        }

        public Type GetHandler(Type actionType)
        {
            return _actionToHandler.TryGetValue(actionType, out var result) ? result : null;
        }

        public IEnumerable<Type> GetHandlers()
        {
            foreach (var kvp in _actionToHandler)
            {
                yield return kvp.Value;
            }
        }

        public void Dispose()
        {
            _actionToHandler.Clear();
        }
    }
}