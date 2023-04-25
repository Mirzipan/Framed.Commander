using System;

namespace Mirzipan.Heist.Meta
{
    public interface IMetadataIndexer
    {
        void Index(Type type);
    }
}