using Mirzipan.Heist.Processors;
using Reflex.Attributes;
using UnityEngine;

namespace Mirzipan.Heist.Unity
{
    public class ProcessorTicker : MonoBehaviour
    {
        [Inject]
        private IClientProcessor _processor;

        private void FixedUpdate()
        {
            _processor?.Tick();
        }
    }
}