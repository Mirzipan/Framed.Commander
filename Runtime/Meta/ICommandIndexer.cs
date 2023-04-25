using System;
using System.Collections.Generic;

namespace Mirzipan.Heist.Meta
{
    public interface ICommandIndexer : IMetadataIndexer
    {
        Type GetReceiver(Type commandType);
        IEnumerable<Type> GetReceivers();
    }
}