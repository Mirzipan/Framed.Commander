using System;

namespace Mirzipan.Heist.Networking
{
    public interface IIncomingCommands : IDisposable
    {
        event CommandReceived OnCommandReceived;
    }    
}
