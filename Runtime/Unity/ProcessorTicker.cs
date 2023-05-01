using Mirzipan.Heist.Processors;
using Reflex.Attributes;
using UnityEngine;

namespace Mirzipan.Heist.Unity
{
    public class ProcessorTicker : MonoBehaviour
    {
        [Inject]
        private IClientProcessor _client;
        [Inject]
        private IServerProcessor _server;

        private void Start()
        {
            if (_client != null)
            {
                Debug.Log("[Ticker] client exists.");
            }
            
            if (_server != null)
            {
                Debug.Log("[Ticker] server exists.");
            }
        }

        private void FixedUpdate()
        {
            _client?.Tick();
        }
    }
}