using System;
using Mirzipan.Heist.Meta;
using Mirzipan.Heist.Networking;
using Mirzipan.Heist.Processors;
using Reflex.Core;

namespace Mirzipan.Heist.Installers
{
    public static class HeistInstaller
    {
        /// <summary>
        /// Adds IMetadataContainer, IResolver, IActionIndexer, ICommandIndexer, and all actions and commands.
        /// </summary>
        /// <param name="descriptor"></param>
        public static void InstallCommon(ContainerDescriptor descriptor)
        {
            var metaContainer = new MetadataContainer();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            metaContainer.Add(assemblies);
            
            descriptor.AddInstance(metaContainer, typeof(IMetadataContainer));
            descriptor.AddSingleton(typeof(Resolver), typeof(IResolver));

            InstallIndexers(descriptor, metaContainer);
        }

        private static void InstallIndexers(ContainerDescriptor descriptor, IMetadataContainer container)
        {
            var actionIndexer = new ActionIndexer();
            var commandIndexer = new CommandIndexer();

            foreach (var entry in container.GetAllTypes())
            {
                actionIndexer.Index(entry);
                commandIndexer.Index(entry);
            }
            
            descriptor.AddInstance(actionIndexer, typeof(IActionIndexer));
            foreach (var entry in actionIndexer.GetHandlers())
            {
                descriptor.AddSingleton(entry);
            }
            
            descriptor.AddInstance(commandIndexer, typeof(ICommandIndexer));
            foreach (var entry in commandIndexer.GetReceivers())
            {
                descriptor.AddSingleton(entry);
            }
        }

        /// <summary>
        /// Adds an implementation of <see cref="INetwork"/> suitable for local single-player.
        /// Do not call this if you want another type of network.
        /// </summary>
        /// <param name="descriptor"></param>
        public static void InstallNullNetwork(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(NullNetwork), typeof(INetwork));
        }

        /// <summary>
        /// Adds default implementation of <see cref="IClientProcessor"/>.
        /// Do not call this if you want your custom client processor.
        /// </summary>
        /// <param name="descriptor"></param>
        public static void InstallClient(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(ClientProcessor), typeof(IClientProcessor));
        }

        /// <summary>
        /// Adds default implementation of <see cref="IServerProcessor"/>.
        /// Do not call this if you want your custom server processor.
        /// </summary>
        /// <param name="descriptor"></param>
        public static void InstallServer(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(ServerProcessor), typeof(IServerProcessor));
        }
    }
}