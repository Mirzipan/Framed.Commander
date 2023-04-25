using System;
using System.Collections.Generic;

namespace Mirzipan.Heist.Meta
{
    public interface IActionIndexer : IMetadataIndexer
    {
        Type GetHandler(Type actionType);
        IEnumerable<Type> GetHandlers();
    }
}