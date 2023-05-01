using System;
using System.Collections.Generic;
using System.Linq;
using Reflex.Core;

namespace Mirzipan.Heist.Meta
{
    public sealed class MetadataContainer : IMetadataContainer, IDisposable
    {
        private Container _container;
        
        #region Lifecycle

        public MetadataContainer(Container container)
        {
            _container = container;
        }

        public void Process()
        {
            var indexers = _container.All<IMetadataIndexer>().ToList();
            
            foreach (Type entry in GetAllTypes())
            {
                foreach (var indexer in indexers)
                {
                    indexer.Index(entry);
                }
            }
        }

        public void Dispose()
        {
            _container = null;
        }

        #endregion Lifecycle
        
        #region Private

        private IEnumerable<Type> GetAllTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    yield return type;
                }
            }
        }

        #endregion Private
    }
}