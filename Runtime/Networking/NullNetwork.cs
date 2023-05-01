using Mirzipan.Heist.Commands;

namespace Mirzipan.Heist.Networking
{
    public sealed class NullNetwork : INetwork
    {
        public event ProcessableReceived OnReceived;
        
        public void Send(IProcessable processable)
        {
            OnReceived?.Invoke(processable);
        }
    }
}