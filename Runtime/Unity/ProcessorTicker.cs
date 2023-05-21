using Mirzipan.Heist.Processors;
using Reflex.Attributes;
using UnityEngine;

namespace Mirzipan.Heist.Unity
{
    public class ProcessorTicker : MonoBehaviour
    {
        private const string LogTag = "Ticker";
        
        [Inject]
        private ClientProcessor _client;
        [Inject]
        private ServerProcessor _server;

        private void Start()
        {
            if (_client != null)
            {
                HeistLogger.Info(LogTag, $"{nameof(ClientProcessor)} exists.");
            }
            
            if (_server != null)
            {
                HeistLogger.Info(LogTag, $"{nameof(ServerProcessor)} exists.");
            }
        }

        private void FixedUpdate()
        {
            _client?.Tick();
        }
    }
}