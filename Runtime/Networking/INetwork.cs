using Mirzipan.Heist.Commands;

namespace Mirzipan.Heist.Networking
{
    public interface INetwork
    {
        event ProcessableReceived OnReceived;
        void Send(IProcessable processable);
    }
}