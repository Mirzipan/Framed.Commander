using System;

namespace Mirzipan.Heist.Commands
{
    [Flags]
    public enum ValidationOptions
    {
        None = 0,
        SenderIsServer = 1 << 0,
    }
}