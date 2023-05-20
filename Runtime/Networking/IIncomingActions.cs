using System;

namespace Mirzipan.Heist.Networking
{
    public interface IIncomingActions : IDisposable
    {
        event ActionReceived OnActionReceived;
    }
}