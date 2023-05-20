using Reflex.Core;
using UnityEngine;

namespace Mirzipan.Heist.Reflex
{
    public class SinglePlayerHeistInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            descriptor.AddMetadataIndexers();
            
            descriptor.AddLoopbackQueue();
            descriptor.AddClientProcessor();
            descriptor.AddServerProcessor();
        }
    }
}