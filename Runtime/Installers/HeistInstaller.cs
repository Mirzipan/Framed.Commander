using System;
using Mirzipan.Heist.Networking;
using Mirzipan.Heist.Processors;
using Mirzipan.Heist.Reflex;
using Reflex.Core;

namespace Mirzipan.Heist.Installers
{
    public static class HeistInstaller
    {
        /// <summary>
        /// Adds IMetadataContainer, IResolver, IActionIndexer, ICommandIndexer, and all actions and commands.
        /// </summary>
        /// <param name="descriptor"></param>
        [Obsolete("Use AddMetadataIndexers extension method.")]
        public static void InstallCommon(ContainerDescriptor descriptor)
        {
            descriptor.AddMetadataIndexers();
        }

        /// <summary>
        /// Adds an implementation of <see cref="INetwork"/> suitable for local single-player.
        /// Do not call this if you want another type of network.
        /// </summary>
        /// <param name="descriptor"></param>
        [Obsolete("Use AddNullNetwork extension method.")]
        public static void InstallNullNetwork(ContainerDescriptor descriptor)
        {
            descriptor.AddNullNetwork();
        }

        /// <summary>
        /// Adds default implementation of <see cref="IClientProcessor"/>.
        /// Do not call this if you want your custom client processor.
        /// </summary>
        /// <param name="descriptor"></param>
        [Obsolete("Use AddClientProcessor extension method.")]
        public static void InstallClient(ContainerDescriptor descriptor)
        {
            descriptor.AddClientProcessor();
        }

        /// <summary>
        /// Adds default implementation of <see cref="IServerProcessor"/>.
        /// Do not call this if you want your custom server processor.
        /// </summary>
        /// <param name="descriptor"></param>
        [Obsolete("Use AddServerProcessor extension method.")]
        public static void InstallServer(ContainerDescriptor descriptor)
        {
            descriptor.AddServerProcessor();
        }
    }
}