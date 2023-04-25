using Mirzipan.Heist.Meta;
using Mirzipan.Heist.Processors;
using Reflex.Core;
using UnityEngine;

namespace Mirzipan.Heist.Installers
{
    public class LocalProcessorInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            InstallMetadata(descriptor);

            descriptor.AddSingleton(typeof(Resolver), typeof(IResolver));
            descriptor.AddSingleton(typeof(LocalProcessor), typeof(IProcessor));
        }

        private void InstallMetadata(ContainerDescriptor descriptor)
        {
            descriptor.AddInstance(new ActionIndexer(), typeof(IActionIndexer), typeof(IMetadataIndexer));
            descriptor.AddInstance(new CommandIndexer(), typeof(ICommandIndexer), typeof(IMetadataIndexer));

            descriptor.AddSingleton(typeof(MetadataContainer), typeof(IMetadataContainer));
        }
    }
}