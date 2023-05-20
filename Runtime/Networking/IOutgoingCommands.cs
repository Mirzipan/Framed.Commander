using System;

namespace Mirzipan.Heist.Networking
{
    public interface IOutgoingCommands : IDisposable
    {
        void Send(ICommand command, int[] clientIds, bool sendToAll);
    }
}