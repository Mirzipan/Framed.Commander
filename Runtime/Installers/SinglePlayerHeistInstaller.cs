using Mirzipan.Heist.Reflex;
using Reflex.Core;
using UnityEngine;

namespace Mirzipan.Heist.Installers
{
    public class SinglePlayerHeistInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            descriptor.AddMetadataIndexers();
            
            descriptor.AddNullNetwork();
            descriptor.AddClientProcessor();
            descriptor.AddServerProcessor();
        }
    }
}