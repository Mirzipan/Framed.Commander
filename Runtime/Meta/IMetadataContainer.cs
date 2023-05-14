using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mirzipan.Heist.Meta
{
    public interface IMetadataContainer
    {
        IReadOnlyCollection<Assembly> KnownAssemblies { get; }
        void Add(IEnumerable<Assembly> assemblies);
        void Add(params Assembly[] assemblies);
        void Add(Assembly assembly);
        IEnumerable<Type> GetAllTypes();
    }
}