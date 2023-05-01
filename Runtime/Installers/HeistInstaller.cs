using Mirzipan.Heist.Meta;
using Mirzipan.Heist.Networking;
using Mirzipan.Heist.Processors;
using Reflex.Core;
using UnityEngine;

namespace Mirzipan.Heist.Installers
{
    public sealed class HeistInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            InstallIndexers(descriptor);

            descriptor.AddSingleton(typeof(Resolver), typeof(IResolver));
            descriptor.AddSingleton(typeof(NullNetwork), typeof(INetwork));
            descriptor.AddSingleton(typeof(ClientProcessor), typeof(IClientProcessor));
            descriptor.AddSingleton(typeof(ServerProcessor), typeof(IServerProcessor));
        }

        private void InstallIndexers(ContainerDescriptor descriptor)
        {
            descriptor.AddInstance(new ActionIndexer(), typeof(IActionIndexer), typeof(IMetadataIndexer));
            descriptor.AddInstance(new CommandIndexer(), typeof(ICommandIndexer), typeof(IMetadataIndexer));

            descriptor.AddSingleton(typeof(MetadataContainer), typeof(IMetadataContainer));
        }
    }
}