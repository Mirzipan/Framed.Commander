using System;
using Mirzipan.Heist.Meta;
using Mirzipan.Heist.Networking;
using Mirzipan.Heist.Processors;
using Reflex.Core;

namespace Mirzipan.Heist.Reflex
{
    public static class ContainerDescriptorExtensions
    {
        /// <summary>
        /// Adds IMetadataContainer, IResolver, IActionIndexer, ICommandIndexer, and all actions and commands.
        /// </summary>
        /// <param name="this"></param>
        public static void AddMetadataIndexers(this ContainerDescriptor @this)
        {
            var metaContainer = new MetadataContainer();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            metaContainer.Add(assemblies);
            
            @this.AddInstance(metaContainer, typeof(IMetadataContainer));
            @this.AddSingleton(typeof(Resolver), typeof(IResolver));

            AddIndexers(@this, metaContainer);
        }

        /// <summary>
        /// Adds an implementation of <see cref="INetwork"/> suitable for local single-player.
        /// Do not call this if you want another type of network.
        /// </summary>
        /// <param name="this"></param>
        public static void AddNullNetwork(this ContainerDescriptor @this)
        {
            @this.AddSingleton(typeof(NullNetwork), typeof(INetwork));
        }

        /// <summary>
        /// Adds default implementation of <see cref="IClientProcessor"/>.
        /// Do not call this if you want your custom client processor.
        /// </summary>
        /// <param name="this"></param>
        public static void AddClientProcessor(this ContainerDescriptor @this)
        {
            @this.AddSingleton(typeof(ClientProcessor), typeof(IClientProcessor));
        }

        /// <summary>
        /// Adds default implementation of <see cref="IServerProcessor"/>.
        /// Do not call this if you want your custom server processor.
        /// </summary>
        /// <param name="this"></param>
        public static void AddServerProcessor(this ContainerDescriptor @this)
        {
            @this.AddSingleton(typeof(ServerProcessor), typeof(IServerProcessor));
        }

        private static void AddIndexers(ContainerDescriptor descriptor, IMetadataContainer container)
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
    }
}