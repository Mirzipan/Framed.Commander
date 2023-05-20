using System;

namespace Mirzipan.Heist.Networking
{
    public interface IOutgoingActions : IDisposable
    {
        void Send(IAction action);
    }
}